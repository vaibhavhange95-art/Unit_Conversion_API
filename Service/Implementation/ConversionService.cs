using Unit_Conversion_API.Constants;
using Unit_Conversion_API.DTOs;
using Unit_Conversion_API.Services.Interfaces;
using Unit_Conversion_API.Exceptions;

namespace Unit_Conversion_API.Services.Implementation
{
    public class ConversionService : IConversionService
    {
        public ConversionResponseDto Convert(ConversionRequestDto request)
        {
            var category = ConversionData.Categories
                .FirstOrDefault(x =>
                    x.Name.Equals(request.Category,
                    StringComparison.OrdinalIgnoreCase));

            if (category == null)
            {
                throw new ConversionException(
       $"Conversion category '{request.Category}' is not supported.");
            }


            var fromUnit = category.Units
                .FirstOrDefault(x =>
                    x.Name.Equals(request.FromUnit,
                    StringComparison.OrdinalIgnoreCase));


            var toUnit = category.Units
                .FirstOrDefault(x =>
                    x.Name.Equals(request.ToUnit,
                    StringComparison.OrdinalIgnoreCase));


            if (fromUnit == null || toUnit == null)
            {
                throw new ConversionException("Source or target unit is not supported.");
            }


            double convertedValue;


            if (category.Name.Equals("Temperature",
                StringComparison.OrdinalIgnoreCase))
            {
                convertedValue = ConvertTemperature(
                    request.Value,
                    fromUnit.Name,
                    toUnit.Name);
            }
            else
            {
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
            if (fromUnit.Equals(toUnit,
                StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }


            if (fromUnit == "Celsius" &&
                toUnit == "Fahrenheit")
            {
                return (value * 9 / 5) + 32;
            }


            if (fromUnit == "Fahrenheit" &&
                toUnit == "Celsius")
            {
                return (value - 32) * 5 / 9;
            }


            if (fromUnit == "Celsius" &&
                toUnit == "Kelvin")
            {
                return value + 273.15;
            }


            if (fromUnit == "Kelvin" &&
                toUnit == "Celsius")
            {
                return value - 273.15;
            }


            if (fromUnit == "Fahrenheit" &&
                toUnit == "Kelvin")
            {
                return ((value - 32) * 5 / 9) + 273.15;
            }


            if (fromUnit == "Kelvin" &&
                toUnit == "Fahrenheit")
            {
                return ((value - 273.15) * 9 / 5) + 32;
            }


            throw new ConversionException(
                "Temperature conversion is not supported between these units.");
        }
    }
}