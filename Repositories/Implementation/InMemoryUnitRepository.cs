using Unit_Conversion_API.Models;
using Unit_Conversion_API.Repositories.Interfaces;

namespace Unit_Conversion_API.Repositories.Implementation
{
    public class InMemoryUnitRepository : IUnitRepository
    {
        private readonly Dictionary<string, List<UnitDefinition>> _units;


        public InMemoryUnitRepository()
        {
            _units = new Dictionary<string, List<UnitDefinition>>
    {
        {
            "Length",
            new List<UnitDefinition>
            {
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
                    Name = "Centimeter",
                    ToBaseFactor = 0.01
                },

                new UnitDefinition
                {
                    Name = "Millimeter",
                    ToBaseFactor = 0.001
                },

                new UnitDefinition
                {
                    Name = "Micrometer",
                    ToBaseFactor = 0.000001
                },

                new UnitDefinition
                {
                    Name = "Nanometer",
                    ToBaseFactor = 0.000000001
                },

                new UnitDefinition
                {
                    Name = "Inch",
                    ToBaseFactor = 0.0254
                },

                new UnitDefinition
                {
                    Name = "Foot",
                    ToBaseFactor = 0.3048
                },

                new UnitDefinition
                {
                    Name = "Yard",
                    ToBaseFactor = 0.9144
                },

                new UnitDefinition
                {
                    Name = "Mile",
                    ToBaseFactor = 1609.344
                }
            }
        },


        {
            "Weight",
            new List<UnitDefinition>
            {
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
                    Name = "Milligram",
                    ToBaseFactor = 0.000001
                },

                new UnitDefinition
                {
                    Name = "MetricTon",
                    ToBaseFactor = 1000
                },

                new UnitDefinition
                {
                    Name = "Pound",
                    ToBaseFactor = 0.45359237
                },

                new UnitDefinition
                {
                    Name = "Ounce",
                    ToBaseFactor = 0.0283495231
                }
            }
        },

                {
                    "Volume",
new List<UnitDefinition>
{
    new UnitDefinition
    {
        Name = "Liter",
        ToBaseFactor = 1
    },

    new UnitDefinition
    {
        Name = "Milliliter",
        ToBaseFactor = 0.001
    },

    new UnitDefinition
    {
        Name = "CubicMeter",
        ToBaseFactor = 1000
    },

    new UnitDefinition
    {
        Name = "Gallon",
        ToBaseFactor = 3.78541
    },

    new UnitDefinition
    {
        Name = "Quart",
        ToBaseFactor = 0.946353
    }
}
                },

        {
            "Temperature",
            new List<UnitDefinition>
            {
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
            }
        }
    };
        }

        public IEnumerable<string> SearchCategories(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return _units.Keys;
                }

                return _units.Keys
                    .Where(category =>
                        category.Contains(
                            searchText,
                            StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            catch (Exception)
            {
                return Enumerable.Empty<string>();
            }
        }
        public IEnumerable<UnitDefinition> GetUnits(
            string category)
        {
            try
            { 
                var matchedCategory = _units.Keys
             .FirstOrDefault(x =>
                 x.Contains(category, StringComparison.OrdinalIgnoreCase));

                if (matchedCategory != null)
                {
                    return _units[matchedCategory];
                }

                return Enumerable.Empty<UnitDefinition>();
            }
            catch (Exception )
            {
                return Enumerable.Empty<UnitDefinition>();
            }
        }

        // Formula management moved to FormulaCategoryRegistry; repository no longer stores formulas

        public IEnumerable<string> GetUnitCategories()
        {
            return _units.Keys.ToList();
        }

        public UnitDefinition? GetUnit(
            string category,
            string unitName)
        {
            var units = GetUnits(category);

     //       return units.FirstOrDefault(x =>
     //x.Name.StartsWith(
     //    unitName,
     //    StringComparison.OrdinalIgnoreCase));
     //   }

        return units
    .Where(x => x.Name.StartsWith(unitName, StringComparison.OrdinalIgnoreCase))
    .OrderByDescending(x => x.Name.Length)
    .FirstOrDefault();
            }

        public bool UpdateUnitBaseFactor(
    string category,
    string unitName,
    double toBaseFactor)
        {
            try
            {
                if (!_units.ContainsKey(category))
                {
                    return false;
                }

                var existingUnit = _units[category]
                    .FirstOrDefault(x =>
                        x.Name.Equals(
                            unitName,
                            StringComparison.OrdinalIgnoreCase));

                if (existingUnit == null)
                {
                    return false;
                }

                existingUnit.ToBaseFactor = toBaseFactor;

                return true;
            }
            catch (Exception  )
            {
                return false;
            }
        }


        public bool AddUnit(
      string category,
      UnitDefinition unit)
        {
            try
            {
                if (!_units.ContainsKey(category))
                {
                    _units[category] =
                        new List<UnitDefinition>();
                }


                var existingUnit = _units[category]
                    .FirstOrDefault(x =>
                        x.Name.Equals(
                            unit.Name,
                            StringComparison.OrdinalIgnoreCase));


                if (existingUnit != null)
                {
                    return false;
                }


                _units[category].Add(unit);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}