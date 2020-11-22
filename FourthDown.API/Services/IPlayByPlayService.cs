using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public interface IPlayByPlayService
    {
        public Task<List<PlayByPlay>> GetGamePlayByPlays(PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);

        public Task<IEnumerable<WinProbability>> GetGameWinProbability(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);
    }
}
