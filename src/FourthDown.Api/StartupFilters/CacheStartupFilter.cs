using System;
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
            return async builder =>
            {
                using (var scope = builder.ApplicationServices.CreateScope())
                {
                    var token = new CancellationToken();
                    
                    // https://github.com/nflverse/nflfastR-raw/blob/master/raw/2020/2020_06_BAL_PHI.json.gz?raw=true
                    // https://github.com/nflverse/nflfastR-raw/blob/upstream/raw/2020/2020_10_WAS_DET.json.gz?raw=true;

                    var gamesRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
                    _ = Task.Run(() => gamesRepository.TryPopulateCacheAsync(token), token);

                    var currentSeason = StringParser.GetCurrentSeason();
                    var games = await gamesRepository.GetGamesForSeason(currentSeason, token);

                    var gamePlaysRepository = scope.ServiceProvider.GetRequiredService<IGamePlayRepository>();
                    _ = Task.Run(() => gamePlaysRepository.TryPopulateCacheAsync(games, token), token);
                }

                next(builder);
            };
        }
    }
}