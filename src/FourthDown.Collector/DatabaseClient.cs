using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FourthDown.Database;
using FourthDown.Shared.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace FourthDown.Collector
{
    public interface IDatabaseClient<in T>
    {
        Task<IEnumerable<string>> GetGameIds(CancellationToken cancellationToken);

        Task<bool> Write(string tableName, IEnumerable<T> records);
    }

    public class DatabaseClient : IDatabaseClient<GameDetail>
    {
        private readonly string _connectionString;
        private readonly int WriteBatchSize = 10_000;
        private readonly int CommandTimeout = 60;

        public DatabaseClient(
            IOptions<DatabaseOptions> databaseOptions)
        {
            _connectionString = databaseOptions.Value.ConnectionString;
        }

        public async Task<IEnumerable<string>> GetGameIds(CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken);

            var query = "SELECT GameId FROM dbo.Schedule";

            var command = new CommandDefinition(
                query,
                new { },
                flags: CommandFlags.None,
                commandTimeout: 5,
                cancellationToken: cancellationToken);

            var gameIds = await conn.QueryAsync<string>(command);

            return gameIds;
        }

        public Task<bool> Write(string tableName, IEnumerable<GameDetail> records)
        {
            throw new NotImplementedException();
            
            // await using var conn = new SqlConnection(_connectionString);
            // await conn.OpenAsync();
            //
            // using var sqlBulkCopy = new SqlBulkCopy(_connectionString)
            // {
            //     BatchSize = WriteBatchSize,
            //     DestinationTableName = tableName,
            //     BulkCopyTimeout = CommandTimeout,
            //     EnableStreaming = true
            // };
            //
            // foreach (var columnName in MyDataReader.ColumnNames)
            //     sqlBulkCopy.ColumnMappings.Add(columnName, columnName);
            //
            // await using var dataReader = new MyDataReader(records);
            //
            // await sqlBulkCopy.WriteToServerAsync(dataReader);
            //
            // return false;
        }
    }
}