#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Shared.Repositories
{
    public interface IPlayByPlayRepository
    {
        Task TryPopulateCacheAsync(CancellationToken cancellationToken);
        
        IEnumerable<NflfastrPlayByPlayRow> GetPlayByPlaysAsync(
            int season,
            string? team,
            CancellationToken cancellationToken);
    }
}