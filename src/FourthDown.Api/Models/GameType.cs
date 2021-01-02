using System.Runtime.Serialization;

namespace FourthDown.Api.Models
{
    public enum GameType
    {
        [EnumMember(Value = "REG")] REG, // Regular
        [EnumMember(Value = "WC")] WC, // Wild Card
        [EnumMember(Value = "DIV")] DIV, // Divisional
        [EnumMember(Value = "CON")] CON, // Conference
        [EnumMember(Value = "SB")] SB, // Super Bowl
        [EnumMember(Value = "All")] All
    }
}