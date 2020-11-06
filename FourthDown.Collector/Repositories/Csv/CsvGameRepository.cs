using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;
using Microsoft.Extensions.Options;
using Options = FourthDown.Collector.Configuration.Options;

namespace FourthDown.Collector.Repositories.Csv
{
    public class CsvGameRepository : IGameRepository
    {
        private readonly Options _options;
        private readonly int FirstSeason = 1999;
        private const string url = @"https://raw.githubusercontent.com/leesharpe/nfldata/master/data/games.csv";

        public CsvGameRepository(IOptions<Options> options)
        {
            _options = options.Value;
        }

        public async Task<IEnumerable<Game>> GetGames(int season, CancellationToken cancellationToken)
        {
            var currentSeason = CurrentSeason();
            var seasons = _options.BackLoad
                ? Enumerable.Range(FirstSeason, currentSeason - FirstSeason)
                : new List<int>() {currentSeason};

            var timeout = TimeSpan.FromSeconds(10);
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(httpClientHandler) {Timeout = timeout};

            var response = await httpClient.GetAsync(url, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync();

            var games = responseBody.Split("\n").Skip(1)
                .Select(x => new Game(x))
                .Where(x => seasons.Contains(x.Season))
                .ToList();

            return games;
        }

        private static int CurrentSeason()
        {
            var today = DateTime.UtcNow;
            return today.Month > 8 ? today.Year : today.Year - 1;
        }
    }
}
