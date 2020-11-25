using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;

namespace FourthDown.Api.Repositories
{
    public interface IGamePlayRepository
    {
        Task<GameDetail> GetGamePlays(string gameId, int season, CancellationToken cancellationToken);
    }
}