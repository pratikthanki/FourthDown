using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Shared.Repositories
{
    public interface IGameRepository : ICollectorGameRepository
    {
        Task<IEnumerable<Game>> GetGamesForSeason(int season, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetGamesForTeam(string team, CancellationToken cancellationToken);
        Task TryPopulateCacheAsync(CancellationToken cancellationToken);
    }
}