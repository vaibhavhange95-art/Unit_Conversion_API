using Unit_Conversion_API.Models;

namespace Unit_Conversion_API.Repositories.Interfaces
{
    public interface IUnitRepository
    {
        IEnumerable<UnitDefinition> GetUnits(string category);

        UnitDefinition? GetUnit(
            string category,
            string unitName);

        void AddUnit(
            string category,
            UnitDefinition unit);
    }
}