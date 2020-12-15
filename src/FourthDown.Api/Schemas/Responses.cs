using System;
using System.Collections.Generic;

namespace FourthDown.Api.Schemas
{
    public class Responses
    {
        
    }

    public class TeamResponse
    {
        /// <example>Dallas</example>
        public string City { get; set; }
        /// <example>Cowboys</example>
        public string Name { get; set; }
        /// <example>DAL</example>
        public string Abbreviation { get; set; }
        /// <example>NFC</example>
        public string Conference { get; set; }
        /// <example>Dallas</example>
        public string East { get; set; }
    }

    public class ScheduleResponse
    {        
        /// <example>2020_01_DAL_LA</example>
        public string GameId { get; set; }
        /// <example>2020</example>
        public int Season { get; set; }
        /// <example>REG</example>
        public string GameType { get; set; }
        /// <example>1</example>
        public int Week { get; set; }
        /// <example>2020-09-13T00:00:00</example>
        public DateTime Gameday { get; set; }
        /// <example>Sunday</example>
        public string Weekday { get; set; }
        /// <example>20:20</example>
        public string Gametime { get; set; }
        /// <example>DAL</example>
        public string AwayTeam { get; set; }
        /// <example>17</example>
        public int AwayScore { get; set; }
        /// <example>LA</example>
        public string HomeTeam { get; set; }
        /// <example>20</example>
        public int HomeScore { get; set; }
        /// <example>home</example>
        public string Location { get; set; }
        /// <example>3</example>
        public int Result { get; set; }
        /// <example>37</example>
        public int Total { get; set; }
        /// <example>false</example>
        public bool Overtime { get; set; }
        /// <example>2020091312</example>
        public string OldGameId { get; set; }
        /// <example>7</example>
        public int AwayRest { get; set; }
        /// <example>7</example>
        public int HomeRest { get; set; }
        /// <example>103</example>
        public double AwayMoneyline { get; set; }
        /// <example>-114</example>
        public double HomeMoneyline { get; set; }
        /// <example>1</example>
        public double SpreadLine { get; set; }
        /// <example>-103</example>
        public double AwaySpreadOdds { get; set; }
        /// <example>-107</example>
        public double HomeSpreadOdds { get; set; }
        /// <example>52</example>
        public double TotalLine { get; set; }
        /// <example>-111</example>
        public double UnderOdds { get; set; }
        /// <example>-100</example>
        public double OverOdds { get; set; }
        /// <example>false</example>
        public bool DivGame { get; set; }
        /// <example></example>
        public string Roof { get; set; }
        /// <example></example>
        public string Surface { get; set; }
        /// <example>0</example>
        public int? Temp { get; set; }
        /// <example>0</example>
        public int? Wind { get; set; }
        /// <example>Mike McCarthy</example>
        public string AwayCoach { get; set; }
        /// <example>Sean McVay</example>
        public string HomeCoach { get; set; }
        /// <example></example>
        public string Referee { get; set; }
        /// <example>LAX01</example>
        public string StadiumId { get; set; }
        /// <example>SoFi Stadium</example>
        public string Stadium { get; set; }
    }

    public class ValidationProblemDetailsResponse
    {
        /// <example>There are errors with your request.</example>
        public string Title { get; set; }
        /// <example>400</example>
        public int Status { get; set; }
        /// <example>
        /// <code>
        /// { "season": [ "Season must be between 1999 and 2020" ] }
        /// </code>
        /// </example>
        public Dictionary<string, string[]> Errors { get; set; }

    }
}