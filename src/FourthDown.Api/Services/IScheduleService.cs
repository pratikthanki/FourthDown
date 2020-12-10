using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<Game>> GetGames(
            ScheduleQueryParameter queryParameter,
            CancellationToken cancellationToken);

        Task<IEnumerable<Game>> GetGamesById(
            Dictionary<int, List<string>> gameIdsBySeason,
            CancellationToken cancellationToken);
    }
}