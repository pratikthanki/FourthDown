using System.Collections.Generic;
using System.IO;
using System.Linq;
using FourthDown.UI.Models;
using FourthDown.UI.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace FourthDown.UI.Services
{
    public class JsonPlayByPlayService : IPlayByPlayService
    {
        private readonly IPlayByPlayRepository _playByPlayRepository;
        public JsonPlayByPlayService(
            IWebHostEnvironment webHostEnvironment, 
            IPlayByPlayRepository playByPlayRepository)
        {
            WebHostEnvironment = webHostEnvironment;
            _playByPlayRepository = playByPlayRepository;
        }

        private IWebHostEnvironment WebHostEnvironment { get; set; }

        private string JsonFileName => 
            Path.Combine(WebHostEnvironment.WebRootPath, "data", "play_by_play_2020.json");

        public IEnumerable<PlayByPlay> GetPlayByPlays()
        {
            return _playByPlayRepository
                .GetGamePlays(JsonFileName)
                .Where(x => x.GameId == "2020_01_DAL_LA");
        }

        public IEnumerable<WinProbability> GetGameWinProbability()
        {
            var game = GetPlayByPlays();

            return game
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