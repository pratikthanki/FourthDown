using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Shared.Repositories.Csv
{
    public class CsvGameRepository : IGameRepository
    {
        private static ITracer _tracer;
        private static ILogger<CsvGameRepository> _logger;
        private readonly IRequestHelper _requestHelper;

        private DateTime _lastCacheUpdateDateTime = DateTime.MinValue;
        private readonly TimeSpan _cacheUpdateFrequency = TimeSpan.FromHours(1);
        private const int CacheDelayMilliseconds = 60 * 60 * 1_000; // 1 hour in milliseconds
        private readonly ConcurrentDictionary<int, ConcurrentBag<Game>> _gamesPerSeasonCache;
        private bool _cacheInitialized;

        public CsvGameRepository(
            ITracer tracer,
            ILogger<CsvGameRepository> logger,
            IRequestHelper requestHelper)
        {
            _tracer = tracer;
            _logger = logger;
            _requestHelper = requestHelper;
            _gamesPerSeasonCache = new ConcurrentDictionary<int, ConcurrentBag<Game>>();
        }

        public async Task<IEnumerable<Game>> GetGamesForSeason(int season, CancellationToken cancellationToken)
        {
            if (!_cacheInitialized) await InitializeCache(cancellationToken);

            return _gamesPerSeasonCache.TryGetValue(season, out var games) ? games : Enumerable.Empty<Game>();
        }

        public async Task<IEnumerable<Game>> GetGamesForTeam(string team, CancellationToken cancellationToken)
        {
            if (!_cacheInitialized) await InitializeCache(cancellationToken);

            return _gamesPerSeasonCache.SelectMany(x => x.Value.Where(g => g.HomeTeam == team || g.AwayTeam == team));
        }

        public async Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Querying for all games.");
            return await GetGamesAsync(cancellationToken);
        }

        public async Task TryPopulateCacheAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (_lastCacheUpdateDateTime.Add(_cacheUpdateFrequency) >= DateTime.UtcNow) continue;

                _logger.LogInformation($"Starting cache refresh: {nameof(Game)}");

                await InitializeCache(cancellationToken);

                _logger.LogInformation($"Finished cache refresh: {nameof(Game)}");

                await Task.Delay(CacheDelayMilliseconds, cancellationToken);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private async Task InitializeCache(CancellationToken cancellationToken)
        {
            _cacheInitialized = false;

            var games = await GetGamesAsync(cancellationToken);
            foreach (var seasonGrouping in games.GroupBy(x => x.Season))
            {
                _gamesPerSeasonCache[seasonGrouping.Key] = new ConcurrentBag<Game>(seasonGrouping);
            }

            _cacheInitialized = true;
            _lastCacheUpdateDateTime = DateTime.UtcNow;
        }

        private async Task<IEnumerable<Game>> GetGamesAsync(CancellationToken cancellationToken)
        {
            var response =
                await _requestHelper.GetRequestResponse(RepositoryEndpoints.GamesEndpoint, cancellationToken);

            _logger.LogInformation(
                $"Fetching data. Url: {RepositoryEndpoints.GamesEndpoint}; Status: {response.StatusCode}");

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            var csvResponse = responseBody
                .Split("\n")
                .Skip(1) // header row
                .Select(x => x.Split(","));

            return response.IsSuccessStatusCode ? ProcessGamesResponse(csvResponse) : Enumerable.Empty<Game>();
        }

        private static IEnumerable<Game> ProcessGamesResponse(IEnumerable<string[]> csvResponse)
        {
            var games = new List<Game>();
            Parallel.ForEach(csvResponse, fields =>
            {
                if (!fields.All(string.IsNullOrEmpty))
                {
                    var game = new Game
                    {
                        GameId = fields[0],
                        Season = StringParser.ToInt(fields[1]),
                        GameType = fields[2],
                        Week = StringParser.ToInt(fields[3]),
                        Gameday = StringParser.ToDateTime(fields[4], "yyyy-MM-dd"),
                        Weekday = fields[5],
                        Gametime = string.IsNullOrWhiteSpace(fields[6]) ? "00:00" : fields[6],
                        AwayTeam = fields[7],
                        AwayScore = StringParser.ToIntDefaultZero(fields[8]),
                        HomeTeam = fields[9],
                        HomeScore = StringParser.ToIntDefaultZero(fields[10]),
                        Location = fields[11],
                        Result = StringParser.ToIntDefaultZero(fields[12]),
                        Total = StringParser.ToIntDefaultZero(fields[13]),
                        Overtime = StringParser.ToBool(fields[14]),

                        // ID fields to map against other data sources
                        OldGameId = fields[15],
                        Gsis = fields[16],
                        NflDetailId = fields[17],
                        Pfr = fields[18],
                        Pff = fields[19],
                        Espn = StringParser.ToIntDefaultZero(fields[20]),
                        Ftn = fields[21],

                        AwayRest = StringParser.ToInt(fields[22]),
                        HomeRest = StringParser.ToInt(fields[23]),
                        AwayMoneyline = StringParser.ToDoubleDefaultZero(fields[24]),
                        HomeMoneyline = StringParser.ToDoubleDefaultZero(fields[25]),
                        SpreadLine = StringParser.ToDoubleDefaultZero(fields[26]),
                        AwaySpreadOdds = StringParser.ToDoubleDefaultZero(fields[27]),
                        HomeSpreadOdds = StringParser.ToDoubleDefaultZero(fields[28]),
                        TotalLine = StringParser.ToDoubleDefaultZero(fields[29]),
                        UnderOdds = StringParser.ToDoubleDefaultZero(fields[30]),
                        OverOdds = StringParser.ToDoubleDefaultZero(fields[31]),
                        DivGame = StringParser.ToBool(fields[32]),
                        Roof = fields[33],
                        Surface = fields[34],
                        Temp = StringParser.ToIntDefaultZero(fields[35]),
                        Wind = StringParser.ToIntDefaultZero(fields[36]),
                        AwayQbId = fields[37],
                        HomeQbId = fields[38],
                        AwaQbName = fields[39],
                        HomeQbName = fields[40],
                        AwayCoach = fields[41],
                        HomeCoach = fields[42],
                        Referee = fields[43],
                        StadiumId = fields[44],
                        Stadium = fields[45]
                    };

                    games.Add(game);
                }
            });

            return games;
        }
    }
}