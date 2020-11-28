namespace FourthDown.Api.Configuration
{
    public class JaegerConfiguration
    {
        public string AgentHost { get; set; } = "localhost";
        public int AgentPort { get; set; } = 6831;
    }
}