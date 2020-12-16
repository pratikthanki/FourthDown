using System;
using System.Collections.Generic;
using FourthDown.Api.Models;

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

    public class GameDrivesResponse
    {
        /// <example>10160000-0581-8196-873c-fbd4b6a4688f</example>
        public string Id { get; set; }
        /// <example>78165</example>
        public string Attendance { get; set; }
        /// <example>AT&T Stadium</example>
        public string Stadium { get; set; }
        /// <example>20:20:00</example>
        public string StartTime { get; set; }
        /// <example>Controlled Climate. Temp: 68° F, Humidity: 70%, Wind: 4mph</example>
        public string WeatherShortDescription { get; set; }
        
        public ApiGamePlay.TeamStats HomeTeam { get; set; }
        public ApiGamePlay.TeamStats VisitorTeam { get; set; }
        public IList<Drive> Drives { get; set; }
    }

    public class GamePlaysResponse
    {
        /// <example>10160000-0581-8196-873c-fbd4b6a4688f</example>
        public string Id { get; set; }
        /// <example>78165</example>
        public string Attendance { get; set; }
        /// <example>AT&T Stadium</example>
        public string Stadium { get; set; }
        /// <example>20:20:00</example>
        public string StartTime { get; set; }
        /// <example>Controlled Climate. Temp: 68° F, Humidity: 70%, Wind: 4mph</example>
        public string WeatherShortDescription { get; set; }

        public ApiGamePlay.TeamStats HomeTeam { get; set; }
        public ApiGamePlay.TeamStats VisitorTeam { get; set; }
        public IList<Play> Plays { get; set; }
    }

    public class GameScoringSummariesResponse
    {
        /// <example>10160000-0581-8196-873c-fbd4b6a4688f</example>
        public string Id { get; set; }
        /// <example>78165</example>
        public string Attendance { get; set; }
        /// <example>AT&T Stadium</example>
        public string Stadium { get; set; }
        /// <example>20:20:00</example>
        public string StartTime { get; set; }
        /// <example>Controlled Climate. Temp: 68° F, Humidity: 70%, Wind: 4mph</example>
        public string WeatherShortDescription { get; set; }

        public ApiGamePlay.TeamStats HomeTeam { get; set; }
        public ApiGamePlay.TeamStats VisitorTeam { get; set; }
        public IList<ScoringSummary> ScoringSummaries { get; set; }
    }

    public class PlayByPlayResponse
    {
        public int PlayId { get; set; }
        public string GameId { get; set; }
        public int OldGameId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string SeasonType { get; set; }
        public int Week { get; set; }
        public string PosTeam { get; set; }
        public string PosTeamType { get; set; }
        public string DefTeam { get; set; }
        public string SideOfField { get; set; }
        public int? YardLine100 { get; set; }
        public DateTime? GameDate { get; set; }
        public int QuarterSecondsRemaining { get; set; }
        public int HalfSecondsRemaining { get; set; }
        public int GameSecondsRemaining { get; set; }
        public string GameHalf { get; set; }
        public int QuarterEnd { get; set; }
        public int? Drive { get; set; }
        public int Sp { get; set; }
        public int Qtr { get; set; }
        public int? Down { get; set; }
        public int GoalToGo { get; set; }
        public string Time { get; set; }
        public string YardLine { get; set; }
        public int YdsToGo { get; set; }
        public int? YardsNet { get; set; }
        public string Desc { get; set; }
        public string PlayType { get; set; }
        public int? YardsGained { get; set; }
        public int Shotgun { get; set; }
        public int NoHuddle { get; set; }
        public int? QbDropBack { get; set; }
        public int QbKneel { get; set; }
        public int QbSpike { get; set; }
        public int QbScramble { get; set; }
        public string PassLength { get; set; }
        public string PassLocation { get; set; }
        public int? AirYards { get; set; }
        public int? YardsAfterCatch { get; set; }
        public string RunLocation { get; set; }
        public string RunGap { get; set; }
        public string FieldGoalResult { get; set; }
        public int? KickDistance { get; set; }
        public string ExtraPointResult { get; set; }
        public string TwoPointConvResult { get; set; }
        public int HomeTimeoutsRemaining { get; set; }
        public int AwayTimeoutsRemaining { get; set; }
        public int? Timeout { get; set; }
        public string TimeoutTeam { get; set; }
        public string TdTeam { get; set; }
        public int? PosTeamTimeoutsRemaining { get; set; }
        public int? DefTeamTimeoutsRemaining { get; set; }
        public int TotalHomeScore { get; set; }
        public int TotalAwayScore { get; set; }
        public int? PosTeamScore { get; set; }
        public int? DefTeamScore { get; set; }
        public int? ScoreDifferential { get; set; }
        public int? PosTeamScorePost { get; set; }
        public int? DefTeamScorePost { get; set; }
        public int? ScoreDifferentialPost { get; set; }
        public double NoScoreProb { get; set; }
        public double OppFgProb { get; set; }
        public double OppSafetyProb { get; set; }
        public double OppTdProb { get; set; }
        public double FgProb { get; set; }
        public double SafetyProb { get; set; }
        public double TdProb { get; set; }
        public double ExtraPointProb { get; set; }
        public double TwoPointConversionProb { get; set; }
        public double? Ep { get; set; }
        public double? Epa { get; set; }
        public double TotalHomeEpa { get; set; }
        public double TotalAwayEpa { get; set; }
        public double TotalHomeRushEpa { get; set; }
        public double TotalAwayRushEpa { get; set; }
        public double TotalHomePassEpa { get; set; }
        public double TotalAwayPassEpa { get; set; }
        public double? AirEpa { get; set; }
        public double? YacEpa { get; set; }
        public double? CompAirEpa { get; set; }
        public double? CompYacEpa { get; set; }
        public double TotalHomeCompAirEpa { get; set; }
        public double TotalAwayCompAirEpa { get; set; }
        public double TotalHomeCompYacEpa { get; set; }
        public double TotalAwayCompYacEpa { get; set; }
        public double TotalHomeRawAirEpa { get; set; }
        public double TotalAwayRawAirEpa { get; set; }
        public double TotalHomeRawYacEpa { get; set; }
        public double TotalAwayRawYacEpa { get; set; }
        public double? Wp { get; set; }
        public double? DefWp { get; set; }
        public double? HomeWp { get; set; }
        public double? AwayWp { get; set; }
        public double? Wpa { get; set; }
        public double? HomeWpPost { get; set; }
        public double? AwayWpPost { get; set; }
        public double? VegasWp { get; set; }
        public double? VegasHomeWp { get; set; }
        public double TotalHomeRushWpa { get; set; }
        public double TotalAwayRushWpa { get; set; }
        public double TotalHomePassWpa { get; set; }
        public double TotalAwayPassWpa { get; set; }
        public double? AirWpa { get; set; }
        public double? YacWpa { get; set; }
        public double? CompAirWpa { get; set; }
        public double? CompYacWpa { get; set; }
        public double TotalHomeCompAirWpa { get; set; }
        public double TotalAwayCompAirWpa { get; set; }
        public double TotalHomeCompYacWpa { get; set; }
        public double TotalAwayCompYacWpa { get; set; }
        public double TotalHomeRawAirWpa { get; set; }
        public double TotalAwayRawAirWpa { get; set; }
        public double TotalHomeRawYacWpa { get; set; }
        public double TotalAwayRawYacWpa { get; set; }
        public int Season { get; set; }
        public double? Cp { get; set; }
        public double? Cpoe { get; set; }
        public int Series { get; set; }
        public int SeriesSuccess { get; set; }
        public string SeriesResult { get; set; }
        public double OrderSequence { get; set; }
        public string StartTime { get; set; }
        public string TimeOfDay { get; set; }
        public string Stadium { get; set; }
        public string Weather { get; set; }
        public string NflApiId { get; set; }
        public int PlayClock { get; set; }
        public bool PlayDeleted { get; set; }
        public string PlayTypeNfl { get; set; }
        public bool SpecialTeamsPlay { get; set; }
        public string SpecialTeamsPlayType { get; set; }
        public string EndClockTime { get; set; }
        public string EndYardLine { get; set; }
        public int FixedDrive { get; set; }
        public string FixedDriveResult { get; set; }
        public string DriveRealStartTime { get; set; }
        public int? DrivePlayCount { get; set; }
        public string DriveTimeOfPossession { get; set; }
        public int? DriveFirstDowns { get; set; }
        public int? DriveInside20 { get; set; }
        public int? DriveEndedWithScore { get; set; }
        public int? DriveQuarterStart { get; set; }
        public int? DriveQuarterEnd { get; set; }
        public int? DriveYardsPenalized { get; set; }
        public string DriveStartTransition { get; set; }
        public string DriveEndTransition { get; set; }
        public string DriveGameClockStart { get; set; }
        public string DriveGameClockEnd { get; set; }
        public string DriveStartYardLine { get; set; }
        public string DriveEndYardLine { get; set; }
        public int? DrivePlayIdStarted { get; set; }
        public int? DrivePlayIdEnded { get; set; }
        public int AwayScore { get; set; }
        public int HomeScore { get; set; }
        public string Location { get; set; }
        public int Result { get; set; }
        public int Total { get; set; }
        public double SpreadLine { get; set; }
        public double TotalLine { get; set; }
        public int DivGame { get; set; }
        public string Roof { get; set; }
        public string Surface { get; set; }
        public string Temp { get; set; }
        public string Wind { get; set; }
        public string HomeCoach { get; set; }
        public string AwayCoach { get; set; }
        public string StadiumId { get; set; }
        public string GameStadium { get; set; }
        public bool? Success { get; set; }
        public string Passer { get; set; }
        public int? PasserJerseyNumber { get; set; }
        public string Rusher { get; set; }
        public int? RusherJerseyNumber { get; set; }
        public string Receiver { get; set; }
        public int? ReceiverJerseyNumber { get; set; }
        public bool Pass { get; set; }
        public bool Rush { get; set; }
        public bool? FirstDown { get; set; }
        public bool AbortedPlay { get; set; }
        public bool Special { get; set; }
        public bool Play { get; set; }
        public string PasserId { get; set; }
        public string RusherId { get; set; }
        public string ReceiverId { get; set; }
        public string Name { get; set; }
        public int? JerseyNumber { get; set; }
        public string Id { get; set; }
        public double? QbEpa { get; set; }
        public double? XyacEpa { get; set; }
        public double? XyacMeanYardage { get; set; }
        public int? XyacMedianYardage { get; set; }
        public double? XyacSuccess { get; set; }
        public double? XyacFd { get; set; }
    }
}