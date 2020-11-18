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
        private readonly IGameRepository _gameRepository;
        private readonly DateTime Today = DateTime.UtcNow;
        private readonly int _currentSeason;

        public ScheduleService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            _currentSeason = CurrentSeason();
        }

        private int CurrentSeason() => Today.Month > 8 ? Today.Year : Today.Year - 1;

        private int GetCurrentWeek(IEnumerable<Game> games)
        {
            return games
                .Where(x => x.Season == _currentSeason && x.Gameday <= Today.Date)
                .Max(x => x.Week);
        }

        private async Task<Dictionary<int, List<Game>>> GetAllGames(CancellationToken cancellationToken)
        {
            return await _gameRepository.GetGames(cancellationToken);
        }

        public async Task<IEnumerable<Game>> GetGameById(
            string gameId,
            CancellationToken cancellationToken)
        {
            var gamesPerSeason = await GetAllGames(cancellationToken);
            var season = gameId.ToString().Substring(0, 4);

            return gamesPerSeason[StringParser.ToInt(season)].Where(x => x.GameId == gameId);
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

            if (week == null)
            {
                if (season == _currentSeason)
                    games = games.Where(x => x.Week == currentWeek).ToList();
            }
            else
            {
                games = games.Where(x => x.Week == week).ToList();
            }

            if (!string.IsNullOrWhiteSpace(team))
                games = games.Where(x => x.HomeTeam == team || x.AwayTeam == team).ToList();

            return games;
        }
    }
}
