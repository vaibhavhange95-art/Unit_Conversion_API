namespace Unit_Conversion_API.DTOs
{
    public class ConversionResponseDto
    {
        public double OriginalValue { get; set; }

        public string FromUnit { get; set; } = string.Empty;

        public string ToUnit { get; set; } = string.Empty;

        public double ConvertedValue { get; set; }

        public string Category { get; set; } = string.Empty;    
        // Indicates whether conversion succeeded. False when there was an error.
        public bool IsSuccess { get; set; } = true;

        // If IsSuccess is false, this contains the error message to show in the UI.
        public string Message { get; set; } = string.Empty;
    }
}