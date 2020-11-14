using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Collector.Models;
using FourthDown.Shared.Models;

namespace FourthDown.Collector.Repositories
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetGames(CancellationToken cancellationToken);
    }
}