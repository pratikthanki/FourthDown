using System.Text.Json;
using System.Text.Json.Serialization;

namespace FourthDown.Shared.Models
{
    public class Team
    {
        public string City { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("abr")] 
        public string Abbreviation { get; set; }

        [JsonPropertyName("conf")] 
        public string Conference { get; set; }

        [JsonPropertyName("div")] 
        public string Division { get; set; }

        public string Label => $"{Conference} {Division}";
        public string TeamNameLabel => $"{City} {Name}";

        [JsonIgnore]
        public int DivisionIndex =>
            Division switch
            {
                "North" => 1,
                "East" => 2,
                "South" => 3,
                "West" => 4,
                _ => 0
            };

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}