using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public interface IPlayByPlayService
    {
        public Task<IEnumerable<GameRaw>> GetPlayByPlays(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);

        public Task<IEnumerable<WinProbability>> GetGameWinProbability(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);
    }
}