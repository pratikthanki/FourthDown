using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Shared.Repositories
{
    public interface IGamePlayRepository
    {
        Task<GameDetail> GetGamePlaysAsync(Game game, CancellationToken cancellationToken);

        Task TryPopulateCacheAsync(IEnumerable<Game> games, CancellationToken cancellationToken);
    }
}