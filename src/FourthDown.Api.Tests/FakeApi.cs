using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;
using FourthDown.Shared.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace FourthDown.Api.Tests
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class FakeApi
    {
        private Mock<IGameRepository> _mockGameRepository;
        private Mock<ITeamRepository> _mockTeamRepository;
        private Mock<IGamePlayRepository> _mockGamePlayRepository;
        private Mock<IPlayByPlayRepository> _mockPlayByPlayRepository;

        private WebApplicationFactory<Startup> _factory;
        private HttpClient Client { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new WebApplicationFactory<Startup>();

            Client = _factory.WithWebHostBuilder(builder =>
            {
                builder
                    .ConfigureAppConfiguration((_, config) =>
                    {
                        config.Sources.Clear();
                        var testConfig = new Dictionary<string, string>
                        {
                            ["UseSampleAuth"] = "true"
                        };
                        config.AddInMemoryCollection(testConfig);
                    })
                    .ConfigureTestServices(ConfigureMockServices);
            }).CreateClient();
        }

        private void ConfigureMockServices(IServiceCollection services)
        {
            _mockGameRepository = new Mock<IGameRepository>(MockBehavior.Strict);
            _mockTeamRepository = new Mock<ITeamRepository>(MockBehavior.Strict);
            _mockGamePlayRepository = new Mock<IGamePlayRepository>(MockBehavior.Strict);
            _mockPlayByPlayRepository = new Mock<IPlayByPlayRepository>(MockBehavior.Strict);

            services.AddSingleton(_mockGameRepository.Object);
            services.AddSingleton(_mockTeamRepository.Object);
            services.AddSingleton(_mockGamePlayRepository.Object);
            services.AddSingleton(_mockPlayByPlayRepository.Object);

            _mockGameRepository
                .Setup(g => g.GetGamesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Game>() {new Game()});

            _mockTeamRepository
                .Setup(t => t.GetTeamsAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Team> {new Team()});

            _mockGamePlayRepository
                .Setup(gp => gp.GetGamePlaysAsync(
                    It.IsAny<Game>(),
                    It.IsAny<CancellationToken>()))
                .Returns(() => Task<GameDetail>.Factory.StartNew(() => new GameDetail()));

            _mockPlayByPlayRepository
                .Setup(pbp => pbp.GetPlayByPlaysAsync(
                    It.IsAny<int?>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()));
        }

        protected async Task<(HttpStatusCode, string)> SendRequest(
            string endpoint,
            Dictionary<string, string> parameters)
        {
            var parameterString = parameters == null
                ? ""
                : string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(endpoint + parameterString, UriKind.Relative),
                Method = HttpMethod.Get,
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            return (response.StatusCode, responseString);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _factory?.Dispose();
        }
    }
}