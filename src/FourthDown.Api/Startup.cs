using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FourthDown.Api.Authentication;
using FourthDown.Api.Configuration;
using FourthDown.Api.HealthChecks;
using FourthDown.Api.Monitoring;
using FourthDown.Api.Repositories;
using FourthDown.Api.Repositories.Csv;
using FourthDown.Api.Repositories.Json;
using FourthDown.Api.Services;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
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
using Microsoft.Extensions.Options;
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
                .Configure<AuthenticationOptions>(Configuration)
                .Configure<TracerOptions>(Configuration.GetSection("tracer"));

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
                    var tracerOptions = serviceProvider.GetRequiredService<IOptions<TracerOptions>>();

                    var serviceName = tracerOptions.Value?.ServiceName ?? Assembly.GetEntryAssembly()?.GetName().Name;
                    var mode = tracerOptions.Value?.Mode ?? TracerMode.Udp;

                    var endpoint = tracerOptions.Value?.HttpEndPoint ?? "http://localhost:14268/api/traces";
                    var udpEndpoint = tracerOptions.Value?.UdpEndPoint?.Host ?? "localhost";
                    var port = tracerOptions.Value?.UdpEndPoint?.Port ?? 6831;

                    var sender = mode == TracerMode.Http
                        ? (ISender) new HttpSender(endpoint)
                        : new UdpSender(udpEndpoint, port, 0);

                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                    ISampler sampler = new ConstSampler(true);

                    var reporter = new RemoteReporter.Builder()
                        .WithLoggerFactory(loggerFactory)
                        .WithSender(sender)
                        .Build();

                    var tracer = new Tracer.Builder(serviceName)
                        .WithReporter(reporter)
                        .WithLoggerFactory(loggerFactory)
                        .WithSampler(sampler)
                        .Build();

                    GlobalTracer.Register(tracer);

                    return tracer;
                });

            services.AddControllers();
            services.AddOpenTracing();

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
                            "The Fourth Down API is an HTTP REST-based API serving NFL data. This can be used " +
                            "to get schedule data and different types of game play by play data. The API is " +
                            "designed to be language/tool agnostic.\n" +
                            "\nMost endpoints can be interacted with the same set of query parameters: " +
                            "`GameId`, `Season`, `Team` and `Week`.",
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

                    c.AddServer(new OpenApiServer()
                    {
                        Url = "https://fourthdown.analytics.com"
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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fourth Down V1");
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseHttpMetrics();
            app.UseAuthentication();
            app.UseAuthorization();

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