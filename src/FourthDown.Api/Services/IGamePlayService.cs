using System.Collections.Generic;
using System.Threading;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public interface IGamePlayService
    {
        IAsyncEnumerable<GamePlays> GetGamePlaysAsync(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);

        IAsyncEnumerable<GameDrives> GetGameDrivesAsync(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);

        IAsyncEnumerable<GameScoringSummaries> GetGameScoringSummariesAsync(
            PlayByPlayQueryParameter queryParameter,
            CancellationToken cancellationToken);
    }
}