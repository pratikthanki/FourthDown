using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Repositories;

namespace FourthDown.Api.Services
{
    public class GamePlayService : IGamePlayService
    {
        private readonly IGamePlayRepository _gamePlayRepository;
        private readonly IScheduleService _scheduleService;

        public GamePlayService(
            IGamePlayRepository gamePlayRepository,
            IScheduleService scheduleService)
        {
            _gamePlayRepository = gamePlayRepository;
            _scheduleService = scheduleService;
        }

        public async Task<IEnumerable<GamePlays>> GetGamePlays(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var gameDetails = await QueryForGameStats(queryParameter, cancellationToken);

            return gameDetails.Select(x => x.ParseToGamePlays());
        }

        public async Task<IEnumerable<GameDrives>> GetGameDrives(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var gameDetails = await QueryForGameStats(queryParameter, cancellationToken);

            return gameDetails.Select(x => x.ParseToGameDrives());
        }

        public async Task<IEnumerable<GameScoringSummaries>> GetGameScoringSummaries(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var gameDetails = await QueryForGameStats(queryParameter, cancellationToken);

            return gameDetails.Select(x => x.ParseToGameScoringSummaries());
        }

        private async Task<IEnumerable<Game>> GetGamesFromQueryOptions(
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

                games = await _scheduleService.GetGames(scheduleParams, cancellationToken);
            }
            else
            {
                games = await _scheduleService.GetGameById(queryParameter.GameId, cancellationToken);
            }

            return games.Where(game => game.Gameday < DateTime.UtcNow.Date).ToList();
        }

        private async Task<IEnumerable<GameDetailsFormatted>> QueryForGameStats(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var games = await GetGamesFromQueryOptions(queryParameter, cancellationToken);

            if (games == null || !games.Any()) return null;

            var gamePlays = new List<GameDetailsFormatted>();

            foreach (var game in games)
            {
                var pbp = await _gamePlayRepository.GetGamePlaysAsync(game.GameId, game.Season, cancellationToken);
                gamePlays.Add(new GameDetailsFormatted(pbp));
            }

            return gamePlays.ToList();
        }
    }
}