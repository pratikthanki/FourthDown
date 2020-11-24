using System;
using System.Reflection;
using System.IO;
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
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using OpenTracing;
using OpenTracing.Util;

namespace FourthDown.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<ApiKeyOptions>(Configuration);

            services
                .AddSingleton<IGamePlayService, GameGamePlayService>()
                .AddSingleton<IScheduleService, ScheduleService>()
                .AddSingleton<ITeamRepository, JsonTeamRepository>()
                .AddSingleton<IGameRepository, CsvGameRepository>()
                .AddSingleton<IGamePlayRepository, JsonGamePlayRepository>()
                .AddSingleton<IAuthClient, AuthClient>();

            services.AddControllers();

            services.AddOpenTracing();
            // Adds the Jaeger Tracer.
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var serviceName = serviceProvider.GetRequiredService<IWebHostEnvironment>().ApplicationName;

                // This will log to a default localhost installation of Jaeger.
                var tracer = new Tracer.Builder(serviceName)
                    .WithSampler(new ConstSampler(true))
                    .Build();

                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);

                return tracer;
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
                            Url = new Uri("http://pratikthanki.github.io/"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://choosealicense.com/licenses/mit/"),
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
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] {"data"});

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
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
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