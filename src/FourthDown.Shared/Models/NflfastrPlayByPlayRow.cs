using System.Collections.Generic;
using FourthDown.Shared.Utilities;

namespace FourthDown.Shared.Models
{
    public class NflfastrPlayByPlayRow
    {
        public NflfastrPlayByPlayRow(IReadOnlyList<string> x)
        {
            #region NflfastrPlayByPlay fields

            PlayId = StringParser.ToInt(x[0]);
            GameId = StringParser.ToString(x[1]);
            HomeTeam = StringParser.ToString(x[3]);
            AwayTeam = StringParser.ToString(x[4]);
            SeasonType = StringParser.ToString(x[5]);
            Week = StringParser.ToInt(x[6]);
            PosTeam = StringParser.ToString(x[7]);
            DefTeam = StringParser.ToString(x[9]);
            SideOfField = StringParser.ToString(x[10]);
            YardLine100 = StringParser.ToNullableInt(x[11]);
            QuarterSecondsRemaining = StringParser.ToInt(x[13]);
            HalfSecondsRemaining = StringParser.ToInt(x[14]);
            GameSecondsRemaining = StringParser.ToInt(x[15]);
            Drive = StringParser.ToNullableInt(x[18]);
            Qtr = StringParser.ToInt(x[20]);
            Down = StringParser.ToNullableInt(x[21]);
            YdsToGo = StringParser.ToInt(x[25]);
            YardsNet = StringParser.ToNullableInt(x[26]);
            PlayType = StringParser.ToString(x[28]);
            YardsGained = StringParser.ToNullableInt(x[29]);
            Shotgun = StringParser.ToInt(x[30]);
            NoHuddle = StringParser.ToInt(x[31]);
            QbDropBack = StringParser.ToNullableInt(x[32]);
            QbKneel = StringParser.ToInt(x[33]);
            QbSpike = StringParser.ToInt(x[34]);
            QbScramble = StringParser.ToInt(x[35]);
            PassLength = StringParser.ToString(x[36]);
            PassLocation = StringParser.ToString(x[37]);
            AirYards = StringParser.ToNullableInt(x[38]);
            YardsAfterCatch = StringParser.ToNullableInt(x[39]);
            TotalHomeScore = StringParser.ToInt(x[55]);
            TotalAwayScore = StringParser.ToInt(x[56]);
            ScoreDifferential = StringParser.ToNullableInt(x[59]);
            PosTeamScorePost = StringParser.ToNullableInt(x[60]);
            DefTeamScorePost = StringParser.ToNullableInt(x[61]);
            ScoreDifferentialPost = StringParser.ToNullableInt(x[62]);
            Ep = StringParser.ToNullableDouble(x[72]);
            Epa = StringParser.ToNullableDouble(x[73]);
            TotalHomeEpa = StringParser.ToDoubleDefaultZero(x[74]);
            TotalAwayEpa = StringParser.ToDoubleDefaultZero(x[75]);
            TotalHomeRushEpa = StringParser.ToDoubleDefaultZero(x[76]);
            TotalAwayRushEpa = StringParser.ToDoubleDefaultZero(x[77]);
            TotalHomePassEpa = StringParser.ToDoubleDefaultZero(x[78]);
            TotalAwayPassEpa = StringParser.ToDoubleDefaultZero(x[79]);
            AirEpa = StringParser.ToNullableDouble(x[80]);
            YacEpa = StringParser.ToNullableDouble(x[81]);
            CompAirEpa = StringParser.ToNullableDouble(x[82]);
            CompYacEpa = StringParser.ToNullableDouble(x[83]);
            TotalHomeCompAirEpa = StringParser.ToDoubleDefaultZero(x[84]);
            TotalAwayCompAirEpa = StringParser.ToDoubleDefaultZero(x[85]);
            TotalHomeCompYacEpa = StringParser.ToDoubleDefaultZero(x[86]);
            TotalAwayCompYacEpa = StringParser.ToDoubleDefaultZero(x[87]);
            TotalHomeRawAirEpa = StringParser.ToDoubleDefaultZero(x[88]);
            TotalAwayRawAirEpa = StringParser.ToDoubleDefaultZero(x[89]);
            TotalHomeRawYacEpa = StringParser.ToDoubleDefaultZero(x[90]);
            TotalAwayRawYacEpa = StringParser.ToDoubleDefaultZero(x[91]);
            HomeWinProbability = StringParser.ToDoubleDefaultZero(x[94]);
            AwayWinProbability = StringParser.ToDoubleDefaultZero(x[95]);
            TotalHomeRushWpa = StringParser.ToDoubleDefaultZero(x[103]);
            TotalAwayRushWpa = StringParser.ToDoubleDefaultZero(x[104]);
            TotalHomePassWpa = StringParser.ToDoubleDefaultZero(x[105]);
            TotalAwayPassWpa = StringParser.ToDoubleDefaultZero(x[106]);
            AirWpa = StringParser.ToNullableDouble(x[107]);
            YacWpa = StringParser.ToNullableDouble(x[108]);
            CompAirWpa = StringParser.ToNullableDouble(x[109]);
            CompYacWpa = StringParser.ToNullableDouble(x[110]);
            TotalHomeCompAirWpa = StringParser.ToDoubleDefaultZero(x[111]);
            TotalAwayCompAirWpa = StringParser.ToDoubleDefaultZero(x[112]);
            TotalHomeCompYacWpa = StringParser.ToDoubleDefaultZero(x[113]);
            TotalAwayCompYacWpa = StringParser.ToDoubleDefaultZero(x[114]);
            TotalHomeRawAirWpa = StringParser.ToDoubleDefaultZero(x[115]);
            TotalAwayRawAirWpa = StringParser.ToDoubleDefaultZero(x[116]);
            TotalHomeRawYacWpa = StringParser.ToDoubleDefaultZero(x[117]);
            TotalAwayRawYacWpa = StringParser.ToDoubleDefaultZero(x[118]);
            CompletionProbability = StringParser.ToNullableDouble(x[286]);
            CompletionProbabilityOverExpected = StringParser.ToNullableDouble(x[287]);
            SeriesResult = StringParser.ToString(x[290]);
            PlayTypeNfl = StringParser.ToString(x[299]);
            FixedDrive = StringParser.ToInt(x[304]);
            FixedDriveResult = StringParser.ToString(x[305]);
            DriveRealStartTime = StringParser.ToString(x[306]);
            DrivePlayCount = StringParser.ToNullableInt(x[307]);
            DriveTimeOfPossession = StringParser.ToString(x[308]);
            DriveFirstDowns = StringParser.ToNullableInt(x[309]);
            DriveInside20 = StringParser.ToNullableInt(x[310]);
            DriveEndedWithScore = StringParser.ToNullableInt(x[311]);
            DriveYardsPenalized = StringParser.ToNullableInt(x[314]);
            DriveStartTransition = StringParser.ToString(x[315]);
            DriveEndTransition = StringParser.ToString(x[316]);
            DriveStartYardLine = StringParser.ToString(x[319]);
            DriveEndYardLine = StringParser.ToString(x[320]);
            StartDrivePlayId = StringParser.ToNullableInt(x[321]);
            EndDrivePlayId = StringParser.ToNullableInt(x[322]);
            IsAbortedPlay = StringParser.ToBool(x[339]);
            IsSuccess = StringParser.ToNullableBool(x[340]);
            PasserName = StringParser.ToString(x[341]);
            RusherName = StringParser.ToString(x[343]);
            ReceiverName = StringParser.ToString(x[345]);
            IsPass = StringParser.ToBool(x[347]);
            IsRush = StringParser.ToBool(x[348]);
            IsFirstDown = StringParser.ToNullableBool(x[349]);
            Play = StringParser.ToBool(x[351]);
            Name = StringParser.ToString(x[355]);
            QbEpa = StringParser.ToNullableDouble(x[364]);
            ExpectedYardsAfterCatchEpa = StringParser.ToNullableDouble(x[365]);
            ExpectedYardsAfterCatchMeanYardage = StringParser.ToNullableDouble(x[366]);
            ExpectedYardsAfterCatchMedianYardage = StringParser.ToNullableInt(x[367]);
            ExpectedYardsAfterCatchSuccess = StringParser.ToNullableDouble(x[368]);
            ExpectedYardsAfterCatchFirstDown = StringParser.ToNullableDouble(x[369]);

            #endregion
        }

        public NflfastrPlayByPlayRow()
        {
        }

        #region NflfastrPlayByPlayProperties fields

        public int PlayId { get; set; }
        public string GameId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string SeasonType { get; set; }
        public int Week { get; set; }
        public string PosTeam { get; set; }
        public string DefTeam { get; set; }
        public string SideOfField { get; set; }
        public int? YardLine100 { get; set; }
        public int QuarterSecondsRemaining { get; set; }
        public int HalfSecondsRemaining { get; set; }
        public int GameSecondsRemaining { get; set; }
        public int? Drive { get; set; }
        public int Qtr { get; set; }
        public int? Down { get; set; }
        public int YdsToGo { get; set; }
        public int? YardsNet { get; set; }
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
        public int TotalHomeScore { get; set; }
        public int TotalAwayScore { get; set; }
        public int? ScoreDifferential { get; set; }
        public int? PosTeamScorePost { get; set; }
        public int? DefTeamScorePost { get; set; }
        public int? ScoreDifferentialPost { get; set; }
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
        public double HomeWinProbability { get; set; }
        public double AwayWinProbability { get; set; }
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
        public double? CompletionProbability { get; set; }
        public double? CompletionProbabilityOverExpected { get; set; }
        public string SeriesResult { get; set; }
        public string PlayTypeNfl { get; set; }
        public int FixedDrive { get; set; }
        public string FixedDriveResult { get; set; }
        public string DriveRealStartTime { get; set; }
        public int? DrivePlayCount { get; set; }
        public string DriveTimeOfPossession { get; set; }
        public int? DriveFirstDowns { get; set; }
        public int? DriveInside20 { get; set; }
        public int? DriveEndedWithScore { get; set; }
        public int? DriveYardsPenalized { get; set; }
        public string DriveStartTransition { get; set; }
        public string DriveEndTransition { get; set; }
        public string DriveStartYardLine { get; set; }
        public string DriveEndYardLine { get; set; }
        public int? StartDrivePlayId { get; set; }
        public int? EndDrivePlayId { get; set; }
        public bool IsAbortedPlay { get; set; }
        public bool? IsSuccess { get; set; }
        public string PasserName { get; set; }
        public string RusherName { get; set; }
        public string ReceiverName { get; set; }
        public bool IsPass { get; set; }
        public bool IsRush { get; set; }
        public bool? IsFirstDown { get; set; }
        public bool Play { get; set; }
        public string Name { get; set; }
        public double? QbEpa { get; set; }
        public double? ExpectedYardsAfterCatchEpa { get; set; }
        public double? ExpectedYardsAfterCatchMeanYardage { get; set; }
        public int? ExpectedYardsAfterCatchMedianYardage { get; set; }
        public double? ExpectedYardsAfterCatchSuccess { get; set; }
        public double? ExpectedYardsAfterCatchFirstDown { get; set; }

        #endregion

        /// <summary>
        /// Group by and aggregate fields.
        /// </summary>
        public (string GameId, string PosTeam, int? Down) ToPlayKey() => (GameId, PosTeam, Down);

        public bool IsEarlyDown() => Down == 1 || Down == 2;
        public bool IsLateDown() => Down == 3 || Down == 4;
    }
}