using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace FourthDown.Collector.Repositories
{
    public interface IReader<T>
    {
        Task<IEnumerable<T>> ReadAsync(
            CommandDefinition command,
            CancellationToken cancellationToken);
    }
}