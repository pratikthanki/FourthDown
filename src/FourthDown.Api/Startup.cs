using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FourthDown.Api.Authentication;
using FourthDown.Api.Configuration;
using FourthDown.Api.HealthChecks;
using FourthDown.Api.Repositories;
using FourthDown.Api.Repositories.Csv;
using FourthDown.Api.Repositories.Json;
using FourthDown.Api.Services;
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
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using OpenTracing;
using OpenTracing.Util;
using Prometheus;

namespace FourthDown.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<AuthenticationOptions>(Configuration);

            services
                .AddSingleton<IGamePlayService, GamePlayService>()
                .AddSingleton<IScheduleService, ScheduleService>()
                .AddSingleton<ITeamRepository, JsonTeamRepository>()
                .AddSingleton<IGameRepository, CsvGameRepository>()
                .AddSingleton<IGamePlayRepository, JsonGamePlayRepository>()
                .AddSingleton<IPlayByPlayRepository, CsvPlayByPlayRepository>()
                .AddSingleton<IAuthClient, AuthClient>()
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

                    var tracer = new Tracer.Builder(serviceName)
                        .WithReporter(remoteReporter)
                        .WithLoggerFactory(loggerFactory)
                        .WithSampler(sampler)
                        .Build();

                    GlobalTracer.Register(tracer);

                    return tracer;
                });

            services.AddControllers();
            services.AddOpenTracing();
            services.AddResponseCaching();
            services.AddApplicationInsightsTelemetry();

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
                        Title = "Fourth Down API",
                        Description =
                            "The Fourth Down API is organised around the HTTP [REST](https://en.wikipedia.org/wiki/Representational_state_transfer) protocol. " +
                            "Our API has predictable resource-oriented URLs and will returns [JSON-encoded](http://www.json.org/) responses, " +
                            "and uses standard HTTP response codes, authentication, and verbs.\n" +
                            "The API can be used to get NFL schedule and team details, as well as the more sophisticated and " +
                            "different types of play-by-play game data. The API is designed with the primary goal of " +
                            "being language/tool agnostic, a shortfall in NFL data resources currently available.\n" +
                            "\nMost endpoints share the same set of base query parameters: `GameId`, `Season`, " +
                            "`Team` and `Week`.\n" +
                            "This API is documented in **OpenAPI format** and supported by a few " +
                            "[vendor extensions](https://github.com/Redocly/redoc/blob/master/docs/redoc-vendor-extensions.md).",
                        Extensions = new Dictionary<string, IOpenApiExtension>
                        {
                            {
                                "x-logo", new OpenApiObject
                                {
                                    {"url", new OpenApiString("https://bit.ly/37qIayZ")},
                                    {"backgroundColor", new OpenApiString("#007CBD")},
                                    {"altText", new OpenApiString("Fourth Down")}
                                }
                            }
                        },
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Pratik Thanki",
                            Email = "pratikthanki1@gmail.com",
                            Url = new Uri("http://pratikthanki.github.io/")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://choosealicense.com/licenses/mit/")
                        }
                    });

                    c.AddSecurityDefinition("apiKey",
                        new OpenApiSecurityScheme
                        {
                            Name = "X-API-KEY", In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey
                        });

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

            services
                .AddHealthChecks()
                .AddCheck<DataAccessHealthCheck>(
                    "Health check for access to data repository",
                    HealthStatus.Degraded,
                    new[] {"data"});

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                    options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                })
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                    ApiKeyAuthenticationOptions.DefaultScheme, options => { });
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
            
            app.UseSwagger();
            app.UseRouting();
            app.UseResponseCaching();
            app.UseHttpMetrics();

            app.UseAuthentication();
            app.UseAuthorization();
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

                endpoints.MapGet("/", async context =>
                {
                    var file = Path.Join(env.WebRootPath, "index.html");
                    await context.Response.WriteAsync(file);
                });

                endpoints.MapGet("/hello", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapMetrics();
            });
        }
    }
}