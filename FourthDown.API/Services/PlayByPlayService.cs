using System;
using System.Collections.Generic;
using System.Threading;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using System.Threading.Tasks;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public class PlayByPlayService : IPlayByPlayService
    {
        private readonly IPlayByPlayRepository _pbpRepository;
        private readonly IScheduleService _scheduleService;

        private const string GameId = "2020_09_PIT_DAL";
        private const int Season = 2020;

        public PlayByPlayService(
            IPlayByPlayRepository pbpRepository,
            IScheduleService scheduleService)
        {
            _pbpRepository = pbpRepository;
            _scheduleService = scheduleService;
        }

        public async Task<List<GameDetail>> GetGamePlayByPlays(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var games = await GetGamesFromQuery(queryParameter, cancellationToken);

            if (games == null)
            {
                return null;
            }

            var gamePlayByPlays = new List<GameDetail>();

            foreach (var game in games)
            {
                var pbp = await _pbpRepository.GetGamePlays(game.GameId, game.Season, cancellationToken);
                gamePlayByPlays.Add(pbp);
            }

            return gamePlayByPlays;
        }

        private async Task<IEnumerable<Game>> GetGamesFromQuery(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            IEnumerable<Game> games;
            if (string.IsNullOrWhiteSpace(queryParameter.GameId))
            {
                var scheduleParams = new ScheduleQueryParameter()
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

            return games;
        }

        public Task<IEnumerable<WinProbability>> GetGameWinProbability(
            PlayByPlayQueryParameter queryParameter, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
