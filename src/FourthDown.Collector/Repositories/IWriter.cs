using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FourthDown.Collector.Repositories
{
    public interface IWriter
    {
        Task BulkInsertAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken);
    }
}