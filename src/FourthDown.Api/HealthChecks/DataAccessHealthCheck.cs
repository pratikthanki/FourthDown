using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Utilities;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FourthDown.Api.HealthChecks
{
    public class DataAccessHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            async Task<bool> GetStatusCode(string s)
            {
                var response = await RequestHelper.GetRequestResponse(s, cancellationToken);
                return response.IsSuccessStatusCode;
            }

            var jsonDataUrl = $"{RepositoryEndpoints.GamePlayEndpoint}/raw/2020/2020_01_DAL_LA.json.gz?raw=true";
            var csvDataUrl = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_2020.csv.gz?raw=true";
            var gamesDataUrl = RepositoryEndpoints.GamesEndpoint;

            var jsonDataResponse = await GetStatusCode(jsonDataUrl);
            var csvDataResponse = await GetStatusCode(csvDataUrl);
            var gamesDataResponse = await GetStatusCode(gamesDataUrl);

            if (jsonDataResponse && csvDataResponse && gamesDataResponse)
                return await Task.FromResult(HealthCheckResult.Healthy("A healthy result."));

            return await Task.FromResult(HealthCheckResult.Unhealthy("An unhealthy result."));
        }
    }
}