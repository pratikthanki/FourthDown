using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Api.Repositories.Csv
{
    public class CsvPlayByPlayRepository : IPlayByPlayRepository
    {
        private readonly ITracer _tracer;
        private static ILogger<CsvPlayByPlayRepository> _logger;

        public CsvPlayByPlayRepository(
            ITracer tracer,
            ILogger<CsvPlayByPlayRepository> logger)
        {
            _tracer = tracer;
            _logger = logger;
        }

        public async Task<IEnumerable<PlayByPlay>> GetPlayByPlaysAsync(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetPlayByPlaysAsync));
            
            scope.LogStart(nameof(GetPlayByPlaysAsync));
            
            var season = queryParameter.Season;

            var path = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_{season}.csv.gz?raw=true";

            var response = await RequestHelper.GetRequestResponse(path, cancellationToken);
            _logger.LogInformation($"Fetching data. Url: {path}; Status: {response.StatusCode}");

            var stream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<PlayByPlay>();

            var responseString = await ResponseHelper.ReadCompressedStreamToString(stream);

            var team = queryParameter.Team;
            var week = queryParameter.Week;
            var results = new List<PlayByPlay>();

            scope.LogStart(nameof(ProcessPlayByPlayResponse));

            foreach (var play in ProcessPlayByPlayResponse(responseString))
            {
                if (string.IsNullOrWhiteSpace(team))
                {
                    results.Add(play);
                }
                else
                {
                    if (play.AwayTeam == team || play.HomeTeam == team)
                        results.Add(play);
                }
            }

            scope.LogEnd(nameof(ProcessPlayByPlayResponse));
            
            if (week != null)
            {
                results = results
                    .GroupBy(x => x.Week)
                    .ToDictionary(x => x.Key, x => x.ToList())[(int) week];
            }

            scope.LogEnd(nameof(GetPlayByPlaysAsync));

            scope.Span.SetTag("Total rows", results.Count);
            
            return results;
        }

        private static IEnumerable<PlayByPlay> ProcessPlayByPlayResponse(string responseBody)
        {
            var csvResponse = responseBody
                .Split("\n")
                .Skip(1);

            foreach (var row in csvResponse)
            {
                var x = SplitCsv(row);

                if (x.All(cell => cell == ""))
                    continue;

                var Play = new PlayByPlay();

                #region PlayByPlay fields
                Play.PlayId = StringParser.ToInt(x[0]);
                Play.GameId = StringParser.ToString(x[1]);
                Play.HomeTeam = StringParser.ToString(x[3]);
                Play.AwayTeam = StringParser.ToString(x[4]);
                Play.SeasonType = StringParser.ToString(x[5]);
                Play.Week = StringParser.ToInt(x[6]);
                Play.PosTeam = StringParser.ToString(x[7]);
                Play.PosTeamType = StringParser.ToString(x[8]);
                Play.DefTeam = StringParser.ToString(x[9]);
                Play.SideOfField = StringParser.ToString(x[10]);
                Play.YardLine100 = StringParser.ToNullableInt(x[11]);
                Play.QuarterSecondsRemaining = StringParser.ToInt(x[13]);
                Play.HalfSecondsRemaining = StringParser.ToInt(x[14]);
                Play.GameSecondsRemaining = StringParser.ToInt(x[15]);
                Play.Drive = StringParser.ToNullableInt(x[18]);
                Play.Qtr = StringParser.ToInt(x[20]);
                Play.Down = StringParser.ToNullableInt(x[21]);
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
                Play.HomeTimeoutsRemaining = StringParser.ToInt(x[46]);
                Play.AwayTimeoutsRemaining = StringParser.ToInt(x[47]);
                Play.Timeout = StringParser.ToNullableInt(x[48]);
                Play.TimeoutTeam = StringParser.ToString(x[49]);
                Play.TdTeam = StringParser.ToString(x[50]);
                Play.TotalHomeScore = StringParser.ToInt(x[53]);
                Play.TotalAwayScore = StringParser.ToInt(x[54]);
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
                Play.HomeWpPost = StringParser.ToNullableDouble(x[95]);
                Play.AwayWpPost = StringParser.ToNullableDouble(x[96]);
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
                Play.CompletionProbability = StringParser.ToNullableDouble(x[262]);
                Play.CompletionProbabilityOverExpected = StringParser.ToNullableDouble(x[263]);
                Play.SeriesResult = StringParser.ToString(x[266]);
                Play.PlayTypeNfl = StringParser.ToString(x[275]);
                Play.FixedDrive = StringParser.ToInt(x[280]);
                Play.FixedDriveResult = StringParser.ToString(x[281]);
                Play.DriveRealStartTime = StringParser.ToString(x[282]);
                Play.DrivePlayCount = StringParser.ToNullableInt(x[283]);
                Play.DriveTimeOfPossession = StringParser.ToString(x[284]);
                Play.DriveFirstDowns = StringParser.ToNullableInt(x[285]);
                Play.DriveInside20 = StringParser.ToNullableInt(x[286]);
                Play.DriveEndedWithScore = StringParser.ToNullableInt(x[287]);
                Play.DriveYardsPenalized = StringParser.ToNullableInt(x[290]);
                Play.DriveStartTransition = StringParser.ToString(x[291]);
                Play.DriveEndTransition = StringParser.ToString(x[292]);
                Play.DriveStartYardLine = StringParser.ToString(x[295]);
                Play.DriveEndYardLine = StringParser.ToString(x[296]);
                Play.StartDrivePlayId = StringParser.ToNullableInt(x[297]);
                Play.EndDrivePlayId = StringParser.ToNullableInt(x[298]);
                Play.IsSuccess = StringParser.ToNullableBool(x[315]);
                Play.PasserName = StringParser.ToString(x[316]);
                Play.RusherName = StringParser.ToString(x[318]);
                Play.ReceiverName = StringParser.ToString(x[320]);
                Play.IsPass = StringParser.ToBool(x[322]);
                Play.IsRush = StringParser.ToBool(x[323]);
                Play.IsFirstDown = StringParser.ToNullableBool(x[324]);
                Play.IsAbortedPlay = StringParser.ToBool(x[325]);
                Play.Play = StringParser.ToBool(x[327]);
                Play.Name = StringParser.ToString(x[331]);
                Play.QbEpa = StringParser.ToNullableDouble(x[334]);
                Play.ExpectedYardsAfterCatchEpa = StringParser.ToNullableDouble(x[335]);
                Play.ExpectedYardsAfterCatchMeanYardage = StringParser.ToNullableDouble(x[336]);
                Play.ExpectedYardsAfterCatchMedianYardage = StringParser.ToNullableInt(x[337]);
                Play.ExpectedYardsAfterCatchSuccess = StringParser.ToNullableDouble(x[338]);
                Play.ExpectedYardsAfterCatchFirstDown = StringParser.ToNullableDouble(x[339]);

                #endregion

                yield return Play;
            }
        }

        private static string[] SplitCsv(string line)
        {
            var result = new List<string>();
            var currentStr = new StringBuilder("");
            var inQuotes = false;

            foreach (var T in line)
                if (T == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (T == ',')
                {
                    if (!inQuotes)
                    {
                        result.Add(currentStr.ToString());
                        currentStr.Clear();
                    }
                    else
                    {
                        currentStr.Append(T);
                    }
                }
                else
                {
                    currentStr.Append(T);
                }

            result.Add(currentStr.ToString());

            return result.ToArray();
        }
    }
}