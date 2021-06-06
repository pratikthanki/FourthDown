using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;
#pragma warning disable 1998

namespace FourthDown.Shared.Repositories.Csv
{
    public class CsvPlayByPlayRepository : IPlayByPlayRepository
    {
        private readonly ITracer _tracer;
        private static ILogger<CsvPlayByPlayRepository> _logger;
        private readonly IRequestHelper _requestHelper;

        public CsvPlayByPlayRepository(
            ITracer tracer,
            ILogger<CsvPlayByPlayRepository> logger,
            IRequestHelper requestHelper)
        {
            _tracer = tracer;
            _logger = logger;
            _requestHelper = requestHelper;
        }

        public async IAsyncEnumerable<NflfastrPlayByPlay> GetPlayByPlaysAsync(
            int? season,
            string team,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetPlayByPlaysAsync));

            scope.LogStart(nameof(GetPlayByPlaysAsync));

            var path = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_{season}.csv.gz?raw=true";

            var response = await _requestHelper.GetRequestResponse(path, cancellationToken);
            _logger.LogInformation($"Fetching data. Url: {path}; Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                yield return new NflfastrPlayByPlay();

            _logger.LogInformation($"{nameof(GetPlayByPlaysAsync)}: before ReadCompressedStreamToString");

            var responseString = await ResponseHelper.ReadCompressedStreamToString(response);

            _logger.LogInformation($"{nameof(GetPlayByPlaysAsync)}: after ReadCompressedStreamToString");

            await foreach (var play in ProcessPlayByPlayResponse(responseString, team, scope)
                .WithCancellation(cancellationToken))
            {
                yield return play;
            }
        }

        private static async IAsyncEnumerable<NflfastrPlayByPlay> ProcessPlayByPlayResponse(
            string data,
            string team,
            IScope scope)
        {
            _logger.LogInformation($"Started method {nameof(ProcessPlayByPlayResponse)}");
            scope.LogStart(nameof(ProcessPlayByPlayResponse));

            var csvResponse = data
                .Split("\n")
                .Skip(1) // Skip header row
                // .Where(x => x.Any(cell => cell != "")) // Account for empty lines
                .ToList();

            // Last element is an empty line
            csvResponse.RemoveAt(csvResponse.Count - 1);

            _logger.LogInformation($"{nameof(ProcessPlayByPlayResponse)}: after csvResponse");

            foreach (var line in csvResponse)
            {
                var play = SplitCsvLine(line, team);

                if (play == null) continue;

                if ((play.IsPass || play.IsRush) && play.Down != null) yield return play;
            }

            scope.LogEnd(nameof(ProcessPlayByPlayResponse));
            _logger.LogInformation($"Finished method {nameof(ProcessPlayByPlayResponse)}");
        }

        private static NflfastrPlayByPlay SplitCsvLine(string line, string team)
        {
            var result = new List<string>();
            var currentStr = new StringBuilder("");
            var inQuotes = false;

            foreach (var T in line)
            {
                switch (T)
                {
                    case '\"':
                        inQuotes = !inQuotes;
                        break;
                    case ',' when !inQuotes:
                        result.Add(currentStr.ToString());
                        currentStr.Clear();
                        break;
                    case ',':
                        currentStr.Append(T);
                        break;
                    default:
                        currentStr.Append(T);
                        break;
                }
            }

            result.Add(currentStr.ToString());
            var final = new NflfastrPlayByPlay(result.ToArray());

            if (string.IsNullOrWhiteSpace(team)) return final;

            return final.PosTeam == team ? final : null;
        }
    }
}