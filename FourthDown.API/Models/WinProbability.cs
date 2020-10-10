namespace FourthDown.API.Models
{
    public class WinProbability
    {
        public int PlayId { get; set; }
        public string GameId { get; set; }
        public int Qtr { get; set; }
        public int QuarterEnd { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public double? HomeWp { get; set; }
        public double? AwayWp { get; set; }
        public int TotalHomeScore { get; set; }
        public int TotalAwayScore { get; set; }
    }
}