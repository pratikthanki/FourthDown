using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;
using FourthDown.Api.Repositories;
using FourthDown.Api.Utilities;

namespace FourthDown.Api.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly int _currentSeason;
        private readonly IGameRepository _gameRepository;
        private readonly DateTime Today = DateTime.UtcNow;

        public ScheduleService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            _currentSeason = CurrentSeason();
        }

        public async Task<IEnumerable<Game>> GetGamesById(
            Dictionary<int, List<string>> gameIdsBySeason,
            CancellationToken cancellationToken)
        {
            var gamesBySeason = await GetAllGames(cancellationToken);

            var games = new List<Game>();
            foreach (var season in gameIdsBySeason.Keys)
            {
                games.AddRange(gamesBySeason[season].ToList().Where(x => gameIdsBySeason[season].Contains(x.GameId)));
            }

            return games;
        }

        public async Task<IEnumerable<Game>> GetGames(
            ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var gamesPerSeason = await GetAllGames(cancellationToken);

            var currentWeek = GetCurrentWeek(gamesPerSeason[_currentSeason]);

            var team = queryParameter.Team;
            var season = queryParameter.Season ?? _currentSeason;
            var week = queryParameter.Week;

            var games = gamesPerSeason[season];

            if (queryParameter.IsNull())
                games = games.Where(x => x.Week == currentWeek).ToList();

            if (week != null)
                games = games.Where(x => x.Week == week).ToList();

            if (!string.IsNullOrWhiteSpace(team))
                games = games.Where(x => x.HomeTeam == team || x.AwayTeam == team).ToList();

            return games;
        }

        private int CurrentSeason()
        {
            return Today.Month > 8 ? Today.Year : Today.Year - 1;
        }

        private int GetCurrentWeek(IEnumerable<Game> games)
        {
            return games
                .Where(x => x.Season == _currentSeason && x.Gameday <= Today.Date)
                .Max(x => x.Week);
        }

        private async Task<Dictionary<int, IEnumerable<Game>>> GetAllGames(CancellationToken cancellationToken)
        {
            return await _gameRepository.GetGamesAsync(cancellationToken);
        }
    }
}