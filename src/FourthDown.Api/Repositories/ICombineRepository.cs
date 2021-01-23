using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;

namespace FourthDown.Api.Repositories
{
    public interface ICombineRepository
    {
        Task<IEnumerable<CombineWorkout>> GetCombineSummaryAsync(int season, CancellationToken cancellationToken);
    }
}