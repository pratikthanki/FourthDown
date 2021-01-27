namespace FourthDown.Collector.Options
{
    public class DatabaseOptions
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public string ConnectionString =>
            $"Server={Server};Database={Database};User Id={UserId};Password='{Password}'";

        public string MasterConnectionString =>
            $"Server={Server};Database=master;User Id={UserId};Password='{Password}'";

        public override string ToString() => ConnectionString;
    }
}