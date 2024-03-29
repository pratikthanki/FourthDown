using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Extensions;
using FourthDown.Shared.Models;
using FourthDown.Api.Parameters;
using FourthDown.Shared.Repositories;
using OpenTracing;

namespace FourthDown.Api.Services
{
    public class GamePlayService : IGamePlayService
    {
        private readonly IGamePlayRepository _gamePlayRepository;
        private readonly IScheduleService _scheduleService;
        private readonly ITracer _tracer;

        public GamePlayService(
            ITracer tracer,
            IGamePlayRepository gamePlayRepository,
            IScheduleService scheduleService)
        {
            _tracer = tracer;
            _gamePlayRepository = gamePlayRepository;
            _scheduleService = scheduleService;
        }

        public async IAsyncEnumerable<GamePlays> GetGamePlaysAsync(
            PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetGamePlaysAsync));

            scope.LogStart(nameof(GetGamePlaysAsync));

            await foreach (var game in QueryForGameStats(queryParameter, cancellationToken))
            {
                if (game == null) continue;

                yield return game.ToGamePlays();
            }

            scope.LogEnd(nameof(GetGamePlaysAsync));
        }

        public async IAsyncEnumerable<GameDrives> GetGameDrivesAsync(
            PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetGameDrivesAsync));

            scope.LogStart(nameof(GetGameDrivesAsync));

            await foreach (var game in QueryForGameStats(queryParameter, cancellationToken))
            {
                if (game == null) continue;

                yield return game.ToGameDrives();
            }

            scope.LogEnd(nameof(GetGameDrivesAsync));
        }

        public async IAsyncEnumerable<GameScoringSummaries> GetGameScoringSummariesAsync(
            PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var scope = _tracer.BuildTrace(nameof(GetGameScoringSummariesAsync));

            scope.LogStart(nameof(GetGameScoringSummariesAsync));

            await foreach (var game in QueryForGameStats(queryParameter, cancellationToken))
            {
                if (game == null) continue;

                scope.Span.SetTag("Total rows", 1);

                yield return game.ToGameScoringSummaries();
            }

            scope.LogEnd(nameof(GetGameScoringSummariesAsync));
        }

        private async IAsyncEnumerable<ApiGamePlay> QueryForGameStats(
            PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var scope = _tracer.BuildTrace(nameof(QueryForGameStats));

            scope.LogStart(nameof(QueryForGameStats));

            var games = await GetGamesFromQueryOptions(queryParameter, cancellationToken);

            games = games.Where(game => game.Gameday < DateTime.UtcNow.Date).ToList();

            if (!games.Any())
                yield return null;

            var requests = games
                .OrderByDescending(game => game.Gameday)
                .Select(game => _gamePlayRepository.GetGamePlaysAsync(game, cancellationToken))
                .ToList();

            //Wait for all the requests to finish
            await Task.WhenAll(requests);

            //Get the responses
            foreach (var request in requests)
            {
                var pbp = await request;

                if (pbp.Game == null)
                {
                    continue;
                }

                yield return new ApiGamePlay(pbp);
            }

            scope.LogEnd(nameof(QueryForGameStats));
        }

        private async Task<IList<Game>> GetGamesFromQueryOptions(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var scheduleParams = queryParameter.ToScheduleQueryParameters();

            var allGames = await _scheduleService.GetGames(scheduleParams, cancellationToken);

            // Filter by the gameId provided by the client
            // ReSharper disable once InvertIf
            if (!string.IsNullOrWhiteSpace(queryParameter.GameId))
            {
                allGames.ToDictionary(x => x.GameId, x => x).TryGetValue(queryParameter.GameId, out var game);
                return new List<Game>() { game };
            }

            return allGames.ToList();
        }

        private int GetCurrentWeek(IEnumerable<Game> games)
        {
            var today = DateTime.UtcNow;
            var season = today.Month > 8 ? today.Year : today.Year - 1;

            return games
                .Where(x => x.Season == season && x.Gameday < today.Date)
                .Max(x => x.Week);
        }
    }
}