using System.Collections.Generic;
using System.Threading;
using FourthDown.Api.Models;

namespace FourthDown.Api.Repositories
{
    public interface IPlayByPlayRepository
    {
        IAsyncEnumerable<NflfastrPlayByPlay> GetPlayByPlaysAsync(
            int? season,
            string team,
            CancellationToken cancellationToken);
    }
}