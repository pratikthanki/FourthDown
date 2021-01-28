using FourthDown.Shared.Utilities;

namespace FourthDown.Shared.Models
{
    public class NflfastrPlayByPlay
    {
        public NflfastrPlayByPlay(string[] x)
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
            YardLine100 = StringParser.ToNullableInt(x[11]);
            QuarterSecondsRemaining = StringParser.ToInt(x[13]);
            HalfSecondsRemaining = StringParser.ToInt(x[14]);
            GameSecondsRemaining = StringParser.ToInt(x[15]);
            Drive = StringParser.ToNullableInt(x[18]);
            Qtr = StringParser.ToInt(x[20]);
            Down = StringParser.ToNullableInt(x[21]);
            YdsToGo = StringParser.ToInt(x[25]);
            YardsNet = StringParser.ToNullableInt(x[26]);
            Desc = StringParser.ToString(x[27]);
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
            TotalHomeScore = StringParser.ToInt(x[53]);
            TotalAwayScore = StringParser.ToInt(x[54]);
            ScoreDifferential = StringParser.ToNullableInt(x[57]);
            PosTeamScorePost = StringParser.ToNullableInt(x[58]);
            DefTeamScorePost = StringParser.ToNullableInt(x[59]);
            ScoreDifferentialPost = StringParser.ToNullableInt(x[60]);
            Ep = StringParser.ToNullableDouble(x[70]);
            Epa = StringParser.ToNullableDouble(x[71]);
            TotalHomeEpa = StringParser.ToDouble(x[72]);
            TotalAwayEpa = StringParser.ToDouble(x[73]);
            TotalHomeRushEpa = StringParser.ToDouble(x[74]);
            TotalAwayRushEpa = StringParser.ToDouble(x[75]);
            TotalHomePassEpa = StringParser.ToDouble(x[76]);
            TotalAwayPassEpa = StringParser.ToDouble(x[77]);
            AirEpa = StringParser.ToNullableDouble(x[78]);
            YacEpa = StringParser.ToNullableDouble(x[79]);
            CompAirEpa = StringParser.ToNullableDouble(x[80]);
            CompYacEpa = StringParser.ToNullableDouble(x[81]);
            TotalHomeCompAirEpa = StringParser.ToDouble(x[82]);
            TotalAwayCompAirEpa = StringParser.ToDouble(x[83]);
            TotalHomeCompYacEpa = StringParser.ToDouble(x[84]);
            TotalAwayCompYacEpa = StringParser.ToDouble(x[85]);
            TotalHomeRawAirEpa = StringParser.ToDouble(x[86]);
            TotalAwayRawAirEpa = StringParser.ToDouble(x[87]);
            TotalHomeRawYacEpa = StringParser.ToDouble(x[88]);
            TotalAwayRawYacEpa = StringParser.ToDouble(x[89]);
            TotalHomeRushWpa = StringParser.ToDouble(x[99]);
            TotalAwayRushWpa = StringParser.ToDouble(x[100]);
            TotalHomePassWpa = StringParser.ToDouble(x[101]);
            TotalAwayPassWpa = StringParser.ToDouble(x[102]);
            AirWpa = StringParser.ToNullableDouble(x[103]);
            YacWpa = StringParser.ToNullableDouble(x[104]);
            CompAirWpa = StringParser.ToNullableDouble(x[105]);
            CompYacWpa = StringParser.ToNullableDouble(x[106]);
            TotalHomeCompAirWpa = StringParser.ToDouble(x[107]);
            TotalAwayCompAirWpa = StringParser.ToDouble(x[108]);
            TotalHomeCompYacWpa = StringParser.ToDouble(x[109]);
            TotalAwayCompYacWpa = StringParser.ToDouble(x[110]);
            TotalHomeRawAirWpa = StringParser.ToDouble(x[111]);
            TotalAwayRawAirWpa = StringParser.ToDouble(x[112]);
            TotalHomeRawYacWpa = StringParser.ToDouble(x[113]);
            TotalAwayRawYacWpa = StringParser.ToDouble(x[114]);
            CompletionProbability = StringParser.ToNullableDouble(x[262]);
            CompletionProbabilityOverExpected = StringParser.ToNullableDouble(x[263]);
            SeriesResult = StringParser.ToString(x[266]);
            PlayTypeNfl = StringParser.ToString(x[275]);
            FixedDrive = StringParser.ToInt(x[280]);
            FixedDriveResult = StringParser.ToString(x[281]);
            DriveRealStartTime = StringParser.ToString(x[282]);
            DrivePlayCount = StringParser.ToNullableInt(x[283]);
            DriveTimeOfPossession = StringParser.ToString(x[284]);
            DriveFirstDowns = StringParser.ToNullableInt(x[285]);
            DriveInside20 = StringParser.ToNullableInt(x[286]);
            DriveEndedWithScore = StringParser.ToNullableInt(x[287]);
            DriveYardsPenalized = StringParser.ToNullableInt(x[290]);
            DriveStartTransition = StringParser.ToString(x[291]);
            DriveEndTransition = StringParser.ToString(x[292]);
            DriveStartYardLine = StringParser.ToString(x[295]);
            DriveEndYardLine = StringParser.ToString(x[296]);
            StartDrivePlayId = StringParser.ToNullableInt(x[297]);
            EndDrivePlayId = StringParser.ToNullableInt(x[298]);
            IsSuccess = StringParser.ToNullableBool(x[315]);
            PasserName = StringParser.ToString(x[316]);
            RusherName = StringParser.ToString(x[318]);
            ReceiverName = StringParser.ToString(x[320]);
            IsPass = StringParser.ToBool(x[322]);
            IsRush = StringParser.ToBool(x[323]);
            IsFirstDown = StringParser.ToNullableBool(x[324]);
            IsAbortedPlay = StringParser.ToBool(x[325]);
            Play = StringParser.ToBool(x[327]);
            Name = StringParser.ToString(x[331]);
            QbEpa = StringParser.ToNullableDouble(x[334]);
            ExpectedYardsAfterCatchEpa = StringParser.ToNullableDouble(x[335]);
            ExpectedYardsAfterCatchMeanYardage = StringParser.ToNullableDouble(x[336]);
            ExpectedYardsAfterCatchMedianYardage = StringParser.ToNullableInt(x[337]);
            ExpectedYardsAfterCatchSuccess = StringParser.ToNullableDouble(x[338]);
            ExpectedYardsAfterCatchFirstDown = StringParser.ToNullableDouble(x[339]);
            
            #endregion
        }

        public NflfastrPlayByPlay()
        {
        }

        #region NflfastrPlayByPlayProperties fields

        public int PlayId { get; set; }
        public int Week { get; set; }
        public string GameId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string PosTeam { get; set; }
        public string DefTeam { get; set; }
        public string SeasonType { get; set; }
        public int? YardLine100 { get; set; }
        public int QuarterSecondsRemaining { get; set; }
        public int HalfSecondsRemaining { get; set; }
        public int GameSecondsRemaining { get; set; }
        public int? Drive { get; set; }
        public int Qtr { get; set; }
        public int? Down { get; set; }
        public string Desc { get; set; }
        public string PlayType { get; set; }
        public int YdsToGo { get; set; }
        public int? YardsNet { get; set; }
        public int? YardsGained { get; set; }
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
        public bool? IsSuccess { get; set; }
        public bool IsPass { get; set; }
        public bool IsRush { get; set; }
        public bool? IsFirstDown { get; set; }
        public bool IsAbortedPlay { get; set; }
        public bool Play { get; set; }
        public int Shotgun { get; set; }
        public int NoHuddle { get; set; }
        public int? QbDropBack { get; set; }
        public int QbKneel { get; set; }
        public int QbSpike { get; set; }
        public int QbScramble { get; set; }
        public string PasserName { get; set; }
        public string RusherName { get; set; }
        public string ReceiverName { get; set; }
        public string Name { get; set; }
        public double? CompletionProbability { get; set; }
        public double? CompletionProbabilityOverExpected { get; set; }
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
