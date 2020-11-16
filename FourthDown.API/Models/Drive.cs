namespace FourthDown.Api.Models
{
    public class Drive
    {
        public string GameId { get; set; }
        public int PlayId { get; set; }
        public string PosTeam { get; set; }
        public int? Down { get; set; }
        public int YdsToGo { get; set; }
        public string Desc { get; set; }
        public double? Ep { get; set; }
        public double? Epa { get; set; }
        public double? VegasWp { get; set; }
    }
}