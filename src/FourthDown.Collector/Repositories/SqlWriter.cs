using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Collector.Options;
using FourthDown.Shared.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FourthDown.Collector.Repositories
{
    public class SqlWriter : IWriter
    {
        private readonly ILogger _logger;
        private readonly DatabaseOptions _options;

        private const string GamesTable = "Games";
        private const string GameTeamsTable = "GameTeams";
        private const string GameDrivesTable = "GameDrives";
        private const string GameScoringSummariesTable = "GameScoringSummaries";
        private const string GamePlaysTable = "GamePlays";

        public SqlWriter(
            ILogger<SqlWriter> logger,
            IOptions<DatabaseOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task BulkInsertAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken)
        {
            var enumerable = items.ToList();
            var dataTable = new DataTable();

            var tableName = GetTableName(enumerable);

            // TODO add columns
            dataTable.Columns.Add("Column");

            foreach (var i in enumerable)
            {
                dataTable.Rows.Add(i);
            }

            using var sqlBulkCopy = new SqlBulkCopy(_options.ConnectionString)
            {
                DestinationTableName = tableName
            };

            await sqlBulkCopy.WriteToServerAsync(dataTable, cancellationToken);
        }

        private static string GetTableName<T>(IEnumerable<T> items)
        {
            var item = items.First();

            return item switch
            {
                Game _ => GamesTable,
                GameDetail _ => GameDrivesTable,
                _ => null
            };
        }
    }
}