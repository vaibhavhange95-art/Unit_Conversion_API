using Unit_Conversion_API.Models;
using Unit_Conversion_API.Services.UnitResolver.Interfaces;

namespace Unit_Conversion_API.Services.UnitResolver.Implementation
{
    public class DynamicUnitResolverService : IUnitResolverService
    {
        public UnitDefinition? ResolveUnit(
            string category,
            string unitName)
        {
            if (category.Equals(
                "Length",
                StringComparison.OrdinalIgnoreCase))
            {
                return ResolveLengthUnit(unitName);
            }

            if (category.Equals(
                "Weight",
                StringComparison.OrdinalIgnoreCase))
            {
                return ResolveWeightUnit(unitName);
            }

            return null;
        }


        private UnitDefinition? ResolveLengthUnit(
            string unitName)
        {
            return unitName.ToUpper() switch
            {
                "M" or "METER" or "METRE" =>
                    new UnitDefinition
                    {
                        Name = "Meter",
                        ToBaseFactor = 1
                    },

                "CM" or "CENTIMETER" =>
                    new UnitDefinition
                    {
                        Name = "Centimeter",
                        ToBaseFactor = 0.01
                    },

                "MM" or "MILLIMETER" =>
                    new UnitDefinition
                    {
                        Name = "Millimeter",
                        ToBaseFactor = 0.001
                    },

                "KM" or "KILOMETER" =>
                    new UnitDefinition
                    {
                        Name = "Kilometer",
                        ToBaseFactor = 1000
                    },

                "FT" or "FOOT" =>
                    new UnitDefinition
                    {
                        Name = "Foot",
                        ToBaseFactor = 0.3048
                    },

                _ => null
            };
        }


        private UnitDefinition? ResolveWeightUnit(
            string unitName)
        {
            return unitName.ToUpper() switch
            {
                "KG" or "KILOGRAM" =>
                    new UnitDefinition
                    {
                        Name = "Kilogram",
                        ToBaseFactor = 1
                    },

                "G" or "GRAM" =>
                    new UnitDefinition
                    {
                        Name = "Gram",
                        ToBaseFactor = 0.001
                    },

                "LB" or "POUND" =>
                    new UnitDefinition
                    {
                        Name = "Pound",
                        ToBaseFactor = 0.453592
                    },

                _ => null
            };
        }
    }
}