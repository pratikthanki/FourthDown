using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public interface INflfastrService
    {
        Task<IEnumerable<TeamPlayByPlay>> GetSummarisedStats(
            NflfastrQueryParameter queryParameter,
            CancellationToken cancellationToken);
    }
}