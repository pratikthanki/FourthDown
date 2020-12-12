namespace FourthDown.Api.Configuration
{
    public class TracerOptions
    {
        public string ServiceName { get; set; }
        public TracerMode Mode { get; set; }
        public string HttpEndPoint { get; set; }
        public TracerUdpEndPoint UdpEndPoint { get; set; }
    }

    public class TracerUdpEndPoint
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public enum TracerMode
    {
        Udp,
        Http
    }
}