using Microsoft.AspNetCore.Http;

namespace FourthDown.Api.Monitoring
{
    public static class MetricCollector
    {
        public static void RegisterMetrics(HttpContext httpContext, HttpRequest request)
        {
            PrometheusMetrics.PathCounter
                .WithLabels(request.Method, httpContext.GetEndpoint().DisplayName)
                .Inc();

            if (request.ContentLength != null)
                PrometheusMetrics.RequestSize
                    .WithLabels(request.Method, httpContext.GetEndpoint().DisplayName)
                    .Observe((double) request.ContentLength);
        }
    }
}