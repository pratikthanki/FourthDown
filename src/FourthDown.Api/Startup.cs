using System;
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
using Jaeger.Samplers;
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
using OpenTracing.Util;

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
                .Configure<ApiKeyOptions>(Configuration);

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
                    // Adds the Jaeger Trace.

                    var serviceName = serviceProvider.GetRequiredService<IWebHostEnvironment>().ApplicationName;
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                    ISampler sampler = new ConstSampler(true);

                    // This will log to a default localhost installation of Jaeger.
                    var tracer = new Tracer.Builder(serviceName)
                        .WithLoggerFactory(loggerFactory)
                        .WithSampler(sampler)
                        .Build();

                    // Allows code that can't use DI to also access the tracer.
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
                        Description = "Web API serving NFL data",
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
            Environment.SetEnvironmentVariable("JAEGER_SERVICE_NAME", "my-service-name");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Environment.SetEnvironmentVariable("JAEGER_AGENT_HOST", "localhost");
                Environment.SetEnvironmentVariable("JAEGER_AGENT_PORT", "6831");
                Environment.SetEnvironmentVariable("JAEGER_SAMPLER_TYPE", "const");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fourth Down V1");
                c.InjectStylesheet("/swagger-ui/custom.css");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
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
            });
        }
    }
}