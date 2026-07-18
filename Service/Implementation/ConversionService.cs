using Unit_Conversion_API.DTOs;
using Unit_Conversion_API.Exceptions;
using Unit_Conversion_API.Repositories.Interfaces;
using Unit_Conversion_API.Services.Interfaces;

namespace Unit_Conversion_API.Services.Implementation
{
    public class ConversionService : IConversionService
    {
        private readonly IUnitRepository _unitRepository;

        public ConversionService(
            IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
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


            // Try to get a stored formula first
            if (_unitRepository.TryGetFormula(request.Category, fromUnit.Name, toUnit.Name, out var formula))
            {
                convertedValue = EvaluateFormula(formula.Formula, request.Value);
            }
            else
            {
                // Fallback to base-factor conversion
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
            // Very small expression evaluator: replace 'x' and use DataTable.Compute as a simple option
            // Note: DataTable is available and sufficient for basic arithmetic here.
            try
            {
                var expr = formula.Replace("x", x.ToString(System.Globalization.CultureInfo.InvariantCulture));
                System.Data.DataTable table = new System.Data.DataTable();
                var result = table.Compute(expr, string.Empty);
                return System.Convert.ToDouble(result);
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