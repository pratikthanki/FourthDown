using System.Text.Json.Serialization;

namespace FourthDown.Shared.Models
{
    public class CombineWorkout
    {
        [JsonIgnore]
        [JsonPropertyName("Id")] 
        public int Id { get; set; }

        [JsonIgnore]
        [JsonPropertyName("ShieldId")] 
        public string ShieldId { get; set; }

        [JsonPropertyName("FirstName")] 
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")] 
        public string LastName { get; set; }

        [JsonPropertyName("College")] 
        public string College { get; set; }

        [JsonPropertyName("Position")] 
        public string Position { get; set; }

        [JsonPropertyName("Season")] 
        public int Season { get; set; }

        [JsonPropertyName("FORTY_YARD_DASH")] 
        public double? FortyYardDash { get; set; }

        [JsonPropertyName("BENCH_PRESS")] 
        public double? BenchPress { get; set; }

        [JsonPropertyName("VERTICAL_JUMP")] 
        public double? VerticalJump { get; set; }

        [JsonPropertyName("BROAD_JUMP")] 
        public double? BroadJump { get; set; }

        [JsonPropertyName("THREE_CONE_DRILL")] 
        public double? ThreeConeDrill { get; set; }

        [JsonPropertyName("TWENTY_YARD_SHUTTLE")] 
        public double? TwentyYardShuttle { get; set; }

        [JsonPropertyName("SIXTY_YARD_SHUTTLE")] 
        public double? SixtyYardShuttle { get; set; }
    }
}