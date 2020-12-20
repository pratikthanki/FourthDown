using System.Collections.Generic;
using System.Threading;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public interface IGamePlayService
    {
        public IAsyncEnumerable<GamePlays> GetGamePlaysAsync(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);

        public IAsyncEnumerable<GameDrives> GetGameDrivesAsync(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);

        public IAsyncEnumerable<GameScoringSummaries> GetGameScoringSummariesAsync(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);
    }
}