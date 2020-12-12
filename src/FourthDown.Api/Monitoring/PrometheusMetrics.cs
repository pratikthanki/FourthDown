using Prometheus;

namespace FourthDown.Api.Monitoring
{
    public class PrometheusMetrics
    {
        public static readonly Counter PathCounter = Metrics.CreateCounter
        ("path_counter", "Count request by endpoint and http method",
            new CounterConfiguration
            {
                LabelNames = new[] {"method", "endpoint"}
            });

        public static readonly Histogram RecordsReturned = Metrics
            .CreateHistogram($"records_returned", "Total records returned by the endpoint",
                new HistogramConfiguration
                {
                    Buckets = new double[]
                    {
                        50,
                        100,
                        200,
                        500,
                        1000,
                        2000,
                        5000,
                        10000,
                        20000
                    },
                    LabelNames = new[] {"endpoint"}
                });
    }
}