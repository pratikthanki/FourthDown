using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Collector.Configuration;
using FourthDown.Collector.Utilities;
using FourthDown.Shared.Models;
using Microsoft.Extensions.Options;

namespace FourthDown.Collector.Repositories.Csv
{
    public class CsvGameRepository : IGameRepository
    {
        private readonly ReadSettings _readSettings;
        private const int FirstSeason = 1999;
        private readonly DateTime Today = DateTime.UtcNow;

        private const string GithubHost = @"https://raw.githubusercontent.com";
        private readonly string url = $@"{GithubHost}/leesharpe/nfldata/master/data/games.csv";

        public CsvGameRepository(IOptions<ReadSettings> options)
        {
            _readSettings = options.Value;
        }

        private int CurrentSeason() => Today.Month > 8 ? Today.Year : Today.Year - 1;

        private async Task<List<Game>> GetImportedGames()
        {
            var path = @"../../../Data/ImportedGames.json";
            var absolutePath = StringParser.GetAbsolutePath(path);

            await using var fs = File.OpenRead(absolutePath);
            var importedGames = await JsonSerializer.DeserializeAsync<List<Game>>(fs);
            
            return importedGames;
        }

        public async Task<IEnumerable<Game>> GetGames(int season, CancellationToken cancellationToken)
        {
            var timeout = TimeSpan.FromSeconds(10);
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(httpClientHandler) {Timeout = timeout};
            var response = await httpClient.GetAsync(url, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync();

            return await ProcessGamesResponse(responseBody);
        }

        private async Task<List<Game>> ProcessGamesResponse(string responseBody)
        {
            var importedGames = await GetImportedGames();
            var importedGameIds = importedGames.Select(x => x.GameId).ToList();

            var currentSeason = CurrentSeason();
            var seasons = _readSettings.BackLoad
                ? Enumerable.Range(FirstSeason, currentSeason - FirstSeason).ToList()
                : new List<int>() {currentSeason};

            var csvResponse = responseBody
                .Split("\n")
                .Skip(1)
                .Select(x => x.Split(","));

            var games = new List<Game>();
            foreach (var fields in csvResponse)
            {
                if (fields.All(x => x == ""))
                    continue;

                var game = new Game
                {
                    GameId = fields[0],
                    Season = StringParser.ToInt(fields[1]),
                    GameType = fields[2],
                    Week = StringParser.ToInt(fields[3]),
                    Gameday = StringParser.ToDateTime(fields[4], "yyyy-MM-dd"),
                    Weekday = fields[5],
                    Gametime = fields[6],
                    AwayTeam = fields[7],
                    AwayScore = StringParser.ToIntDefaultZero(fields[8]),
                    HomeTeam = fields[9],
                    HomeScore = StringParser.ToIntDefaultZero(fields[10]),
                    Location = fields[11],
                    Result = StringParser.ToIntDefaultZero(fields[12]),
                    Total = StringParser.ToIntDefaultZero(fields[13]),
                    Overtime = StringParser.ToBool(fields[14]),
                    OldGameId = fields[15],
                    AwayRest = StringParser.ToInt(fields[16]),
                    HomeRest = StringParser.ToInt(fields[17]),
                    AwayMoneyline = StringParser.ToDoubleDefaultZero(fields[18]),
                    HomeMoneyline = StringParser.ToDoubleDefaultZero(fields[19]),
                    SpreadLine = StringParser.ToDoubleDefaultZero(fields[20]),
                    AwaySpreadOdds = StringParser.ToDoubleDefaultZero(fields[21]),
                    HomeSpreadOdds = StringParser.ToDoubleDefaultZero(fields[22]),
                    TotalLine = StringParser.ToDoubleDefaultZero(fields[23]),
                    UnderOdds = StringParser.ToDoubleDefaultZero(fields[24]),
                    OverOdds = StringParser.ToDoubleDefaultZero(fields[25]),
                    DivGame = StringParser.ToBool(fields[26]),
                    Roof = fields[27],
                    Surface = fields[28],
                    Temp = StringParser.ToIntDefaultZero(fields[29]),
                    Wind = StringParser.ToIntDefaultZero(fields[30]),
                    AwayCoach = fields[31],
                    HomeCoach = fields[32],
                    Referee = fields[33],
                    StadiumId = fields[34],
                    Stadium = fields[35]
                };

                if (seasons.Contains(game.Season) &&
                    game.Gameday < Today.Date &&
                    !importedGameIds.Contains(game.GameId))
                {
                    games.Add(game);
                }
            }

            return games;
        }
    }
}
