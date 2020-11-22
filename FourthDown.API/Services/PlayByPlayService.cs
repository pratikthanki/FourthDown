using System;
using System.Collections.Generic;
using System.Linq;
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

        public PlayByPlayService(
            IPlayByPlayRepository pbpRepository,
            IScheduleService scheduleService)
        {
            _pbpRepository = pbpRepository;
            _scheduleService = scheduleService;
        }

        public async Task<List<PlayByPlay>> GetGamePlayByPlays(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var games = await GetGamesFromQueryOptions(queryParameter, cancellationToken);

            if (games == null || !games.Any())
            {
                return null;
            }

            var gamePlayByPlays = new List<PlayByPlay>();

            foreach (var game in games)
            {
                var pbp = await _pbpRepository.GetGamePlays(game.GameId, game.Season, cancellationToken);
                gamePlayByPlays.Add(new PlayByPlay(pbp));
            }

            return gamePlayByPlays;
        }

        private async Task<IEnumerable<Game>> GetGamesFromQueryOptions(
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

            return games.Where(game => game.Gameday < DateTime.UtcNow.Date).ToList();
        }

        public Task<IEnumerable<WinProbability>> GetGameWinProbability(
            PlayByPlayQueryParameter queryParameter, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
