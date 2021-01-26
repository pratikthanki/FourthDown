using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace FourthDown.Database
{
    public interface IDatabaseClient
    {
        Task<bool> PreDeploymentCheckSuccessful(CancellationToken cancellationToken);
    }

    public class DatabaseClient : IDatabaseClient
    {
        private readonly DatabaseOptions _databaseOptions;
        private readonly int _commandTimeout = 30;

        public DatabaseClient(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions.Value;
        }

        private async Task<IEnumerable<ChangeScript>> GetChangeScripts(CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_databaseOptions.ConnectionString);
            await conn.OpenAsync(cancellationToken);

            var command = new CommandDefinition(
                DatabaseStatements.GetAllChangeScripts,
                new { },
                flags: CommandFlags.None,
                commandTimeout: _commandTimeout,
                cancellationToken: cancellationToken);

            var scripts = await conn.QueryAsync<ChangeScript>(command);

            return scripts.Where(s => s.ChangeScriptDeploySuccess);
        }

        public async Task<bool> PreDeploymentCheckSuccessful(CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_databaseOptions.ConnectionString);
            await conn.OpenAsync(cancellationToken);

            var databaseQuery = $@"SELECT 1 FROM sys.databases WHERE [name] = '{_databaseOptions.Database}';";
            var tableQuery = $@"SELECT 1 FROM sys.tables WHERE [name] = 'ChangeScripts';";

            var databaseExists = await ExecuteScalarAsync<bool>(conn, databaseQuery, cancellationToken);
            var tableExists = await ExecuteScalarAsync<bool>(conn, tableQuery, cancellationToken);

            return databaseExists && tableExists;
        }

        private async Task<T> ExecuteScalarAsync<T>(SqlConnection conn, string query, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(
                query,
                parameters: new { },
                commandTimeout: _commandTimeout,
                cancellationToken: cancellationToken
            );

            return await conn.ExecuteScalarAsync<T>(command);
        }
    }
}