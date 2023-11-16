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

        private readonly ConcurrentDictionary<string, ConcurrentBag<NflfastrPlayByPlayRow>> _pbpRowsByTeam = new();
        private readonly PeriodicTimer _periodicTimer;

        private readonly int _validRefreshHour = 9;
        private readonly int[] _validRefreshMonths = new[] { 1, 2, 9, 10, 11, 12 };

        public CsvPlayByPlayRepository(
            ITracer tracer,
            ILogger<CsvPlayByPlayRepository> logger,
            IRequestHelper requestHelper)
        {
            _tracer = tracer;
            _logger = logger;
            _requestHelper = requestHelper;
            _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(5));
        }

        public IEnumerable<NflfastrPlayByPlayRow> GetPlayByPlaysAsync(int season, string? team,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetPlayByPlaysAsync));

            _logger.LogInformation($"Started method {nameof(GetPlayByPlaysAsync)}");
            scope.LogStart(nameof(GetPlayByPlaysAsync));

            bool IsValidPlay(NflfastrPlayByPlayRow play)
            {
                return play is not { IsPass: false, IsRush: false } && play.Down != null;
            }

            // Filter by team if provided
            if (!string.IsNullOrWhiteSpace(team))
            {
                if (_pbpRowsByTeam.TryGetValue(team, out var playByPlayRows))
                {
                    foreach (var play in playByPlayRows)
                    {
                        if (IsValidPlay(play)) yield return play;
                    }

                    yield break;
                }
            }

            foreach (var teamPlays in _pbpRowsByTeam.Values)
            {
                foreach (var play in teamPlays)
                {
                    // We only care about valid plays and downs
                    if (IsValidPlay(play)) yield return play;
                }
            }

            scope.LogEnd(nameof(GetPlayByPlaysAsync));
            _logger.LogInformation($"Finished method {nameof(GetPlayByPlaysAsync)}");
        }

        public async Task TryPopulateCacheAsync(bool forceRefresh, CancellationToken cancellationToken = default)
        {
            if (forceRefresh) await RefreshAsync(cancellationToken);
            
            while (await _periodicTimer.WaitForNextTickAsync(cancellationToken))
            {
                // Attempt to refresh data everyday at 9am during the in-season months
                var now = DateTime.UtcNow;
                if (_validRefreshMonths.Contains(now.Month) && now.Hour >= _validRefreshHour)
                {
                    await RefreshAsync(cancellationToken);
                }
            }
        }

        private async Task RefreshAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Starting cache refresh: {nameof(Game)}");

            var currentSeason = StringParser.GetCurrentSeason();
            var path = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_{currentSeason}.csv.gz?raw=true";

            var response = await _requestHelper.GetRequestResponse(path, cancellationToken);
            _logger.LogInformation($"Fetching data. Url: {path}; Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode) return;

            var responseString = await ResponseHelper.ReadCompressedStreamToString(response);

            // TODO make this nicer
            var data = responseString
                .Split("\n")
                .Skip(1) // Skip header row
                // .Where(x => x.Any(cell => cell != "")) // Account for empty lines
                .ToList();

            // Last element is an empty line
            data.RemoveAt(data.Count - 1);

            Parallel.ForEach(data, line =>
            {
                var row = SplitLineToArray(line);
                if (row.Count != 372) return;

                var pbpRow = new NflfastrPlayByPlayRow(row);

                if (_pbpRowsByTeam.TryGetValue(pbpRow.PosTeam, out var rows))
                {
                    rows.Add(pbpRow);
                }
                else
                {
                    rows = new ConcurrentBag<NflfastrPlayByPlayRow>() { pbpRow };
                    _pbpRowsByTeam.TryAdd(pbpRow.PosTeam, rows);
                }
            });

            _logger.LogInformation($"Finished cache refresh: {nameof(Game)}");
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