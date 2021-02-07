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

            if (!response.IsSuccessStatusCode)
                return new Dictionary<int, IEnumerable<Game>>();

            var games = ProcessGamesResponse(responseBody);

            return games
                .GroupBy(x => x.Season)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());
        }

        private static IEnumerable<Game> ProcessGamesResponse(string responseBody)
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

                games.Add(game);
            }

            return games;
        }
    }
}