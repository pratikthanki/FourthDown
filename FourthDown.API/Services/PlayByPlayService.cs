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

        private readonly string GameId = "2020_09_PIT_DAL";        
        private readonly int Season = 2020;

        public PlayByPlayService(
            IPlayByPlayRepository pbpRepository,
            IScheduleService scheduleService)
        {
            _pbpRepository = pbpRepository;
            _scheduleService = scheduleService;
        }

        public async Task<IEnumerable<GameRaw>> GetPlayByPlays(
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

            if (games == null)
            {
                return null;
            }

            return await _pbpRepository.GetGamePlays(GameId, Season, cancellationToken);
        }

        public Task<IEnumerable<WinProbability>> GetGameWinProbability(PlayByPlayQueryParameter queryParameter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
