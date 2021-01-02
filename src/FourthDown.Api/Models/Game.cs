using System;

namespace FourthDown.Api.Models
{
    public class Game
    {
        public string GameId { get; set; }
        public int Season { get; set; }
        public GameType GameType { get; set; }
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
        public string AwayCoach { get; set; }
        public string HomeCoach { get; set; }
        public string Referee { get; set; }
        public string StadiumId { get; set; }
        public string Stadium { get; set; }
    }
}