using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FourthDown.Collector.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<string>> GetGameIdsAsync(CancellationToken cancellationToken);
    }
}