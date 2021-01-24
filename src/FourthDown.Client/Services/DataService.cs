using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Utilities;
using FourthDown.Client.Configuration;
using Microsoft.Extensions.Options;

namespace FourthDown.Client.Services
{
    public class DataService : IDataService
    {
        private readonly string _baseUrl;
        private const string teamEndpoint = "/api/teams";
        private const string combineEndpoint = "/api/combine";
        private const string scheduleEndpoint = "/api/schedule";
        private const string scheduleResultsEndpoint = "/api/schedule/results";
        private const string nflfastrEndpoint = "/api/nflfastr";
        private const string gamePlaysEndpoint = "/api/game/plays";
        private const string gameDrivesEndpoint = "/api/game/drives";
        private const string gameScoringSummariesEndpoint = "/api/game/scoringsummaries";

        public DataService(IOptions<ApiOptions> apiOptions)
        {
            _baseUrl = apiOptions.Value.ApiBaseUrl;
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            return await GetData<Team>(teamEndpoint);
        }

        public async Task<IEnumerable<Game>> GetGames()
        {
            var Today = DateTime.UtcNow;
            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;

            var data = (await GetData<Game>(scheduleEndpoint)).ToList();
            var currentWeek = data
                .Where(game => game.Gameday >= Today.Date)
                .Select(game => game.Week).Min();

            return data.Where(game => game.Week == currentWeek);
        }

        private async Task<IEnumerable<T>> GetData<T>(string endpoint)
        {
            var url = $"{_baseUrl}{endpoint}";

            var response = await RequestHelper.GetRequestResponse(url, CancellationToken.None);

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<T>();

            var data = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<IEnumerable<T>>(data, StringParser.JsonSerializerOptions);

            return results;
        }
    }
}