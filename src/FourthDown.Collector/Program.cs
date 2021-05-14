using FourthDown.Collector.Options;
using FourthDown.Collector.Repositories;
using FourthDown.Collector.Services;
using FourthDown.Shared.Repositories;
using FourthDown.Shared.Repositories.Csv;
using FourthDown.Shared.Repositories.Json;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Util;

namespace FourthDown.Collector
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddCommandLine(args);
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<DatabaseOptions>(hostContext.Configuration)
                        .AddSingleton<ITracer>(serviceProvider =>
                        {
                            // This will log to a default localhost installation of Jaeger.
                            var tracer = new Tracer.Builder("Collector Service")
                                .WithSampler(new ConstSampler(true))
                                .Build();

                            // Allows code that can't use DI to also access the tracer.
                            GlobalTracer.Register(tracer);

                            return tracer;
                        })
                        .AddSingleton<IWriter, SqlWriter>()
                        .AddSingleton<IGameRepository, CsvGameRepository>()
                        .AddSingleton<ISqlGameRepository, SqlGameRepository>()
                        .AddSingleton<IGamePlayRepository, JsonGamePlayRepository>()
                        .AddSingleton<ICollectorManager, CollectorManager>()
                        .AddHostedService<CollectorService>();
                })
                .UseConsoleLifetime();
        }
    }
}