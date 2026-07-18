namespace Unit_Conversion_API.Models
{
    public class UpdateUnitRequest
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public double ToBaseFactor { get; set; }
    }
}
