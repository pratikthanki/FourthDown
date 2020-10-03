using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FourthDown.UI.Models;
using Microsoft.AspNetCore.Hosting;

namespace FourthDown.UI.Services
{
    public class JsonPlayByPlayService : IPlayByPlayService
    {
        public JsonPlayByPlayService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        private IWebHostEnvironment WebHostEnvironment { get; set; }

        private string JsonFileName => 
            Path.Combine(WebHostEnvironment.WebRootPath, "data", "play_by_play_2020.json");

        private IEnumerable<PlayByPlay> GetGames()
        {
            using var jsonFileReader = File.OpenText(JsonFileName);

            var plays = JsonSerializer.Deserialize<PlayByPlay[]>(jsonFileReader.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            return plays.Where(x => x.GameId == "2020_01_DAL_LA");
        }

        public IEnumerable<PlayByPlay> GetPlayByPlays()
        {
            return GetGames();
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