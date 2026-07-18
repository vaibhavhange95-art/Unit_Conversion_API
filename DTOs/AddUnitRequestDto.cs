namespace Unit_Conversion_API.DTOs
{
    public class AddUnitRequestDto
    {
        public string Category { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public double ToBaseFactor { get; set; }
    }
}