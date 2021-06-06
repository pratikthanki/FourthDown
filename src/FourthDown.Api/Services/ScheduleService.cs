using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Api.Parameters;
using FourthDown.Shared.Repositories;
using OpenTracing;

namespace FourthDown.Api.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly ITracer _tracer;
        private readonly IGameRepository _gameRepository;
        private readonly DateTime _today = DateTime.UtcNow;

        private DateTime _lastCacheUpdateDateTime = DateTime.MaxValue;
        private readonly TimeSpan _cacheUpdateFrequency = TimeSpan.FromDays(-1);
        private Dictionary<int, IEnumerable<Game>> _gamesPerSeasonCache = new Dictionary<int, IEnumerable<Game>>();

        public ScheduleService(
            ITracer tracer,
            IGameRepository gameRepository)
        {
            _tracer = tracer;
            _gameRepository = gameRepository;
        }

        public async Task<IEnumerable<Game>> GetGames(
            ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            using var scope = _tracer.InitializeTrace(nameof(GetGames));

            scope.LogStart(nameof(GetGames));

            await UpdateGamesCache(cancellationToken);

            var currentSeason = _today.Month > 8 ? _today.Year : _today.Year - 1;
            var season = queryParameter.Season ?? currentSeason;

            var games = _gamesPerSeasonCache[season];

            if (!string.IsNullOrWhiteSpace(queryParameter.Team))
            {
                games = games.Where(x => x.HomeTeam == queryParameter.Team || x.AwayTeam == queryParameter.Team);
            }

            if (queryParameter.Week != null)
            {
                games = games.Where(x => x.Week == queryParameter.Week);
            }

            scope.LogEnd(nameof(GetGames));

            return games;
        }

        public async Task<IEnumerable<Game>> GetGamesBetween(
            GameResultQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var team = queryParameter.Team;
            var opposition = queryParameter.Opposition;
            var offset = queryParameter.GameOffset;
            var gameType = queryParameter.ToGameTypeFilter();
            
            await UpdateGamesCache(cancellationToken);

            var games = _gamesPerSeasonCache
                .SelectMany(x => x.Value.Where(g => g.HomeTeam == team || g.AwayTeam == team));

            if (gameType != GameTypeFilter.All)
            {
                games = gameType == GameTypeFilter.Reg
                    ? games.Where(x => x.GameType == GameType.REG)
                    : games.Where(x => x.GameType != GameType.REG);
            }

            if (!string.IsNullOrWhiteSpace(opposition))
            {
                games = games.Where(x => x.HomeTeam == opposition || x.AwayTeam == opposition);
            }

            games = games.OrderByDescending(x => x.Gameday);

            return games.Take(offset);
        }

        private async Task UpdateGamesCache(CancellationToken cancellationToken)
        {
            if (!_gamesPerSeasonCache.Any() || _lastCacheUpdateDateTime < DateTime.UtcNow.Add(_cacheUpdateFrequency))
            {
                var task = Task.Run(async () =>
                {
                    var games = await _gameRepository.GetGamesAsync(cancellationToken);

                    return games.GroupBy(x => x.Season).ToDictionary(x => x.Key, x => x.AsEnumerable());

                }, cancellationToken);

                _gamesPerSeasonCache = await task;
                _lastCacheUpdateDateTime = DateTime.UtcNow;
            }
        }
    }
}