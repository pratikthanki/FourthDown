#nullable enable
using System.Collections.Generic;
using System.Threading;
using FourthDown.Shared.Models;

namespace FourthDown.Shared.Repositories
{
    public interface IPlayByPlayRepository
    {
        IAsyncEnumerable<NflfastrPlayByPlayRow> GetPlayByPlaysAsync(
            int season,
            string? team,
            CancellationToken cancellationToken);
    }
}