using Unit_Conversion_API.Models;

namespace Unit_Conversion_API.Repositories.Interfaces
{
    public interface IUnitRepository
    {
        IEnumerable<UnitDefinition> GetUnits(string category);

        IEnumerable<string> SearchCategories(string searchText);
        UnitDefinition? GetUnit(    string category,
            string unitName);

        bool AddUnit(
     string category,
     UnitDefinition unit);
    }
}