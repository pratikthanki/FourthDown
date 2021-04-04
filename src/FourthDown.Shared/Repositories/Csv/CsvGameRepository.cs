using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace FourthDown.Shared.Repositories.Csv
{
    public class CsvGameRepository : IGameRepository
    {
        private static ITracer _tracer;
        private static ILogger<CsvGameRepository> _logger;

        public CsvGameRepository(
            ITracer tracer,
            ILogger<CsvGameRepository> logger)
        {
            _tracer = tracer;
            _logger = logger;
        }

        public async Task<Dictionary<int, IEnumerable<Game>>> GetGamesAsync(CancellationToken cancellationToken)
        {
            var url = RepositoryEndpoints.GamesEndpoint;
            var response = await RequestHelper.GetRequestResponse(url, cancellationToken);
            
            _logger.LogInformation($"Fetching data. Url: {url}; Status: {response.StatusCode}");

            var responseBody = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode
                ? ProcessGamesResponse(responseBody)
                : new Dictionary<int, IEnumerable<Game>>();
        }

        private static Dictionary<int, IEnumerable<Game>> ProcessGamesResponse(string responseBody)
        {
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
                    Espn = StringParser.ToIntDefaultZero(fields[16]),
                    AwayRest = StringParser.ToInt(fields[17]),
                    HomeRest = StringParser.ToInt(fields[18]),
                    AwayMoneyline = StringParser.ToDoubleDefaultZero(fields[19]),
                    HomeMoneyline = StringParser.ToDoubleDefaultZero(fields[20]),
                    SpreadLine = StringParser.ToDoubleDefaultZero(fields[21]),
                    AwaySpreadOdds = StringParser.ToDoubleDefaultZero(fields[22]),
                    HomeSpreadOdds = StringParser.ToDoubleDefaultZero(fields[23]),
                    TotalLine = StringParser.ToDoubleDefaultZero(fields[24]),
                    UnderOdds = StringParser.ToDoubleDefaultZero(fields[25]),
                    OverOdds = StringParser.ToDoubleDefaultZero(fields[26]),
                    DivGame = StringParser.ToBool(fields[27]),
                    Roof = fields[28],
                    Surface = fields[29],
                    Temp = StringParser.ToIntDefaultZero(fields[30]),
                    Wind = StringParser.ToIntDefaultZero(fields[31]),
                    AwayQbId = fields[32],
                    HomeQbId = fields[33],
                    AwaQbName = fields[34],
                    HomeQbName = fields[35],
                    AwayCoach = fields[36],
                    HomeCoach = fields[37],
                    Referee = fields[38],
                    StadiumId = fields[39],
                    Stadium = fields[40]
                };

                games.Add(game);
            }

            return games
                .GroupBy(x => x.Season)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());
        }
    }
}