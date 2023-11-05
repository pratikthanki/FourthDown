#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Shared.Repositories.Csv
{
    public class CsvPlayByPlayRepository : IPlayByPlayRepository
    {
        private readonly ITracer _tracer;
        private readonly ILogger<CsvPlayByPlayRepository> _logger;
        private readonly IRequestHelper _requestHelper;

        private readonly ConcurrentBag<NflfastrPlayByPlayRow> _playByPlayRows = new();
        private DateTime _lastCacheUpdateDateTime = DateTime.MinValue;
        private readonly TimeSpan _cacheUpdateFrequency = TimeSpan.FromHours(1);
        private const int CacheDelayMilliseconds = 60 * 60 * 1_000; // 1 hour in milliseconds

        public CsvPlayByPlayRepository(
            ITracer tracer,
            ILogger<CsvPlayByPlayRepository> logger,
            IRequestHelper requestHelper)
        {
            _tracer = tracer;
            _logger = logger;
            _requestHelper = requestHelper;
        }

        public IEnumerable<NflfastrPlayByPlayRow> GetPlayByPlaysAsync(int season, string? team,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetPlayByPlaysAsync));

            _logger.LogInformation($"Started method {nameof(GetPlayByPlaysAsync)}");
            scope.LogStart(nameof(GetPlayByPlaysAsync));

            foreach (var play in _playByPlayRows)
            {
                // We only care about valid plays and downs
                if (play is { IsPass: false, IsRush: false } || play.Down == null) continue;

                // Filter by team if provided
                if (string.IsNullOrWhiteSpace(team) || play.PosTeam == team) yield return play;
            }

            scope.LogEnd(nameof(GetPlayByPlaysAsync));
            _logger.LogInformation($"Finished method {nameof(GetPlayByPlaysAsync)}");
        }

        public async Task TryPopulateCacheAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (_lastCacheUpdateDateTime.Add(_cacheUpdateFrequency) >= DateTime.UtcNow) continue;

                _logger.LogInformation($"Starting cache refresh: {nameof(Game)}");

                var currentSeason = StringParser.GetCurrentSeason();
                var path = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_{currentSeason}.csv.gz?raw=true";

                var response = await _requestHelper.GetRequestResponse(path, cancellationToken);
                _logger.LogInformation($"Fetching data. Url: {path}; Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                var responseString = await ResponseHelper.ReadCompressedStreamToString(response);

                // TODO make this nicer
                var data = responseString
                    .Split("\n")
                    .Skip(1) // Skip header row
                    // .Where(x => x.Any(cell => cell != "")) // Account for empty lines
                    .ToList();

                // Last element is an empty line
                data.RemoveAt(data.Count - 1);

                foreach (var line in data)
                {
                    var row = SplitLineToArray(line);
                    _playByPlayRows.Add(new NflfastrPlayByPlayRow(row.ToArray()));
                }

                _lastCacheUpdateDateTime = DateTime.UtcNow;

                _logger.LogInformation($"Finished cache refresh: {nameof(Game)}");

                await Task.Delay(CacheDelayMilliseconds, cancellationToken);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static List<string> SplitLineToArray(string line)
        {
            var result = new List<string>();
            var currentStr = new StringBuilder("");
            var inQuotes = false;

            foreach (var character in line)
            {
                switch (character)
                {
                    case '\"':
                        inQuotes = !inQuotes;
                        break;
                    case ',' when !inQuotes:
                        result.Add(currentStr.ToString());
                        currentStr.Clear();
                        break;
                    case ',':
                        currentStr.Append(character);
                        break;
                    default:
                        currentStr.Append(character);
                        break;
                }
            }

            result.Add(currentStr.ToString());

            return result;
        }
    }
}