using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Collector.Repositories.Csv
{
    public class CsvGameRepository : IGameRepository
    {
        public async Task<IEnumerable<Game>> GetGames(int season, CancellationToken cancellationToken)
        {
            var currentSeason = CurrentSeason();
            const string url = @"https://raw.githubusercontent.com/leesharpe/nfldata/master/data/games.csv";
            var timeout = TimeSpan.FromSeconds(10);

            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(httpClientHandler) {Timeout = timeout};
            var response = await httpClient.GetAsync(url, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync();
            var games = responseBody.Split("\n").Skip(1);
            
            return games.Select(x => new Game(x));
        }

        private static int CurrentSeason()
        {
            var today = DateTime.UtcNow;
            return today.Month > 8 ? today.Year : today.Year - 1;
        }
    }
}
