using Unit_Conversion_API.Models;

namespace Unit_Conversion_API.Services.UnitResolver.Interfaces
{
    public interface IUnitResolverService
    {
        UnitDefinition? ResolveUnit(
            string category,
            string unitName);
    }
}