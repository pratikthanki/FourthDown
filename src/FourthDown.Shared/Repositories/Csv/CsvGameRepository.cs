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
    public class CsvGameRepository : IGameRepository, ICollectorGameRepository
    {
        private static ITracer _tracer;
        private static ILogger<CsvGameRepository> _logger;
        private readonly IRequestHelper _requestHelper;
        
        private DateTime _lastCacheUpdateDateTime = DateTime.MinValue;
        private readonly TimeSpan _cacheUpdateFrequency = TimeSpan.FromHours(1);
        private const int OneHourMilliseconds = 1_000 * 60;
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
            if (!_cacheInitialized)
            {
                await InitializeCache(cancellationToken);
            }

            return _gamesPerSeasonCache[season];
        }
        
        public async Task<IEnumerable<Game>> GetGamesForTeam(string team, CancellationToken cancellationToken)
        {
            if (!_cacheInitialized)
            {
                await InitializeCache(cancellationToken);
            }

            return _gamesPerSeasonCache.SelectMany(x => x.Value.Where(g => g.HomeTeam == team || g.AwayTeam == team));
        }

        public async Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken)
        {
            return await GetGamesAsync(cancellationToken);
        }

        public async Task TryPopulateCacheAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                var nextUpdateTime = _lastCacheUpdateDateTime.Add(_cacheUpdateFrequency);
                if (nextUpdateTime < DateTime.UtcNow)
                {
                    continue;
                }

                await InitializeCache(cancellationToken);
                await Task.Delay(OneHourMilliseconds, cancellationToken);
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
            const string url = RepositoryEndpoints.GamesEndpoint;
            var response = await _requestHelper.GetRequestResponse(url, cancellationToken);

            _logger.LogInformation($"Fetching data. Url: {url}; Status: {response.StatusCode}");

            var responseBody = await response.Content.ReadAsStringAsync();

            var csvResponse = responseBody
                .Split("\n")
                .Skip(1)
                .Select(x => x.Split(","));

            return response.IsSuccessStatusCode ? ProcessGamesResponse(csvResponse) : Enumerable.Empty<Game>();
        }

        private static IEnumerable<Game> ProcessGamesResponse(IEnumerable<string[]> csvResponse)
        {
            var games = new List<Game>();
            Parallel.ForEach(csvResponse, fields =>
            {
                if (fields.All(x => x == ""))
                {
                    // do nothing
                }
                else
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
                        OldGameId = fields[15],
                        Espn = StringParser.ToIntDefaultZero(fields[16]),
                        AwayRest = StringParser.ToInt(fields[17]),
                        HomeRest = StringParser.ToInt(fields[18]),
                        AwayMoneyline = StringParser.ToDoubleDefaultZero(fields[19]),
                        HomeMoneyline = StringParser.ToDoubleDefaultZero(fields[20]),
                        SpreadLine = StringParser.ToDoubleDefaultZero(fields[21]),
                        AwaySpreadOdds = StringParser.ToDoubleDefaultZero(fields[22]),
                        HomeSpreadOdds = StringParser.ToDoubleDefaultZero(fields[23]),
                        TotalLine = StringParser.ToDoubleDefaultZero(fields[24]),
                        UnderOdds = StringParser.ToDoubleDefaultZero(fields[25]),
                        OverOdds = StringParser.ToDoubleDefaultZero(fields[26]),
                        DivGame = StringParser.ToBool(fields[27]),
                        Roof = fields[28],
                        Surface = fields[29],
                        Temp = StringParser.ToIntDefaultZero(fields[30]),
                        Wind = StringParser.ToIntDefaultZero(fields[31]),
                        AwayQbId = fields[32],
                        HomeQbId = fields[33],
                        AwaQbName = fields[34],
                        HomeQbName = fields[35],
                        AwayCoach = fields[36],
                        HomeCoach = fields[37],
                        Referee = fields[38],
                        StadiumId = fields[39],
                        Stadium = fields[40]
                    };

                    games.Add(game);
                }
            });

            return games;
        }
    }
}