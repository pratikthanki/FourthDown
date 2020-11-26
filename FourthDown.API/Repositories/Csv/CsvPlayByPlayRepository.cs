using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Utilities;

namespace FourthDown.Api.Repositories.Csv
{
    public class CsvPlayByPlayRepository : IPlayByPlayRepository
    {
        public async Task<IEnumerable<PlayByPlay>> GetPlayByPlays(
            PlayByPlayQueryParameter queryParameter, 
            CancellationToken cancellationToken)
        {
            var season = queryParameter.Season;
            var team = queryParameter.Team;
            var week = queryParameter.Week;
            var gameId = queryParameter.GameId;
            
            var baseUrl = "https://github.com/";
            var path = $"{baseUrl}/guga31bb/nflfastR-data/blob/master/data/play_by_play_{season}.csv.gz?raw=true";

            var response = await RequestHelper.GetRequestResponse(path, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync();
            
            var data = await ResponseHelper.ReadCompressedStreamToString(stream);
            
            var plaByPlays = ProcessPlayByPlayResponse(data);

            if (!string.IsNullOrWhiteSpace(team))
                plaByPlays = plaByPlays.Where(x => x.AwayTeam == team || x.HomeTeam == team);
            
            if (week != null)
                plaByPlays = plaByPlays.Where(x => x.Week == week);
            
            return plaByPlays.Where(x => x.Season == season);
        }

        private static IEnumerable<PlayByPlay> ProcessPlayByPlayResponse(string responseBody)
        {
            var csvResponse = responseBody
                .Split("\n")
                .Skip(1);

            var results = new List<PlayByPlay>();

            foreach (var row in csvResponse)
            {
                var x = SplitCsv(row);

                if (x.All(cell => cell == ""))
                    continue;

                #region PlayByPlay fields

                var Play = new PlayByPlay();
                Play.PlayId = StringParser.ToInt(x[0]);
                Play.GameId = StringParser.ToString(x[1]);
                Play.OldGameId = StringParser.ToInt(x[2]);
                Play.HomeTeam = StringParser.ToString(x[3]);
                Play.AwayTeam = StringParser.ToString(x[4]);
                Play.SeasonType = StringParser.ToString(x[5]);
                Play.Week = StringParser.ToInt(x[6]);
                Play.PosTeam = StringParser.ToString(x[7]);
                Play.PosTeamType = StringParser.ToString(x[8]);
                Play.DefTeam = StringParser.ToString(x[9]);
                Play.SideOfField = StringParser.ToString(x[10]);
                Play.YardLine100 = StringParser.ToNullableInt(x[11]);
                Play.GameDate = StringParser.ToDateTimeOrNull(x[12]);
                Play.QuarterSecondsRemaining = StringParser.ToInt(x[13]);
                Play.HalfSecondsRemaining = StringParser.ToInt(x[14]);
                Play.GameSecondsRemaining = StringParser.ToInt(x[15]);
                Play.GameHalf = StringParser.ToString(x[16]);
                Play.QuarterEnd = StringParser.ToInt(x[17]);
                Play.Drive = StringParser.ToNullableInt(x[18]);
                Play.Sp = StringParser.ToInt(x[19]);
                Play.Qtr = StringParser.ToInt(x[20]);
                Play.Down = StringParser.ToNullableInt(x[21]);
                Play.GoalToGo = StringParser.ToInt(x[22]);
                Play.Time = StringParser.ToString(x[23]);
                Play.YardLine = StringParser.ToString(x[24]);
                Play.YdsToGo = StringParser.ToInt(x[25]);
                Play.YardsNet = StringParser.ToNullableInt(x[26]);
                Play.Desc = StringParser.ToString(x[27]);
                Play.PlayType = StringParser.ToString(x[28]);
                Play.YardsGained = StringParser.ToNullableInt(x[29]);
                Play.Shotgun = StringParser.ToInt(x[30]);
                Play.NoHuddle = StringParser.ToInt(x[31]);
                Play.QbDropBack = StringParser.ToNullableInt(x[32]);
                Play.QbKneel = StringParser.ToInt(x[33]);
                Play.QbSpike = StringParser.ToInt(x[34]);
                Play.QbScramble = StringParser.ToInt(x[35]);
                Play.PassLength = StringParser.ToString(x[36]);
                Play.PassLocation = StringParser.ToString(x[37]);
                Play.AirYards = StringParser.ToNullableInt(x[38]);
                Play.YardsAfterCatch = StringParser.ToNullableInt(x[39]);
                Play.RunLocation = StringParser.ToString(x[40]);
                Play.RunGap = StringParser.ToString(x[41]);
                Play.FieldGoalResult = StringParser.ToString(x[42]);
                Play.KickDistance = StringParser.ToNullableInt(x[43]);
                Play.ExtraPointResult = StringParser.ToString(x[44]);
                Play.TwoPointConvResult = StringParser.ToString(x[45]);
                Play.HomeTimeoutsRemaining = StringParser.ToInt(x[46]);
                Play.AwayTimeoutsRemaining = StringParser.ToInt(x[47]);
                Play.Timeout = StringParser.ToNullableInt(x[48]);
                Play.TimeoutTeam = StringParser.ToString(x[49]);
                Play.TdTeam = StringParser.ToString(x[50]);
                Play.PosTeamTimeoutsRemaining = StringParser.ToNullableInt(x[51]);
                Play.DefTeamTimeoutsRemaining = StringParser.ToNullableInt(x[52]);
                Play.TotalHomeScore = StringParser.ToInt(x[53]);
                Play.TotalAwayScore = StringParser.ToInt(x[54]);
                Play.PosTeamScore = StringParser.ToNullableInt(x[55]);
                Play.DefTeamScore = StringParser.ToNullableInt(x[56]);
                Play.ScoreDifferential = StringParser.ToNullableInt(x[57]);
                Play.PosTeamScorePost = StringParser.ToNullableInt(x[58]);
                Play.DefTeamScorePost = StringParser.ToNullableInt(x[59]);
                Play.ScoreDifferentialPost = StringParser.ToNullableInt(x[60]);
                Play.NoScoreProb = StringParser.ToDouble(x[61]);
                Play.OppFgProb = StringParser.ToDouble(x[62]);
                Play.OppSafetyProb = StringParser.ToDouble(x[63]);
                Play.OppTdProb = StringParser.ToDouble(x[64]);
                Play.FgProb = StringParser.ToDouble(x[65]);
                Play.SafetyProb = StringParser.ToDouble(x[66]);
                Play.TdProb = StringParser.ToDouble(x[67]);
                Play.ExtraPointProb = StringParser.ToDouble(x[68]);
                Play.TwoPointConversionProb = StringParser.ToDouble(x[69]);
                Play.Ep = StringParser.ToNullableDouble(x[70]);
                Play.Epa = StringParser.ToNullableDouble(x[71]);
                Play.TotalHomeEpa = StringParser.ToDouble(x[72]);
                Play.TotalAwayEpa = StringParser.ToDouble(x[73]);
                Play.TotalHomeRushEpa = StringParser.ToDouble(x[74]);
                Play.TotalAwayRushEpa = StringParser.ToDouble(x[75]);
                Play.TotalHomePassEpa = StringParser.ToDouble(x[76]);
                Play.TotalAwayPassEpa = StringParser.ToDouble(x[77]);
                Play.AirEpa = StringParser.ToNullableDouble(x[78]);
                Play.YacEpa = StringParser.ToNullableDouble(x[79]);
                Play.CompAirEpa = StringParser.ToNullableDouble(x[80]);
                Play.CompYacEpa = StringParser.ToNullableDouble(x[81]);
                Play.TotalHomeCompAirEpa = StringParser.ToDouble(x[82]);
                Play.TotalAwayCompAirEpa = StringParser.ToDouble(x[83]);
                Play.TotalHomeCompYacEpa = StringParser.ToDouble(x[84]);
                Play.TotalAwayCompYacEpa = StringParser.ToDouble(x[85]);
                Play.TotalHomeRawAirEpa = StringParser.ToDouble(x[86]);
                Play.TotalAwayRawAirEpa = StringParser.ToDouble(x[87]);
                Play.TotalHomeRawYacEpa = StringParser.ToDouble(x[88]);
                Play.TotalAwayRawYacEpa = StringParser.ToDouble(x[89]);
                Play.Wp = StringParser.ToNullableDouble(x[90]);
                Play.DefWp = StringParser.ToNullableDouble(x[91]);
                Play.HomeWp = StringParser.ToNullableDouble(x[92]);
                Play.AwayWp = StringParser.ToNullableDouble(x[93]);
                Play.Wpa = StringParser.ToNullableDouble(x[94]);
                Play.HomeWpPost = StringParser.ToNullableDouble(x[95]);
                Play.AwayWpPost = StringParser.ToNullableDouble(x[96]);
                Play.VegasWp = StringParser.ToNullableDouble(x[97]);
                Play.VegasHomeWp = StringParser.ToNullableDouble(x[98]);
                Play.TotalHomeRushWpa = StringParser.ToDouble(x[99]);
                Play.TotalAwayRushWpa = StringParser.ToDouble(x[100]);
                Play.TotalHomePassWpa = StringParser.ToDouble(x[101]);
                Play.TotalAwayPassWpa = StringParser.ToDouble(x[102]);
                Play.AirWpa = StringParser.ToNullableDouble(x[103]);
                Play.YacWpa = StringParser.ToNullableDouble(x[104]);
                Play.CompAirWpa = StringParser.ToNullableDouble(x[105]);
                Play.CompYacWpa = StringParser.ToNullableDouble(x[106]);
                Play.TotalHomeCompAirWpa = StringParser.ToDouble(x[107]);
                Play.TotalAwayCompAirWpa = StringParser.ToDouble(x[108]);
                Play.TotalHomeCompYacWpa = StringParser.ToDouble(x[109]);
                Play.TotalAwayCompYacWpa = StringParser.ToDouble(x[110]);
                Play.TotalHomeRawAirWpa = StringParser.ToDouble(x[111]);
                Play.TotalAwayRawAirWpa = StringParser.ToDouble(x[112]);
                Play.TotalHomeRawYacWpa = StringParser.ToDouble(x[113]);
                Play.TotalAwayRawYacWpa = StringParser.ToDouble(x[114]);
                Play.PuntBlocked = StringParser.ToNullableBool(x[115]);
                Play.FirstDownRush = StringParser.ToNullableBool(x[116]);
                Play.FirstDownPass = StringParser.ToNullableBool(x[117]);
                Play.FirstDownPenalty = StringParser.ToNullableBool(x[118]);
                Play.ThirdDownConverted = StringParser.ToNullableBool(x[119]);
                Play.ThirdDownFailed = StringParser.ToNullableBool(x[120]);
                Play.FourthDownConverted = StringParser.ToNullableBool(x[121]);
                Play.FourthDownFailed = StringParser.ToNullableBool(x[122]);
                Play.IncompletePass = StringParser.ToNullableBool(x[123]);
                Play.TouchBack = StringParser.ToNullableBool(x[124]);
                Play.Interception = StringParser.ToNullableBool(x[125]);
                Play.PuntInsideTwenty = StringParser.ToNullableBool(x[126]);
                Play.PuntInEndZone = StringParser.ToNullableBool(x[127]);
                Play.PuntOutOfBounds = StringParser.ToNullableBool(x[128]);
                Play.PuntDowned = StringParser.ToNullableBool(x[129]);
                Play.PuntFairCatch = StringParser.ToNullableBool(x[130]);
                Play.KickoffInsideTwenty = StringParser.ToNullableBool(x[131]);
                Play.KickoffInEndZone = StringParser.ToNullableBool(x[132]);
                Play.KickoffOutOfBounds = StringParser.ToNullableBool(x[133]);
                Play.KickoffDowned = StringParser.ToNullableBool(x[134]);
                Play.KickoffFairCatch = StringParser.ToNullableBool(x[135]);
                Play.FumbleForced = StringParser.ToNullableBool(x[136]);
                Play.FumbleNotForced = StringParser.ToNullableBool(x[137]);
                Play.FumbleOutOfBounds = StringParser.ToNullableBool(x[138]);
                Play.SoloTackle = StringParser.ToNullableBool(x[139]);
                Play.Safety = StringParser.ToNullableBool(x[140]);
                Play.Penalty = StringParser.ToNullableBool(x[141]);
                Play.TackledForLoss = StringParser.ToNullableBool(x[142]);
                Play.FumbleLost = StringParser.ToNullableBool(x[143]);
                Play.OwnKickoffRecovery = StringParser.ToNullableBool(x[144]);
                Play.OwnKickoffRecoveryTd = StringParser.ToNullableBool(x[145]);
                Play.QbHit = StringParser.ToNullableBool(x[146]);
                Play.RushAttempt = StringParser.ToNullableBool(x[147]);
                Play.PassAttempt = StringParser.ToNullableBool(x[148]);
                Play.Sack = StringParser.ToNullableBool(x[149]);
                Play.Touchdown = StringParser.ToNullableBool(x[150]);
                Play.PassTouchdown = StringParser.ToNullableBool(x[151]);
                Play.RushTouchdown = StringParser.ToNullableBool(x[152]);
                Play.ReturnTouchdown = StringParser.ToNullableBool(x[153]);
                Play.ExtraPointAttempt = StringParser.ToNullableBool(x[154]);
                Play.TwoPointAttempt = StringParser.ToNullableBool(x[155]);
                Play.FieldGoalAttempt = StringParser.ToNullableBool(x[156]);
                Play.KickoffAttempt = StringParser.ToNullableBool(x[157]);
                Play.PuntAttempt = StringParser.ToNullableBool(x[158]);
                Play.Fumble = StringParser.ToNullableBool(x[159]);
                Play.CompletePass = StringParser.ToNullableBool(x[160]);
                Play.AssistTackle = StringParser.ToNullableBool(x[161]);
                Play.LateralReception = StringParser.ToNullableBool(x[162]);
                Play.LateralRush = StringParser.ToNullableBool(x[163]);
                Play.LateralReturn = StringParser.ToNullableBool(x[164]);
                Play.LateralRecovery = StringParser.ToNullableBool(x[165]);
                Play.PasserPlayerId = StringParser.ToString(x[166]);
                Play.PasserPlayerName = StringParser.ToString(x[167]);
                Play.ReceiverPlayerId = StringParser.ToString(x[168]);
                Play.ReceiverPlayerName = StringParser.ToString(x[169]);
                Play.RusherPlayerId = StringParser.ToString(x[170]);
                Play.RusherPlayerName = StringParser.ToString(x[171]);
                Play.LateralReceiverPlayerId = StringParser.ToString(x[172]);
                Play.LateralReceiverPlayerName = StringParser.ToString(x[173]);
                Play.LateralRusherPlayerId = StringParser.ToString(x[174]);
                Play.LateralRusherPlayerName = StringParser.ToString(x[175]);
                Play.LateralSackPlayerId = StringParser.ToString(x[176]);
                Play.LateralSackPlayerName = StringParser.ToString(x[177]);
                Play.InterceptionPlayerId = StringParser.ToString(x[178]);
                Play.InterceptionPlayerName = StringParser.ToString(x[179]);
                Play.LateralInterceptionPlayerId = StringParser.ToString(x[180]);
                Play.LateralInterceptionPlayerName = StringParser.ToString(x[181]);
                Play.PuntReturnerPlayerId = StringParser.ToString(x[182]);
                Play.PuntReturnerPlayerName = StringParser.ToString(x[183]);
                Play.LateralPuntReturnerPlayerId = StringParser.ToString(x[184]);
                Play.LateralPuntReturnerPlayerName = StringParser.ToString(x[185]);
                Play.KickoffReturnerPlayerName = StringParser.ToString(x[186]);
                Play.KickoffReturnerPlayerId = StringParser.ToString(x[187]);
                Play.LateralKickoffReturnerPlayerId = StringParser.ToString(x[188]);
                Play.LateralKickoffReturnerPlayerName = StringParser.ToString(x[189]);
                Play.PunterPlayerId = StringParser.ToString(x[190]);
                Play.PunterPlayerName = StringParser.ToString(x[191]);
                Play.KickerPlayerName = StringParser.ToString(x[192]);
                Play.KickerPlayerId = StringParser.ToString(x[193]);
                Play.OwnKickoffRecoveryPlayerId = StringParser.ToString(x[194]);
                Play.OwnKickoffRecoveryPlayerName = StringParser.ToString(x[195]);
                Play.BlockedPlayerId = StringParser.ToString(x[196]);
                Play.BlockedPlayerName = StringParser.ToString(x[197]);
                Play.TackleForLoss1PlayerId = StringParser.ToString(x[198]);
                Play.TackleForLoss1PlayerName = StringParser.ToString(x[199]);
                Play.TackleForLoss2PlayerId = StringParser.ToString(x[200]);
                Play.TackleForLoss2PlayerName = StringParser.ToString(x[201]);
                Play.QbHit1PlayerId = StringParser.ToString(x[202]);
                Play.QbHit1PlayerName = StringParser.ToString(x[203]);
                Play.QbHit2PlayerId = StringParser.ToString(x[204]);
                Play.QbHit2PlayerName = StringParser.ToString(x[205]);
                Play.ForcedFumblePlayer1Team = StringParser.ToString(x[206]);
                Play.ForcedFumblePlayer1PlayerId = StringParser.ToString(x[207]);
                Play.ForcedFumblePlayer1PlayerName = StringParser.ToString(x[208]);
                Play.ForcedFumblePlayer2Team = StringParser.ToString(x[209]);
                Play.ForcedFumblePlayer2PlayerId = StringParser.ToString(x[210]);
                Play.ForcedFumblePlayer2PlayerName = StringParser.ToString(x[211]);
                Play.SoloTackle1Team = StringParser.ToString(x[212]);
                Play.SoloTackle2Team = StringParser.ToString(x[213]);
                Play.SoloTackle1PlayerId = StringParser.ToString(x[214]);
                Play.SoloTackle2PlayerId = StringParser.ToString(x[215]);
                Play.SoloTackle1PlayerName = StringParser.ToString(x[216]);
                Play.SoloTackle2PlayerName = StringParser.ToString(x[217]);
                Play.AssistTackle1PlayerId = StringParser.ToString(x[218]);
                Play.AssistTackle1PlayerName = StringParser.ToString(x[219]);
                Play.AssistTackle1Team = StringParser.ToString(x[220]);
                Play.AssistTackle2PlayerId = StringParser.ToString(x[221]);
                Play.AssistTackle2PlayerName = StringParser.ToString(x[222]);
                Play.AssistTackle2Team = StringParser.ToString(x[223]);
                Play.AssistTackle3PlayerId = StringParser.ToString(x[224]);
                Play.AssistTackle3PlayerName = StringParser.ToString(x[225]);
                Play.AssistTackle3Team = StringParser.ToString(x[226]);
                Play.AssistTackle4PlayerId = StringParser.ToString(x[227]);
                Play.AssistTackle4PlayerName = StringParser.ToString(x[228]);
                Play.AssistTackle4Team = StringParser.ToString(x[229]);
                Play.PassDefense1PlayerId = StringParser.ToString(x[230]);
                Play.PassDefense1PlayerName = StringParser.ToString(x[231]);
                Play.PassDefense2PlayerId = StringParser.ToString(x[232]);
                Play.PassDefense2PlayerName = StringParser.ToString(x[233]);
                Play.Fumbled1Team = StringParser.ToString(x[234]);
                Play.Fumbled1PlayerId = StringParser.ToString(x[235]);
                Play.Fumbled1PlayerName = StringParser.ToString(x[236]);
                Play.Fumbled2PlayerId = StringParser.ToString(x[237]);
                Play.Fumbled2PlayerName = StringParser.ToString(x[238]);
                Play.Fumbled2Team = StringParser.ToString(x[239]);
                Play.FumbleRecovery1Team = StringParser.ToString(x[240]);
                Play.FumbleRecovery1Yards = StringParser.ToString(x[241]);
                Play.FumbleRecovery1PlayerId = StringParser.ToString(x[242]);
                Play.FumbleRecovery1PlayerName = StringParser.ToString(x[243]);
                Play.FumbleRecovery2Team = StringParser.ToString(x[244]);
                Play.FumbleRecovery2Yards = StringParser.ToString(x[245]);
                Play.FumbleRecovery2PlayerId = StringParser.ToString(x[246]);
                Play.FumbleRecovery2PlayerName = StringParser.ToString(x[247]);
                Play.ReturnTeam = StringParser.ToString(x[248]);
                Play.ReturnYards = StringParser.ToNullableInt(x[249]);
                Play.PenaltyTeam = StringParser.ToString(x[250]);
                Play.PenaltyPlayerId = StringParser.ToString(x[251]);
                Play.PenaltyPlayerName = StringParser.ToString(x[252]);
                Play.PenaltyYards = StringParser.ToNullableInt(x[253]);
                Play.ReplayOrChallenge = StringParser.ToInt(x[254]);
                Play.ReplayOrChallengeResult = StringParser.ToString(x[255]);
                Play.PenaltyType = StringParser.ToString(x[256]);
                Play.DefensiveTwoPointAttempt = StringParser.ToNullableInt(x[257]);
                Play.DefensiveTwoPointConv = StringParser.ToNullableInt(x[258]);
                Play.DefensiveExtraPointAttempt = StringParser.ToNullableInt(x[259]);
                Play.DefensiveExtraPointConv = StringParser.ToNullableInt(x[260]);
                Play.Season = StringParser.ToInt(x[261]);
                Play.Cp = StringParser.ToNullableDouble(x[262]);
                Play.Cpoe = StringParser.ToNullableDouble(x[263]);
                Play.Series = StringParser.ToInt(x[264]);
                Play.SeriesSuccess = StringParser.ToInt(x[265]);
                Play.SeriesResult = StringParser.ToString(x[266]);
                Play.OrderSequence = StringParser.ToDouble(x[267]);
                Play.StartTime = StringParser.ToString(x[268]);
                Play.TimeOfDay = StringParser.ToString(x[269]);
                Play.Stadium = StringParser.ToString(x[270]);
                Play.Weather = StringParser.ToString(x[271]);
                Play.NflApiId = StringParser.ToString(x[272]);
                Play.PlayClock = StringParser.ToInt(x[273]);
                Play.PlayDeleted = StringParser.ToBool(x[274]);
                Play.PlayTypeNfl = StringParser.ToString(x[275]);
                Play.SpecialTeamsPlay = StringParser.ToBool(x[276]);
                Play.SpecialTeamsPlayType = StringParser.ToString(x[277]);
                Play.EndClockTime = StringParser.ToString(x[278]);
                Play.EndYardLine = StringParser.ToString(x[279]);
                Play.FixedDrive = StringParser.ToInt(x[280]);
                Play.FixedDriveResult = StringParser.ToString(x[281]);
                Play.DriveRealStartTime = StringParser.ToString(x[282]);
                Play.DrivePlayCount = StringParser.ToNullableInt(x[283]);
                Play.DriveTimeOfPossession = StringParser.ToString(x[284]);
                Play.DriveFirstDowns = StringParser.ToNullableInt(x[285]);
                Play.DriveInside20 = StringParser.ToNullableInt(x[286]);
                Play.DriveEndedWithScore = StringParser.ToNullableInt(x[287]);
                Play.DriveQuarterStart = StringParser.ToNullableInt(x[288]);
                Play.DriveQuarterEnd = StringParser.ToNullableInt(x[289]);
                Play.DriveYardsPenalized = StringParser.ToNullableInt(x[290]);
                Play.DriveStartTransition = StringParser.ToString(x[291]);
                Play.DriveEndTransition = StringParser.ToString(x[292]);
                Play.DriveGameClockStart = StringParser.ToString(x[293]);
                Play.DriveGameClockEnd = StringParser.ToString(x[294]);
                Play.DriveStartYardLine = StringParser.ToString(x[295]);
                Play.DriveEndYardLine = StringParser.ToString(x[296]);
                Play.DrivePlayIdStarted = StringParser.ToNullableInt(x[297]);
                Play.DrivePlayIdEnded = StringParser.ToNullableInt(x[298]);
                Play.AwayScore = StringParser.ToInt(x[299]);
                Play.HomeScore = StringParser.ToInt(x[300]);
                Play.Location = StringParser.ToString(x[301]);
                Play.Result = StringParser.ToInt(x[302]);
                Play.Total = StringParser.ToInt(x[303]);
                Play.SpreadLine = StringParser.ToDouble(x[304]);
                Play.TotalLine = StringParser.ToDouble(x[305]);
                Play.DivGame = StringParser.ToInt(x[306]);
                Play.Roof = StringParser.ToString(x[307]);
                Play.Surface = StringParser.ToString(x[308]);
                Play.Temp = StringParser.ToString(x[309]);
                Play.Wind = StringParser.ToString(x[310]);
                Play.HomeCoach = StringParser.ToString(x[311]);
                Play.AwayCoach = StringParser.ToString(x[312]);
                Play.StadiumId = StringParser.ToString(x[313]);
                Play.GameStadium = StringParser.ToString(x[314]);
                Play.Success = StringParser.ToNullableBool(x[315]);
                Play.Passer = StringParser.ToString(x[316]);
                Play.PasserJerseyNumber = StringParser.ToNullableInt(x[317]);
                Play.Rusher = StringParser.ToString(x[318]);
                Play.RusherJerseyNumber = StringParser.ToNullableInt(x[319]);
                Play.Receiver = StringParser.ToString(x[320]);
                Play.ReceiverJerseyNumber = StringParser.ToNullableInt(x[321]);
                Play.Pass = StringParser.ToBool(x[322]);
                Play.Rush = StringParser.ToBool(x[323]);
                Play.FirstDown = StringParser.ToNullableBool(x[324]);
                Play.AbortedPlay = StringParser.ToBool(x[325]);
                Play.Special = StringParser.ToBool(x[326]);
                Play.Play = StringParser.ToBool(x[327]);
                Play.PasserId = StringParser.ToString(x[328]);
                Play.RusherId = StringParser.ToString(x[329]);
                Play.ReceiverId = StringParser.ToString(x[330]);
                Play.Name = StringParser.ToString(x[331]);
                Play.JerseyNumber = StringParser.ToNullableInt(x[332]);
                Play.Id = StringParser.ToString(x[333]);
                Play.QbEpa = StringParser.ToNullableDouble(x[334]);
                Play.XyacEpa = StringParser.ToNullableDouble(x[335]);
                Play.XyacMeanYardage = StringParser.ToNullableDouble(x[336]);
                Play.XyacMedianYardage = StringParser.ToNullableInt(x[337]);
                Play.XyacSuccess = StringParser.ToNullableDouble(x[338]);
                Play.XyacFd = StringParser.ToNullableDouble(x[339]);

                #endregion

                results.Add(Play);
            }

            return results;
        }

        private static string[] SplitCsv(string line)
        {
            var result = new List<string>();
            var currentStr = new StringBuilder("");
            var inQuotes = false;

            foreach (var T in line)
            {
                if (T == '\"')
                    inQuotes = !inQuotes;
                else if (T == ',')
                {
                    if (!inQuotes)
                    {
                        result.Add(currentStr.ToString());
                        currentStr.Clear();
                    }
                    else
                        currentStr.Append(T);
                }
                else
                {
                    currentStr.Append(T);
                }
            }

            result.Add(currentStr.ToString());

            return result.ToArray();
        }
    }
}
