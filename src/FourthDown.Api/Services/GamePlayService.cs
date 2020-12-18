using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Extensions;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Repositories;
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
            _tracer.CreateChildTrace(nameof(GetGamePlaysAsync));

            await foreach (var game in QueryForGameStats(queryParameter, cancellationToken))
            {
                if (game == null) continue;

                yield return game.ToGamePlays();
            }
        }

        public async IAsyncEnumerable<GameDrives> GetGameDrivesAsync(
            PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            _tracer.CreateChildTrace(nameof(GetGameDrivesAsync));

            await foreach (var game in QueryForGameStats(queryParameter, cancellationToken))
            {
                if (game == null) continue;

                yield return game.ToGameDrives();
            }
        }

        public async IAsyncEnumerable<GameScoringSummaries> GetGameScoringSummariesAsync(
            PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            _tracer.CreateChildTrace(nameof(GetGameScoringSummariesAsync));

            await foreach (var game in QueryForGameStats(queryParameter, cancellationToken))
            {
                if (game == null) continue;

                yield return game.ToGameScoringSummaries();
            }
        }

        private async Task<IList<Game>> GetGamesFromQueryOptions(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            IEnumerable<Game> games;
            if (string.IsNullOrWhiteSpace(queryParameter.GameId))
            {
                var scheduleParams = new ScheduleQueryParameter
                {
                    Week = queryParameter.Week,
                    Season = queryParameter.Season,
                    Team = queryParameter.Team
                };

                games = (await _scheduleService.GetGames(scheduleParams, cancellationToken)).ToList();
                ;
                var week = queryParameter.Week ?? GetLatestWeek(games);
                games = games.Where(x => x.Week == week);
            }
            else
            {
                var gameIdsBySeason = queryParameter.GetGameIdsBySeason();
                games = await _scheduleService.GetGamesById(gameIdsBySeason, cancellationToken);
            }

            return games.ToList();
        }

        private async IAsyncEnumerable<ApiGamePlay> QueryForGameStats(
            PlayByPlayQueryParameter queryParameter,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            _tracer.CreateChildTrace(nameof(QueryForGameStats));

            var games = await GetGamesFromQueryOptions(queryParameter, cancellationToken);

            if (games.Any(game => game.Gameday > DateTime.UtcNow.Date))
                yield return null;

            games = games.Where(game => game.Gameday < DateTime.UtcNow.Date).ToList();

            if (!games.Any())
                yield return null;

            var requests = games
                .Select(game => _gamePlayRepository.GetGamePlaysAsync(game.GameId, game.Season, cancellationToken))
                .ToList();

            //Wait for all the requests to finish
            await Task.WhenAll(requests);

            //Get the responses
            var responses = requests.Select(task => task.Result);

            foreach (var pbp in responses)
            {
                if (pbp.Id == null || !pbp.Id.Any())
                    continue;

                yield return new ApiGamePlay(pbp);
            }
        }

        private int GetLatestWeek(IEnumerable<Game> games)
        {
            var Today = DateTime.UtcNow;
            var season = Today.Month > 8 ? Today.Year : Today.Year - 1;

            return games
                .Where(x => x.Season == season && x.Gameday < Today.Date)
                .Max(x => x.Week);
        }
    }
}