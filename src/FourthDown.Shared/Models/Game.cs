using System;
using FourthDown.Shared.Utilities;

namespace FourthDown.Shared.Models
{
    public class Game : IEquatable<Game>
    {
        public string GameId { get; set; }
        public int Season { get; set; }
        public string GameType { get; set; }
        public int Week { get; set; }
        public DateTime Gameday { get; set; }
        public string Weekday { get; set; }
        public string Gametime { get; set; }
        public string AwayTeam { get; set; }
        public int AwayScore { get; set; }
        public string HomeTeam { get; set; }
        public int HomeScore { get; set; }
        public string Location { get; set; }
        public int Result { get; set; }
        public int Total { get; set; }
        public bool Overtime { get; set; }
        public string OldGameId { get; set; }
        public string Gsis { get; set; }
        public string NflDetailId { get; set; }
        public string Pfr { get; set; }
        public string Pff { get; set; }
        public int Espn { get; set; }
        public string Ftn { get; set; }
        public int AwayRest { get; set; }
        public int HomeRest { get; set; }
        public double AwayMoneyline { get; set; }
        public double HomeMoneyline { get; set; }
        public double SpreadLine { get; set; }
        public double AwaySpreadOdds { get; set; }
        public double HomeSpreadOdds { get; set; }
        public double TotalLine { get; set; }
        public double UnderOdds { get; set; }
        public double OverOdds { get; set; }
        public bool DivGame { get; set; }
        public string Roof { get; set; }
        public string Surface { get; set; }
        public int? Temp { get; set; }
        public int? Wind { get; set; }
        public string AwayQbId { get; set; }
        public string HomeQbId { get; set; }
        public string AwaQbName { get; set; }
        public string HomeQbName { get; set; }
        public string AwayCoach { get; set; }
        public string HomeCoach { get; set; }
        public string Referee { get; set; }
        public string StadiumId { get; set; }
        public string Stadium { get; set; }

        public DateTime GameTimeUtc()
        {
            return StringParser.EstDateTimeToUtc(
                string.Concat(Gameday.ToShortDateString(), string.Empty, Gametime));
        }

        public bool Equals(Game other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GameId == other.GameId && Season == other.Season && GameType == other.GameType && Week == other.Week;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Game) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(GameId);
            hashCode.Add(Season);
            hashCode.Add(GameType);
            hashCode.Add(Week);
            return hashCode.ToHashCode();
        }
    }
}