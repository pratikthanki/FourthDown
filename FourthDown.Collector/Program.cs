using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FourthDown.Collector.Configuration;
using FourthDown.Collector.Repositories;
using FourthDown.Collector.Repositories.Csv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FourthDown.Collector
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<Options>(hostContext.Configuration);
                    services.AddHostedService<DataCollector>();
                    services.AddSingleton<IPlayByPlayRepository, CsvPlayByPlayRepository>();
                    services.AddSingleton<IGameRepository, CsvGameRepository>();
                })
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();

                    var env = hostingContext.HostingEnvironment;

                    configuration
                        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                        .AddJsonFile("appsettings.json");

                    var configurationRoot = configuration.Build();
                });

            await builder.RunConsoleAsync();
        }
    }
}