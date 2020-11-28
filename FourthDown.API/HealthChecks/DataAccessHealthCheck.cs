using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Utilities;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FourthDown.Api.HealthChecks
{
    public class DataAccessHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            const string url =
                @"https://github.com/pratikthanki/nflfastR-raw/blob/master/raw/2020/2020_01_DAL_LA.json.gz?raw=true";

            var response = await RequestHelper.GetRequestResponse(url, cancellationToken);
            var healthCheckResultHealthy = response.IsSuccessStatusCode;

            if (healthCheckResultHealthy)
                return await Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));

            return await Task.FromResult(
                HealthCheckResult.Unhealthy("An unhealthy result."));
        }
    }
}