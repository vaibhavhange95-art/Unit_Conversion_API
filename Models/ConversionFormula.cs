namespace Unit_Conversion_API.Models
{
    public class ConversionFormula
    {
        public string Category { get; set; } = string.Empty;

        public string FromUnit { get; set; } = string.Empty;

        public string ToUnit { get; set; } = string.Empty;

        // Formula expression using variable 'x' as input value, e.g. "x * 1000" or "(x - 32) * 5 / 9"
        public string Formula { get; set; } = string.Empty;
    }
}
