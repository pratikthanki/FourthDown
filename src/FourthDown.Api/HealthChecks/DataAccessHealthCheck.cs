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
            const string jsonDataUrl = $"{RepositoryEndpoints.GamePlayEndpoint}/2020/2020_01_DAL_LA.json.gz?raw=true";
            const string csvDataUrl = $"{RepositoryEndpoints.PlayByPlayEndpoint}/play_by_play_2020.csv.gz?raw=true";
            const string gamesDataUrl = RepositoryEndpoints.GamesEndpoint;

            var jsonData = await _requestHelper.GetRequestResponse(jsonDataUrl, cancellationToken);
            var csvData = await _requestHelper.GetRequestResponse(csvDataUrl, cancellationToken);
            var gamesData = await _requestHelper.GetRequestResponse(gamesDataUrl, cancellationToken);

            var state =
                $"jsonDataResponse ({jsonData.StatusCode}), csvDataResponse ({csvData.StatusCode}), gamesDataResponse ({gamesData.StatusCode})";

            var isAllHealthy = jsonData.IsSuccessStatusCode &&
                               csvData.IsSuccessStatusCode &&
                               gamesData.IsSuccessStatusCode;

            var result = isAllHealthy
                ? Task.FromResult(HealthCheckResult.Healthy("A healthy result."))
                : Task.FromResult(HealthCheckResult.Unhealthy($"An unhealthy result: {state}"));

            return await result;
        }
    }
}