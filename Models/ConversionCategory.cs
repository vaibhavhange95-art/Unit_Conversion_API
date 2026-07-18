namespace Unit_Conversion_API.Models
{
    public class ConversionCategory
    {
        public string Name { get; set; } = string.Empty;

        public List<UnitDefinition> Units { get; set; } = new();
    }
}