using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;

namespace FourthDown.Api.Repositories
{
    public interface IGameRepository
    {
        Task<Dictionary<int, IEnumerable<Game>>> GetGamesAsync(CancellationToken cancellationToken);
    }
}