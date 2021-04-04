using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FourthDown.Shared.Repositories
{
    public interface IBulkInsert<in TGame, in TGameDetail, in TPlayByPlay>
    {
        Task BulkInsertAsync(IEnumerable<TGame> items, CancellationToken cancellationToken);
        Task BulkInsertAsync(IEnumerable<TGameDetail> items, CancellationToken cancellationToken);
        Task BulkInsertAsync(IEnumerable<TPlayByPlay> items, CancellationToken cancellationToken);
    }
}