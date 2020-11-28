using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public interface IGamePlayService
    {
        public Task<IEnumerable<GamePlays>> GetGamePlays(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);

        public Task<IEnumerable<GameDrives>> GetGameDrives(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);

        public Task<IEnumerable<GameScoringSummaries>> GetGameScoringSummaries(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);
    }
}