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



            double convertedValue;


            // Temperature requires formula based conversion
            if (request.Category.Equals(
                "Temperature",
                StringComparison.OrdinalIgnoreCase))
            {
                convertedValue = ConvertTemperature(
                    request.Value,
                    fromUnit.Name,
                    toUnit.Name);
            }
            else
            {
                // Convert:
                // Source -> Base Unit -> Target

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