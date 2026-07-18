using System.Text.Json.Serialization;

namespace Unit_Conversion_API.DTOs
{
    public class AddUnitRequestDto
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("toBaseFactor")]
        public double ToBaseFactor { get; set; }
    }
}