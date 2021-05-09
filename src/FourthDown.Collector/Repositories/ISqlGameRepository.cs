using System;
using System.Threading;
using System.Threading.Tasks;

namespace FourthDown.Collector.Repositories
{
    public interface ISqlGameRepository
    {
        Task<DateTime> GetLastGameDateTimeAsync(CancellationToken cancellationToken);
    }
}