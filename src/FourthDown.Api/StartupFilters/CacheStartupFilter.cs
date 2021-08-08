using System;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Repositories;
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
                    var gameRepository = scope.ServiceProvider.GetRequiredService<IGameRepository>();
                    _ = Task.Run(() => gameRepository.TryPopulateCacheAsync(CancellationToken.None));
                }

                next(builder);
            };
        }
    }
}