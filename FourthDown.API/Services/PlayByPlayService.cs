using System.Collections.Generic;
using System.IO;
using System.Linq;
using FourthDown.Api.Models;
using FourthDown.Api.Repositories;
using Microsoft.AspNetCore.Hosting;
using PlayByPlay = FourthDown.Api.Models.PlayByPlay;

namespace FourthDown.Api.Services
{
    public class PlayByPlayService : IPlayByPlayService
    {
        private readonly IPlayByPlayRepository _pbpRepository;

        public PlayByPlayService(
            IWebHostEnvironment webHostEnvironment,
            IPlayByPlayRepository pbpRepository)
        {
            WebHostEnvironment = webHostEnvironment;
            _pbpRepository = pbpRepository;
        }

        private IWebHostEnvironment WebHostEnvironment { get; set; }

        private string JsonFileName =>
            Path.Combine(WebHostEnvironment.WebRootPath, "data", "play_by_play_2020.json");

        public IEnumerable<PlayByPlay> GetPlayByPlays()
        {
            return _pbpRepository
                .GetGamePlays(JsonFileName);
        }

        public IEnumerable<PlayByPlay> GetGamePlayByPlays(int gameId)
        {
            // var gameId = 2020091312;
            return _pbpRepository
                .GetGamePlays(JsonFileName)
                .Where(x => x.OldGameId == gameId);
        }

        public IEnumerable<WinProbability> GetGameWinProbability()
        {
            var game = GetPlayByPlays();

            return game
                .Where(x => x.GameSecondsRemaining % 10 == 0)
                .Select(x => new WinProbability()
                {
                    PlayId = x.PlayId,
                    GameId = x.GameId,
                    Qtr = x.Qtr,
                    QuarterEnd = x.QuarterEnd,
                    HomeTeam = x.HomeTeam,
                    AwayTeam = x.AwayTeam,
                    HomeWp = x.HomeWp,
                    AwayWp = x.AwayWp,
                    TotalHomeScore = x.TotalHomeScore,
                    TotalAwayScore = x.TotalAwayScore
                })
                .Where(x => x.HomeWp != null && x.AwayWp != null);
        }
    }
}