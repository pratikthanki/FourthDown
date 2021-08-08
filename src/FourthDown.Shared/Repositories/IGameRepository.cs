using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Shared.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetGamesForSeason(int season);
        IEnumerable<Game> GetGamesForTeam(string team);
        Task TryPopulateCacheAsync(CancellationToken cancellationToken);
    }
}