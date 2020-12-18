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

        public async Task<IEnumerable<Game>> GetGamesById(
            Dictionary<int, List<string>> gameIdsBySeason,
            CancellationToken cancellationToken)
        {
            var gamesBySeason = await GetAllGames(cancellationToken);

            var games = new List<Game>();
            foreach (var season in gameIdsBySeason.Keys)
            {
                games.AddRange(gamesBySeason[season].Where(x => gameIdsBySeason[season].Contains(x.GameId)));
            }

            return games;
        }

        public async Task<IEnumerable<Game>> GetGames(
            ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var gamesPerSeason = await GetAllGames(cancellationToken);

            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;
            var season = queryParameter.Season ?? currentSeason;
            var team = queryParameter.Team;
            var week = queryParameter.Week;

            var games = gamesPerSeason[season];

            if (!string.IsNullOrWhiteSpace(team))
                games = games.Where(x => x.HomeTeam == team || x.AwayTeam == team);
            
            if (week != null)
                games = games.Where(x => x.Week == week);

            return games;
        }

        private async Task<Dictionary<int, IEnumerable<Game>>> GetAllGames(CancellationToken cancellationToken)
        {
            return await _gameRepository.GetGamesAsync(cancellationToken);
        }
    }
}