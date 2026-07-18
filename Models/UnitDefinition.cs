namespace Unit_Conversion_API.Models
{
    public class UnitDefinition
    {
        public string Name { get; set; } = string.Empty;

        public double ToBaseFactor { get; set; }
    }
}