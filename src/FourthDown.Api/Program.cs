using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Prometheus.DotNetRuntime;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace FourthDown.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;

#if !DEBUG
            new MetricServer(8000).Start();
#endif

            using var collector = DotNetRuntimeStatsBuilder.Default().StartCollecting();

            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();
                    configuration.AddEnvironmentVariables();

                    var secretsDirectory = Environment.GetEnvironmentVariable("SECRETS_DIRECTORY");
                    if (secretsDirectory != null)
                    {
                        configuration.AddKeyPerFile(secretsDirectory, true);
                    }

                    configuration
                        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                        .AddJsonFile("appsettings.json");
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseSerilog((context, services, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration);
                    config.Enrich.FromLogContext();
                    config.Enrich.With(new SourceContextRemover());
                    config.WriteTo.Console(new CompactJsonFormatter());
                });
        }

        private class SourceContextRemover : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.RemovePropertyIfPresent("SourceContext");
            }
        }
    }
}