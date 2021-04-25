using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FourthDown.Collector.Repositories
{
    public interface IWriter<in T> where T : class
    {
        Task BulkInsertAsync(IEnumerable<T> items, CancellationToken cancellationToken);
    }
}