using Unit_Conversion_API.Models;

namespace Unit_Conversion_API.Repositories.Interfaces
{
    public interface IUnitRepository
    {
        IEnumerable<UnitDefinition> GetUnits(string category);
        IEnumerable<string> GetUnitCategories();

        IEnumerable<string> SearchCategories(string searchText);
        // Note: Formula management has been moved to FormulaCategoryRegistry
        bool UpdateUnitBaseFactor(
    string category,
    string unitName,
    double toBaseFactor);
        UnitDefinition? GetUnit(    string category,
            string unitName);

        bool AddUnit(
     string category,
     UnitDefinition unit);
    }
}