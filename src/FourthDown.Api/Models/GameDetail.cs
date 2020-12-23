using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FourthDown.Api.Models
{
    internal class GameRaw
    {
        public DataObject Data { get; set; }
    }

    internal class DataObject
    {
        public Viewer Viewer { get; set; }
    }

    internal class Viewer
    {
        public GameDetail GameDetail { get; set; }
    }

    public class HomePointsOvertime
    {
    }

    public class GameTeam
    {
        public IReadOnlyList<string> Abbreviation { get; set; }
        public IReadOnlyList<string> NickName { get; set; }
    }

    public class Period
    {
    }

    public class ScoringSummary
    {
        public int PlayId { get; set; }
        public string PlayDescription { get; set; }
        public int PatPlayId { get; set; }
        public int HomeScore { get; set; }
        public int VisitorScore { get; set; }
    }

    public class VisitorPointsOvertime
    {
    }

    public class CurrentFahrenheit
    {
    }

    public class Location
    {
    }

    public class LongDescription
    {
    }

    public class CurrentRealFeelFahrenheit
    {
    }

    public class Weather
    {
        public CurrentFahrenheit CurrentFahrenheit { get; set; }
        public Location Location { get; set; }
        public LongDescription LongDescription { get; set; }
        public IList<string> ShortDescription { get; set; }
        public CurrentRealFeelFahrenheit CurrentRealFeelFahrenheit { get; set; }
    }

    public class YardLine
    {
    }

    public class Drive
    {
        public int QuarterStart { get; set; }
        public string EndTransition { get; set; }
        public string EndYardLine { get; set; }
        public bool EndedWithScore { get; set; }
        public int FirstDowns { get; set; }
        public string GameClockEnd { get; set; }
        public string GameClockStart { get; set; }
        public string HowEndedDescription { get; set; }
        public string HowStartedDescription { get; set; }
        public bool Inside20 { get; set; }
        public double OrderSequence { get; set; }
        public int PlayCount { get; set; }
        public int PlayIdEnded { get; set; }
        public int PlayIdStarted { get; set; }
        public int PlaySeqEnded { get; set; }
        public int PlaySeqStarted { get; set; }
        public int QuarterEnd { get; set; }
        public string StartTransition { get; set; }
        public string StartYardLine { get; set; }
        public string TimeOfPossession { get; set; }
        public int Yards { get; set; }
        public int YardsPenalized { get; set; }
        public string PossessionTeamAbbreviation { get; set; }
        public string PossessionTeamNickName { get; set; }
        public string PossessionTeamFranchiseCurrentLogoUrl { get; set; }
    }

    public class PlayStat
    {
        public int StatId { get; set; }
        public int Yards { get; set; }
        public string PlayerName { get; set; }
        public string TeamId { get; set; }
        public string TeamAbbreviation { get; set; }
        public string GsisPlayerId { get; set; }
    }

    public class Play
    {
        public string ClockTime { get; set; }
        public int Down { get; set; }
        public string EndClockTime { get; set; }
        public string EndYardLine { get; set; }
        public bool FirstDown { get; set; }
        public bool GoalToGo { get; set; }
        public bool NextPlayIsGoalToGo { get; set; }
        public string NextPlayType { get; set; }
        public double OrderSequence { get; set; }
        public bool PenaltyOnPlay { get; set; }
        public string PlayClock { get; set; }
        public bool PlayDeleted { get; set; }
        public string PlayDescription { get; set; }
        public string PlayDescriptionWithJerseyNumbers { get; set; }
        public int PlayId { get; set; }
        public string PlayType { get; set; }
        public IList<PlayStat> PlayStats { get; set; }
        public string PrePlayByPlay { get; set; }
        public int Quarter { get; set; }
        public bool ScoringPlay { get; set; }
        public string ShortDescription { get; set; }
        public bool SpecialTeamsPlay { get; set; }
        public string TimeOfDay { get; set; }
        public string YardLine { get; set; }
        public int Yards { get; set; }
        public int YardsToGo { get; set; }
        public int? DriveNetYards { get; set; }
        public int? DrivePlayCount { get; set; }
        public int? DriveSequenceNumber { get; set; }
        public string DriveTimeOfPossession { get; set; }
        public string PossessionTeamAbbreviation { get; set; }
        public string PossessionTeamNickName { get; set; }
        public string PossessionTeamFranchiseCurrentLogoUrl { get; set; }
        public bool? IsBigPlay { get; set; }
        public string ScoringPlayType { get; set; }
        public string ScoringTeamId { get; set; }
        public string ScoringTeamAbbreviation { get; set; }
        public string ScoringTeamNickName { get; set; }
        public string StPlayType { get; set; }
    }

    public class GameDetail
    {
        [JsonIgnore] public IList<string> Id { get; set; }
        [JsonIgnore] public IList<string> Attendance { get; set; }
        [JsonIgnore] public IList<int> Distance { get; set; }
        [JsonIgnore] public IList<int> Down { get; set; }
        [JsonIgnore] public IList<string> GameClock { get; set; }
        [JsonIgnore] public IList<bool> GoalToGo { get; set; }
        [JsonIgnore] public HomePointsOvertime HomePointsOvertime { get; set; }
        [JsonIgnore] public Period Period { get; set; }
        [JsonIgnore] public IList<string> Phase { get; set; }
        [JsonIgnore] public IList<bool> PlayReview { get; set; }
        [JsonIgnore] public GameTeam PossessionTeam { get; set; }
        [JsonIgnore] public IList<bool> Redzone { get; set; }
        [JsonIgnore] public IList<string> Stadium { get; set; }
        [JsonIgnore] public IReadOnlyList<string> StartTime { get; set; }
        [JsonIgnore] public VisitorPointsOvertime VisitorPointsOvertime { get; set; }
        [JsonIgnore] public Weather Weather { get; set; }
        [JsonIgnore] public YardLine YardLine { get; set; }
        [JsonIgnore] public IList<int> YardsToGo { get; set; }

        public Game Game { get; set; }
        public IReadOnlyList<int> HomePointsQ1 { get; set; }
        public IReadOnlyList<int> HomePointsQ2 { get; set; }
        public IReadOnlyList<int> HomePointsQ3 { get; set; }
        public IReadOnlyList<int> HomePointsQ4 { get; set; }
        public IReadOnlyList<int> HomePointsOvertimeTotal { get; set; }
        public IReadOnlyList<int> HomePointsTotal { get; set; }
        [JsonPropertyName("HomeTeam")] public GameTeam HomeGameTeam { get; set; }
        public IReadOnlyList<int> HomeTimeoutsUsed { get; set; }
        public IReadOnlyList<int> HomeTimeoutsRemaining { get; set; }
        public IReadOnlyList<int> VisitorPointsQ1 { get; set; }
        public IReadOnlyList<int> VisitorPointsQ2 { get; set; }
        public IReadOnlyList<int> VisitorPointsQ3 { get; set; }
        public IReadOnlyList<int> VisitorPointsQ4 { get; set; }
        public IReadOnlyList<int> VisitorPointsOvertimeTotal { get; set; }
        public IReadOnlyList<int> VisitorPointsTotal { get; set; }
        [JsonPropertyName("VisitorTeam")] public GameTeam VisitorGameTeam { get; set; }
        public IReadOnlyList<int> VisitorTimeoutsUsed { get; set; }
        public IReadOnlyList<int> VisitorTimeoutsRemaining { get; set; }
        public IList<Drive> Drives { get; set; }
        public IList<Play> Plays { get; set; }
        public IList<ScoringSummary> ScoringSummaries { get; set; }
    }
}