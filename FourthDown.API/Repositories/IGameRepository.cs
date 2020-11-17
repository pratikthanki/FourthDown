using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;

namespace FourthDown.Api.Repositories
{
    public interface IGameRepository
    {
        public Task<Dictionary<int, List<Game>>> GetGames(CancellationToken cancellationToken);
    }
}