using Microsoft.AspNetCore.Http;

namespace FourthDown.Api.Monitoring
{
    public class MetricCollector
    {
        public static void RegisterMetrics(HttpContext httpContext, HttpRequest request, int data)
        {
            PrometheusMetrics.PathCounter
                .WithLabels(request.Method, httpContext.GetEndpoint().DisplayName)
                .Inc();

            if (request.ContentLength != null)
                PrometheusMetrics.RequestSize
                    .WithLabels(request.Method, httpContext.GetEndpoint().DisplayName)
                    .Observe((double) request.ContentLength);

            PrometheusMetrics.RecordsReturned
                .WithLabels(request.Method, httpContext.GetEndpoint().DisplayName)
                .Observe(data);
        }
    }
}