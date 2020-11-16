using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;

namespace FourthDown.Api.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<Game>> GetGamesForWeek(int week, CancellationToken cancellationToken);
        Task<IEnumerable<Game>> GetGamesForCurrentWeek(CancellationToken cancellationToken);
    }
}