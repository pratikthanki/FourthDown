using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FourthDown.Database
{
    class Program
    {
        private const string EnviornmentPrefix = "FD_DATABASE_";

        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    config.AddCommandLine(args);
                    config.AddJsonFile("appsettings.json");
                    config.AddEnvironmentVariables(EnviornmentPrefix);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddHostedService<DeploymentService>()
                        .Configure<DatabaseOptions>(hostContext.Configuration);
                })
                .UseConsoleLifetime();
        }
    }
}