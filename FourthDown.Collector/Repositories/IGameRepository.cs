using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Collector.Repositories
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetGames(int season, CancellationToken cancellationToken);
    }
}