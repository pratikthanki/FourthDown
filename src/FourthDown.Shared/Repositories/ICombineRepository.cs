using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Shared.Models;

namespace FourthDown.Shared.Repositories
{
    public interface ICombineRepository
    {
        Task<IEnumerable<CombineWorkout>> GetCombineSummaryAsync(
            string webRootPath,
            int season,
            CancellationToken cancellationToken);
    }
}