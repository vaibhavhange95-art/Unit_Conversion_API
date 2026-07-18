namespace Unit_Conversion_API.DTOs
{
    public class ConversionResponseDto
    {
        public double OriginalValue { get; set; }

        public string FromUnit { get; set; } = string.Empty;

        public string ToUnit { get; set; } = string.Empty;

        public double ConvertedValue { get; set; }

        public string Category { get; set; } = string.Empty;    
    }
}