using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Repositories;
using FourthDown.Shared.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FourthDown.Api.StartupFilters
{
    public class CacheStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                using (var scope = builder.ApplicationServices.CreateScope())
                {
                    var token = new CancellationToken();
                    
                    var gamesRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
                    _ = Task.Run(() => gamesRepository.TryPopulateCacheAsync(token), token);

                    var currentSeason = StringParser.GetCurrentSeason();
                    var firstGame = gamesRepository
                        .GetGamesForSeason(currentSeason, token)
                        .Result.ToList()
                        .Where(game => game.Season == currentSeason && game.Week == 1)
                        .Select(x => x.Gameday)
                        .Min();

                    var seasonGamesToFetch = firstGame < DateTime.UtcNow ? currentSeason : currentSeason - 1;
                    var gamesToCache = gamesRepository
                        .GetGamesForSeason(seasonGamesToFetch, token)
                        .Result
                        .ToList();

                    var gamePlaysRepository = scope.ServiceProvider.GetRequiredService<IGamePlayRepository>();
                    _ = Task.Run(() => gamePlaysRepository.TryPopulateCacheAsync(gamesToCache, token), token);
                }

                next(builder);
            };
        }
    }
}