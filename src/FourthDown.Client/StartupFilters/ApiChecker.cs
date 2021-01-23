using System;
using FourthDown.Client.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FourthDown.Client.StartupFilters
{
    public class ApiChecker : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                using (var scope = builder.ApplicationServices.CreateScope())
                {
                    var baseUrl = scope.ServiceProvider.GetRequiredService<IOptions<ApiOptions>>().Value.ApiBaseUrl;

                    if (string.IsNullOrEmpty(baseUrl))
                        throw new Exception("Field not provided: 'ApiBaseUrl'.");
                }

                next(builder);
            };
        }
    }
}
