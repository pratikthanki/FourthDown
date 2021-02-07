namespace FourthDown.Collector.Options
{
    public class DatabaseOptions
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public string ConnectionString => ConnectionStringBuilder(Database);

        public string MasterConnectionString => ConnectionStringBuilder("master");

        private string ConnectionStringBuilder(string db) =>
            $"Server={Server};Database={db};User Id={UserId};Password='{Password}'";

        public override string ToString() => ConnectionString;
    }
}