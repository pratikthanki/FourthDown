using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using FourthDown.Api.HealthChecks;
using FourthDown.Shared.Repositories;
using FourthDown.Shared.Repositories.Csv;
using FourthDown.Shared.Repositories.Json;
using FourthDown.Api.Services;
using FourthDown.Api.StartupFilters;
using FourthDown.Shared.Utilities;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using OpenTracing;
using Prometheus;

namespace FourthDown.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services
                .AddTransient<IStartupFilter, CacheStartupFilter>()
                .AddSingleton<IGamePlayService, GamePlayService>()
                .AddSingleton<INflfastrService, NflfastrService>()
                .AddSingleton<IScheduleService, ScheduleService>()
                .AddSingleton<ITeamRepository, JsonTeamRepository>()
                .AddSingleton<IGameRepository, CsvGameRepository>()
                .AddSingleton<IGamePlayRepository, JsonGamePlayRepository>()
                .AddSingleton<IPlayByPlayRepository, CsvPlayByPlayRepository>()
                .AddSingleton<IRequestHelper, RequestHelper>();

            services
                .AddSingleton<ITracer>(serviceProvider =>
                {
                    var serviceName = serviceProvider.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                    ISampler sampler = new ConstSampler(true);

                    var host = Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST") ?? "localhost";

                    var remoteReporter = new RemoteReporter.Builder()
                        .WithLoggerFactory(loggerFactory)
                        .WithSender(new UdpSender(host, 6831, 0))
                        .Build();

                    return new Tracer.Builder(serviceName)
                        .WithReporter(remoteReporter)
                        .WithLoggerFactory(loggerFactory)
                        .WithSampler(sampler)
                        .Build();
                });

            services.AddOpenTracing();
            services.AddResponseCaching();
            services.AddCors();

            services.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "FourthDown API",
                        Description =
                            "The FourthDown API is organised around the HTTP [REST](https://en.wikipedia.org/wiki/Representational_state_transfer) protocol. " +
                            "Our API has predictable resource-oriented URLs and will returns [JSON-encoded](https://www.json.org/) responses, " +
                            "and uses standard HTTP response codes, authentication, and verbs.\n" +
                            "The API can be used to get NFL schedule and team details, as well as the more sophisticated and " +
                            "different types of play-by-play game data. The API is designed with the primary goal of " +
                            "being language/tool agnostic, a shortfall in NFL data resources currently available.\n" +
                            "\nMost endpoints share the same set of base query parameters: `GameId`, `Season`, " +
                            "`Team` and `Week`.\n" +
                            "This API is documented in **OpenAPI format** and supported by a few " +
                            "[vendor extensions](https://github.com/Redocly/redoc/blob/master/docs/redoc-vendor-extensions.md).",
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Pratik Thanki",
                            Email = "pratikthanki1@gmail.com",
                            Url = new Uri("https://pratikthanki.github.io/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://choosealicense.com/licenses/mit/")
                        }
                    });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

            services
                .AddHealthChecks()
                .AddCheck<DataAccessHealthCheck>(
                    "Health check for access to data repository", HealthStatus.Degraded, new[] { "data" })
                .ForwardToPrometheus();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:3000"));

            app.UseSwagger();
            app.UseRouting();
            app.UseResponseCaching();
            app.UseHttpMetrics(options =>
            {
                options.ReduceStatusCodeCardinality();
                options.AddCustomLabel("host", context => context.Request.Host.Host);
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    AllowCachingResponses = false,
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                });

                endpoints.MapMetrics();
            });
        }
    }
}