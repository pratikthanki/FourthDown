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
            async Task<bool> GetStatusCode(string s)
            {
                var response = await RequestHelper.GetRequestResponse(s, cancellationToken);
                return response.IsSuccessStatusCode;
            }

            const string jsonDataUrl =
                @"https://github.com/pratikthanki/nflfastR-raw/blob/master/raw/2020/2020_01_DAL_LA.json.gz?raw=true";

            const string csvDataUrl =
                @"https://github.com/pratikthanki/nflfastR-data/blob/master/data/play_by_play_2020.csv.gz?raw=true";

            var jsonDataResponse = await GetStatusCode(jsonDataUrl);
            var csvDataResponse = await GetStatusCode(csvDataUrl);

            if (jsonDataResponse && csvDataResponse)
                return await Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));

            return await Task.FromResult(
                HealthCheckResult.Unhealthy("An unhealthy result."));
        }
    }
}