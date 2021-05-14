using System;
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

        /// <summary>
        /// Get the dateTime of the last game inserted. The DateTime time zone is in EST.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DateTime> GetLastGameDateTimeAsync(CancellationToken cancellationToken)
        {
            await using var conn = new SqlConnection(_options.ConnectionString);
            await conn.OpenAsync(cancellationToken);

            var query = $"SELECT MAX(CAST(CONCAT(Gameday, ' ', Gametime) AS datetime)) FROM dbo.Games WITH (NOLOCK)";

            var command = new CommandDefinition(
                commandText: query,
                parameters: new { },
                flags: CommandFlags.None,
                cancellationToken: cancellationToken);

            return await conn.QuerySingleAsync<DateTime>(command);
        }
    }
}