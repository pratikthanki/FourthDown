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
    public class ScheduleService : IScheduleService
    {
        private readonly IGameRepository _gameRepository;
        private readonly DateTime Today = DateTime.UtcNow;

        public ScheduleService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<IEnumerable<Game>> GetGames(
            ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var gamesPerSeason = await GetAllGames(cancellationToken);

            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;
            var season = queryParameter.Season ?? currentSeason;

            var games = gamesPerSeason[season];

            if (!string.IsNullOrWhiteSpace(queryParameter.Team))
                games = games.Where(x => x.HomeTeam == queryParameter.Team || x.AwayTeam == queryParameter.Team);

            if (queryParameter.Week != null)
                games = games.Where(x => x.Week == queryParameter.Week);

            return games;
        }

        public async Task<Dictionary<int, IEnumerable<Game>>> GetAllGames(CancellationToken cancellationToken)
        {
            return await _gameRepository.GetGamesAsync(cancellationToken);
        }
    }
}