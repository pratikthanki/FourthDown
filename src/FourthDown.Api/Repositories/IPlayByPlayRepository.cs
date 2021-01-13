using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

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