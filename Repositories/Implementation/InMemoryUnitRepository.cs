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
                            Name = "Centimeter",
                            ToBaseFactor = 0.01
                        },
                        new UnitDefinition
                        {
                            Name = "Millimeter",
                            ToBaseFactor = 0.001
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
                        }
                    }
                }
            };
        }


        public IEnumerable<UnitDefinition> GetUnits(
            string category)
        {
            if (_units.ContainsKey(category))
            {
                return _units[category];
            }

            return Enumerable.Empty<UnitDefinition>();
        }


        public UnitDefinition? GetUnit(
            string category,
            string unitName)
        {
            var units = GetUnits(category);

            return units.FirstOrDefault(x =>
                x.Name.Equals(
                    unitName,
                    StringComparison.OrdinalIgnoreCase));
        }


        public void AddUnit(
            string category,
            UnitDefinition unit)
        {
            if (!_units.ContainsKey(category))
            {
                _units[category] =
                    new List<UnitDefinition>();
            }

            _units[category].Add(unit);
        }
    }
}