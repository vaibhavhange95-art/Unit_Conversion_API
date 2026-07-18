namespace Unit_Conversion_API.DTOs
{
    public class ConversionRequestDto
    {
        public string Category { get; set; } = string.Empty;

        public string FromUnit { get; set; } = string.Empty;

        public string ToUnit { get; set; } = string.Empty;

        public double Value { get; set; }
    }
}