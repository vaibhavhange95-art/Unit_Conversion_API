using Unit_Conversion_API.DTOs;
using Unit_Conversion_API.Exceptions;
using Unit_Conversion_API.Repositories.Interfaces;
using Unit_Conversion_API.Services.Interfaces;

namespace Unit_Conversion_API.Services.Implementation
{
    public class ConversionService : IConversionService
    {
        private readonly IUnitRepository _unitRepository;
        private readonly FormulaCategoryRegistry _formulaRegistry;

        public ConversionService(
            IUnitRepository unitRepository,
            FormulaCategoryRegistry formulaRegistry)
        {
            _unitRepository = unitRepository;
            _formulaRegistry = formulaRegistry;
        }


        public ConversionResponseDto Convert(
            ConversionRequestDto request)
        {
            // Get units from repository
            var units = _unitRepository.GetUnits(
                request.Category);


            if (!units.Any())
            {
                throw new ConversionException(
                    $"Conversion category '{request.Category}' is not supported.");
            }


            // Find source unit
            var fromUnit = _unitRepository.GetUnit(
                request.Category,
                request.FromUnit);


            // Find target unit
            var toUnit = _unitRepository.GetUnit(
                request.Category,
                request.ToUnit);



            if (fromUnit == null || toUnit == null)
            {
                throw new ConversionException(
                    $"Unit conversion is not supported from '{request.FromUnit}' to '{request.ToUnit}'.");
            }
            request.FromUnit = fromUnit.Name;
            request.ToUnit = toUnit.Name;

            double convertedValue;


            // Conversion flows through the FormulaCategoryRegistry first
            if (_formulaRegistry.RequiresFormula(request.Category))
            {
                if (_formulaRegistry.TryGetFormula(request.Category, fromUnit.Name, toUnit.Name, out var formula))
                {
                    convertedValue = EvaluateFormula(formula, request.Value);
                }
                else
                {
                    throw new ConversionException($"No conversion formula found for category '{request.Category}' and units '{fromUnit.Name}' -> '{toUnit.Name}'. Please add a conversion formula.");
                }
            }
            else
            {
                // Default factor-based conversion
                convertedValue =
                    request.Value *
                    fromUnit.ToBaseFactor /
                    toUnit.ToBaseFactor;
            }



            return new ConversionResponseDto
            {
                OriginalValue = request.Value,
                FromUnit = request.FromUnit,
                ToUnit = request.ToUnit,
                ConvertedValue = convertedValue,
                Category = request.Category
            };
        }

        private double EvaluateFormula(string formula, double x)
        {
            // Enhanced evaluator to support log10(...) and 10^(...) patterns in addition to basic arithmetic.
            try
            {
                // Replace x with numeric literal using invariant culture
                var valueStr = x.ToString(System.Globalization.CultureInfo.InvariantCulture);
                var expr = formula.Replace("x", valueStr);

                // Helper to evaluate basic arithmetic expressions using DataTable.Compute
                double EvalArithmetic(string arithmeticExpr)
                {
                    var table = new System.Data.DataTable();
                    var raw = table.Compute(arithmeticExpr, string.Empty);
                    return System.Convert.ToDouble(raw);
                }

                // Replace all occurrences of log10(<inner>) by computed numeric value
                while (true)
                {
                    var idx = expr.IndexOf("log10(", StringComparison.OrdinalIgnoreCase);
                    if (idx < 0) break;

                    // find matching ')'
                    var start = idx + "log10(".Length;
                    var depth = 1;
                    var i = start;
                    for (; i < expr.Length; i++)
                    {
                        if (expr[i] == '(') depth++;
                        else if (expr[i] == ')') depth--;
                        if (depth == 0) break;
                    }

                    if (i >= expr.Length) throw new ConversionException($"Malformed log10(...) in formula '{formula}'.");

                    var inner = expr.Substring(start, i - start);
                    var innerVal = EvalArithmetic(inner);
                    var computed = System.Math.Log10(innerVal).ToString(System.Globalization.CultureInfo.InvariantCulture);

                    expr = expr.Substring(0, idx) + computed + expr.Substring(i + 1);
                }

                // Replace occurrences of 10^(<inner>) with Math.Pow(10, inner)
                while (true)
                {
                    var idx = expr.IndexOf("10^(",
                        StringComparison.OrdinalIgnoreCase);
                    if (idx < 0) break;

                    var start = idx + "10^(".Length;
                    var depth = 1;
                    var i = start;
                    for (; i < expr.Length; i++)
                    {
                        if (expr[i] == '(') depth++;
                        else if (expr[i] == ')') depth--;
                        if (depth == 0) break;
                    }

                    if (i >= expr.Length) throw new ConversionException($"Malformed 10^(...) in formula '{formula}'.");

                    var inner = expr.Substring(start, i - start);
                    var innerVal = EvalArithmetic(inner);
                    var computed = System.Math.Pow(10, innerVal).ToString(System.Globalization.CultureInfo.InvariantCulture);

                    expr = expr.Substring(0, idx) + computed + expr.Substring(i + 1);
                }

                // After replacing supported functions, evaluate the remaining arithmetic
                var final = EvalArithmetic(expr);
                return final;
            }
            catch (ConversionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ConversionException($"Failed to evaluate formula '{formula}': {ex.Message}");
            }
        }



        private double ConvertTemperature(
            double value,
            string fromUnit,
            string toUnit)
        {
            if (fromUnit.Equals(
                toUnit,
                StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }


            if (fromUnit.Equals("Celsius",
                StringComparison.OrdinalIgnoreCase)
                &&
                toUnit.Equals("Fahrenheit",
                StringComparison.OrdinalIgnoreCase))
            {
                return (value * 9 / 5) + 32;
            }


            if (fromUnit.Equals("Fahrenheit",
                StringComparison.OrdinalIgnoreCase)
                &&
                toUnit.Equals("Celsius",
                StringComparison.OrdinalIgnoreCase))
            {
                return (value - 32) * 5 / 9;
            }


            if (fromUnit.Equals("Celsius",
                StringComparison.OrdinalIgnoreCase)
                &&
                toUnit.Equals("Kelvin",
                StringComparison.OrdinalIgnoreCase))
            {
                return value + 273.15;
            }


            if (fromUnit.Equals("Kelvin",
                StringComparison.OrdinalIgnoreCase)
                &&
                toUnit.Equals("Celsius",
                StringComparison.OrdinalIgnoreCase))
            {
                return value - 273.15;
            }


            if (fromUnit.Equals("Fahrenheit",
                StringComparison.OrdinalIgnoreCase)
                &&
                toUnit.Equals("Kelvin",
                StringComparison.OrdinalIgnoreCase))
            {
                return ((value - 32) * 5 / 9) + 273.15;
            }


            if (fromUnit.Equals("Kelvin",
                StringComparison.OrdinalIgnoreCase)
                &&
                toUnit.Equals("Fahrenheit",
                StringComparison.OrdinalIgnoreCase))
            {
                return ((value - 273.15) * 9 / 5) + 32;
            }


            throw new ConversionException(
                $"Temperature conversion from '{fromUnit}' to '{toUnit}' is not supported.");
        }
    }
}