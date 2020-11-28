using System;

namespace FourthDown.Api.Models
{
    public class PlayByPlayFormatted
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

        // TODO: pivot event types into an enum "PlayType"
        public bool? PuntBlocked { get; set; }
        public bool? FirstDownRush { get; set; }
        public bool? FirstDownPass { get; set; }
        public bool? FirstDownPenalty { get; set; }
        public bool? ThirdDownConverted { get; set; }
        public bool? ThirdDownFailed { get; set; }
        public bool? FourthDownConverted { get; set; }
        public bool? FourthDownFailed { get; set; }
        public bool? IncompletePass { get; set; }
        public bool? TouchBack { get; set; }
        public bool? Interception { get; set; }
        public bool? PuntInsideTwenty { get; set; }
        public bool? PuntInEndZone { get; set; }
        public bool? PuntOutOfBounds { get; set; }
        public bool? PuntDowned { get; set; }
        public bool? PuntFairCatch { get; set; }
        public bool? KickoffInsideTwenty { get; set; }
        public bool? KickoffInEndZone { get; set; }
        public bool? KickoffOutOfBounds { get; set; }
        public bool? KickoffDowned { get; set; }
        public bool? KickoffFairCatch { get; set; }
        public bool? FumbleForced { get; set; }
        public bool? FumbleNotForced { get; set; }
        public bool? FumbleOutOfBounds { get; set; }
        public bool? SoloTackle { get; set; }
        public bool? Safety { get; set; }
        public bool? Penalty { get; set; }
        public bool? TackledForLoss { get; set; }
        public bool? FumbleLost { get; set; }
        public bool? OwnKickoffRecovery { get; set; }
        public bool? OwnKickoffRecoveryTd { get; set; }
        public bool? QbHit { get; set; }
        public bool? RushAttempt { get; set; }
        public bool? PassAttempt { get; set; }
        public bool? Sack { get; set; }
        public bool? Touchdown { get; set; }
        public bool? PassTouchdown { get; set; }
        public bool? RushTouchdown { get; set; }
        public bool? ReturnTouchdown { get; set; }
        public bool? ExtraPointAttempt { get; set; }
        public bool? TwoPointAttempt { get; set; }
        public bool? FieldGoalAttempt { get; set; }
        public bool? KickoffAttempt { get; set; }
        public bool? PuntAttempt { get; set; }
        public bool? Fumble { get; set; }
        public bool? CompletePass { get; set; }
        public bool? AssistTackle { get; set; }
        public bool? LateralReception { get; set; }
        public bool? LateralRush { get; set; }
        public bool? LateralReturn { get; set; }
        public bool? LateralRecovery { get; set; }
        public string PasserPlayerId { get; set; }
        public string PasserPlayerName { get; set; }
        public string ReceiverPlayerId { get; set; }
        public string ReceiverPlayerName { get; set; }
        public string RusherPlayerId { get; set; }
        public string RusherPlayerName { get; set; }
        public string ReturnTeam { get; set; }
        public int? ReturnYards { get; set; }
        public string PenaltyTeam { get; set; }
        public string PenaltyPlayerId { get; set; }
        public string PenaltyPlayerName { get; set; }
        public int? PenaltyYards { get; set; }
        public int ReplayOrChallenge { get; set; }
        public string ReplayOrChallengeResult { get; set; }
        public string PenaltyType { get; set; }
        public int? DefensiveTwoPointAttempt { get; set; }
        public int? DefensiveTwoPointConv { get; set; }
        public int? DefensiveExtraPointAttempt { get; set; }
        public int? DefensiveExtraPointConv { get; set; }
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