using System.Text.Json;
using System.Text.Json.Serialization;

namespace FourthDown.Api.Models
{
    public class Team
    {
        public string City { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("abr")] public string Abbreviation { get; set; }

        [JsonPropertyName("conf")] public string Conference { get; set; }

        [JsonPropertyName("div")] public string Division { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}