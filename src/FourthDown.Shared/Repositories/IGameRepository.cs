using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Shared.Repositories
{
    public interface IGameRepository
    {
        Task<Dictionary<int, IEnumerable<Game>>> GetGamesAsync(CancellationToken cancellationToken);
    }
}