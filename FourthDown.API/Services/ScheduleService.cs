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
        private readonly int _currentSeason;

        public ScheduleService(
            IGameRepository gameRepository)
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

        public async Task<IEnumerable<Game>> GetGames(
            ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken)
        {
            var games = await _gameRepository.GetGames(cancellationToken);

            var currentWeek = GetCurrentWeek(games);

            var week = queryParameter.Week ?? currentWeek;
            var season = queryParameter.Season ?? _currentSeason;
            var team = queryParameter.Team;


            return games.Where(x => x.Season == _currentSeason && x.Week == week);
        }
    }
}