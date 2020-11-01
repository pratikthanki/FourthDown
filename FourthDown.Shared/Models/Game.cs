using System;

namespace FourthDown.Shared.Models
{
    public class Game
    {
        public Game(string row)
        {
            var fields = row.Split(",");
            GameId = fields[0];
            Season = int.Parse(fields[1]);
            GameType = fields[2];
            Week = int.Parse(fields[3]);
            Gameday = DateTime.ParseExact(fields[4], "yyyy-MM-dd", null);
            Weekday = fields[5];
            Gametime = fields[6];
            AwayTeam = fields[7];
            AwayScore = int.Parse(fields[8]);
            HomeTeam = fields[9];
            HomeScore = int.Parse(fields[10]);
            Location = fields[11];
            Result = int.Parse(fields[12]);
            Total = int.Parse(fields[13]);
            Overtime = int.Parse(fields[14]);
            OldGameId = fields[15];
            AwayRest = int.Parse(fields[16]);
            HomeRest = int.Parse(fields[17]);
            AwayMoneyline = fields[18];
            HomeMoneyline = fields[19];
            SpreadLine = double.Parse(fields[20]);
            AwaySpreadOdds = fields[21];
            HomeSpreadOdds = fields[22];
            TotalLine = double.Parse(fields[23]);
            UnderOdds = fields[24];
            OverOdds = fields[25];
            DivGame = int.Parse(fields[26]);
            Roof = fields[27];
            Surface = fields[28];
            Temp = fields[29] == "" ? (int?) null : int.Parse(fields[29]);
            Wind = fields[30] == "" ? (int?) null : int.Parse(fields[30]);
            AwayCoach = fields[31];
            HomeCoach = fields[32];
            Referee = fields[33];
            StadiumId = fields[34];
            Stadium = fields[35];
        }

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
        public int Overtime { get; set; }
        public string OldGameId { get; set; }
        public int AwayRest { get; set; }
        public int HomeRest { get; set; }
        public string AwayMoneyline { get; set; }
        public string HomeMoneyline { get; set; }
        public double SpreadLine { get; set; }
        public string AwaySpreadOdds { get; set; }
        public string HomeSpreadOdds { get; set; }
        public double TotalLine { get; set; }
        public string UnderOdds { get; set; }
        public string OverOdds { get; set; }
        public int DivGame { get; set; }
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