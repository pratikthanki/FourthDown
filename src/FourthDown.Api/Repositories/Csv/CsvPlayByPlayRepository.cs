using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
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

        public async Task<IEnumerable<NflfastrPlayByPlay>> GetPlayByPlaysAsync(
            int? season,
            string team,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetPlayByPlaysAsync));

            scope.LogStart(nameof(GetPlayByPlaysAsync));

            var path = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_{season}.csv.gz?raw=true";

            var response = await RequestHelper.GetRequestResponse(path, cancellationToken);
            _logger.LogInformation($"Fetching data. Url: {path}; Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<NflfastrPlayByPlay>();

            var stream = await response.Content.ReadAsByteArrayAsync();

            _logger.LogInformation($"{nameof(GetPlayByPlaysAsync)}: before ReadCompressedStreamToString");

            var responseStringTask = ResponseHelper.ReadCompressedStreamToString(stream);

            _logger.LogInformation($"{nameof(GetPlayByPlaysAsync)}: after ReadCompressedStreamToString");

            var results = new List<NflfastrPlayByPlay>();

            _logger.LogInformation($"Started method {nameof(ProcessPlayByPlayResponse)}");

            await foreach (var play in ProcessPlayByPlayResponse(responseStringTask, scope)
                .WithCancellation(cancellationToken))
            {
                if (string.IsNullOrWhiteSpace(team))
                {
                    results.Add(play);
                }
                else
                {
                    if (play.AwayTeam == team || play.HomeTeam == team)
                    {
                        results.Add(play);
                    }
                }
            }

            _logger.LogInformation($"Finished method {nameof(ProcessPlayByPlayResponse)}");

            scope.LogEnd(nameof(GetPlayByPlaysAsync));

            scope.Span.SetTag("Total rows", results.Count);

            return results;
        }

        private static async IAsyncEnumerable<NflfastrPlayByPlay> ProcessPlayByPlayResponse(Task<string> dataTask,
            IScope scope)
        {
            scope.LogStart(nameof(ProcessPlayByPlayResponse));

            var responseBody = await dataTask;

            _logger.LogInformation($"{nameof(ProcessPlayByPlayResponse)}: before csv response");

            var csvResponse = responseBody
                .Split("\n")
                .Skip(1) // Skip header row
                .Select(x => StringParser.SplitCsvLine(x)) // Parse lines and delimit by column and ignore quotes
                .Where(x => !x.All(cell => cell == "")) // Account for empty lines
                .ToList();

            _logger.LogInformation($"{nameof(ProcessPlayByPlayResponse)}: after csv response");

            var groupedPlays = csvResponse
                .GroupBy(x => x[28])
                .ToDictionary(x => x.Key, x => x.ToList());

            _logger.LogInformation($"{nameof(ProcessPlayByPlayResponse)}: after grouped plays");

            var keysToReturn = new List<string>() {"pass", "run"};
            foreach (var key in keysToReturn)
            {
                _logger.LogInformation($"{nameof(ProcessPlayByPlayResponse)}: after {key} foreach");

                foreach (var row in groupedPlays[key])
                {
                    var Play = new NflfastrPlayByPlay(row);
                    if (Play.IsPass || Play.IsRush || Play.Down != null)
                        yield return Play;
                }

                _logger.LogInformation($"{nameof(ProcessPlayByPlayResponse)}: after {key} foreach");
            }

            scope.LogEnd(nameof(ProcessPlayByPlayResponse));
        }
    }
}