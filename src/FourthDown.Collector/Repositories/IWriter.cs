using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Collector.Repositories
{
    public interface IWriter
    {
        Task BulkInsertGamesAsync(IEnumerable<Game> games, CancellationToken cancellationToken);
        Task BulkInsertGamePlaysAsync(IEnumerable<GamePlays> game, CancellationToken cancellationToken);
        Task BulkInsertGameDrivesAsync(IEnumerable<GameDrives> game, CancellationToken cancellationToken);
        Task BulkInsertGameScoringSummariesAsync(IEnumerable<GameScoringSummaries> game, CancellationToken cancellationToken);
    }
}