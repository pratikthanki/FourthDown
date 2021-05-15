using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Collector.Repositories
{
    public interface IWriter
    {
        Task InsertGamesAsync(IEnumerable<Game> games, CancellationToken cancellationToken);
        Task InsertGamePlayAsync(IEnumerable<GamePlays> game, CancellationToken cancellationToken);
        Task InsertGameDriveAsync(IEnumerable<GameDrives> game, CancellationToken cancellationToken);
        Task InsertGameScoringSummaryAsync(IEnumerable<GameScoringSummaries> game, CancellationToken cancellationToken);
    }
}