using System.Collections.Generic;
using System.Threading;
using FourthDown.Shared.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public interface INflfastrService
    {
        IEnumerable<TeamPlayByPlay> GetSummarisedStats(
            NflfastrQueryParameter queryParameter,
            CancellationToken cancellationToken);
    }
}