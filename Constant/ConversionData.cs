using Unit_Conversion_API.Models;

namespace Unit_Conversion_API.Constants
{
    public static class ConversionData
    {
        public static readonly List<ConversionCategory> Categories =
            new()
            {
                new ConversionCategory
                {
                    Name = "Length",
                    Units =
                    [
                        new UnitDefinition
                        {
                            Name = "Meter",
                            ToBaseFactor = 1
                        },
                        new UnitDefinition
                        {
                            Name = "Kilometer",
                            ToBaseFactor = 1000
                        },
                        new UnitDefinition
                        {
                            Name = "Foot",
                            ToBaseFactor = 0.3048
                        },
                        new UnitDefinition
                        {
                            Name = "Mile",
                            ToBaseFactor = 1609.34
                        }
                    ]
                },

                new ConversionCategory
                {
                    Name = "Weight",
                    Units =
                    [
                        new UnitDefinition
                        {
                            Name = "Kilogram",
                            ToBaseFactor = 1
                        },
                        new UnitDefinition
                        {
                            Name = "Gram",
                            ToBaseFactor = 0.001
                        },
                        new UnitDefinition
                        {
                            Name = "Pound",
                            ToBaseFactor = 0.453592
                        }
                    ]
                },

                new ConversionCategory
                {
                    Name = "Temperature",
                    Units =
                    [
                        new UnitDefinition
                        {
                            Name = "Celsius",
                            ToBaseFactor = 1
                        },
                        new UnitDefinition
                        {
                            Name = "Fahrenheit",
                            ToBaseFactor = 1
                        },
                        new UnitDefinition
                        {
                            Name = "Kelvin",
                            ToBaseFactor = 1
                        }
                    ]
                }
            };
    }
}