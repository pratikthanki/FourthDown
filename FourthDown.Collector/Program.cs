﻿using System.Threading.Tasks;
using FourthDown.Collector.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FourthDown.Collector
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<DataCollector>();
                    services.AddSingleton<IPlayByPlayRepository, CsvRepository>();
                });

            await builder.RunConsoleAsync();
        }
    }
}