namespace FourthDown.Api.Configuration
{
    public class TelemetryOptions
    {
        public bool UseApplicationInsights { get; set; }
        public bool UseOpenTelemetry { get; set; }
        public string ApplicationInsightsInstrumentationKey { get; set; }
        public string ApplicationInsightsForOpenTelemetryInstrumentationKey { get; set; }
    }
}