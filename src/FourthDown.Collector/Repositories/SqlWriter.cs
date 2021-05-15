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

        public SqlWriter(
            ILogger<SqlWriter> logger,
            IOptions<DatabaseOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task BulkInsertGamesAsync(IEnumerable<Game> games, CancellationToken cancellationToken)
        {
            var enumerable = games.ToList();
            var item = enumerable.First();
            var tableName = GetTableName(item);

            var dataTable = new DataTable();

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

        public Task BulkInsertGamePlaysAsync(IEnumerable<GamePlays> game, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task BulkInsertGameDrivesAsync(IEnumerable<GameDrives> game, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task BulkInsertGameScoringSummariesAsync(IEnumerable<GameScoringSummaries> game, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private static string GetTableName<T>(T item)
        {
            return item switch
            {
                Game _ => "Games",
                GamePlays _ => "GamePlays",
                GameDrives _ => "GameDrives",
                GameScoringSummaries _ => "GameScoringSummaries",
                _ => null
            };
        }
    }
}