using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FourthDown.Api.HealthChecks
{
    public class DataAccessHealthCheck : IHealthCheck
    {
        private readonly IRequestHelper _requestHelper;

        public DataAccessHealthCheck(IRequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            async Task<bool> GetStatusCode(string s)
            {
                var response = await _requestHelper.GetRequestResponse(s, cancellationToken);
                return response.IsSuccessStatusCode;
            }

            var jsonDataUrl = $"{RepositoryEndpoints.GamePlayEndpoint}/2020/2020_01_DAL_LA.json.gz?raw=true";
            var csvDataUrl = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_2020.csv.gz?raw=true";
            var gamesDataUrl = RepositoryEndpoints.GamesEndpoint;

            var jsonDataResponse = await GetStatusCode(jsonDataUrl);
            var csvDataResponse = await GetStatusCode(csvDataUrl);
            var gamesDataResponse = await GetStatusCode(gamesDataUrl);

            var state = "";

            if (jsonDataResponse && csvDataResponse && gamesDataResponse)
                return await Task.FromResult(HealthCheckResult.Healthy("A healthy result."));

            if (!jsonDataResponse) state += $"{nameof(jsonDataResponse)}\n";
            if (!csvDataResponse) state += $"{nameof(csvDataResponse)}\n";
            if (!gamesDataResponse) state += $"{nameof(gamesDataResponse)}\n";

            return await Task.FromResult(HealthCheckResult.Unhealthy($"An unhealthy result: {state}"));
        }
    }
}