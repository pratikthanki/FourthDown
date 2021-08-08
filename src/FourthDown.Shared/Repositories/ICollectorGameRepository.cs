using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Shared.Repositories
{
    public interface ICollectorGameRepository
    {
        Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken);
    }
}