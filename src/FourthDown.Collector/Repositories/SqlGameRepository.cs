using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FourthDown.Collector.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace FourthDown.Collector.Repositories
{
    public class SqlGameRepository : ISqlGameRepository
    {
        private readonly DatabaseOptions _options;

        public SqlGameRepository(IOptions<DatabaseOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IEnumerable<string>> GetGameIdsAsync(CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_options.ConnectionString);
            await conn.OpenAsync(cancellationToken);

            var query = $"SELECT GameId FROM dbo.Games WITH (NOLOCK)";

            var command = new CommandDefinition(
                commandText: query,
                parameters: new { },
                flags: CommandFlags.None,
                cancellationToken: cancellationToken);

            var gameIds = await conn.QueryAsync<string>(command);

            return gameIds;
        }
    }
}