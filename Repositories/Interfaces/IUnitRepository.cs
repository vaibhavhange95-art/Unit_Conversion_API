using Unit_Conversion_API.Models;

namespace Unit_Conversion_API.Repositories.Interfaces
{
    public interface IUnitRepository
    {
        IEnumerable<UnitDefinition> GetUnits(string category);
        IEnumerable<string> GetUnitCategories();

        IEnumerable<string> SearchCategories(string searchText);
        // New API to manage formulas
        bool TryGetFormula(string category, string fromUnit, string toUnit, out Models.ConversionFormula? formula);
        bool AddFormula(Models.ConversionFormula formula);
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