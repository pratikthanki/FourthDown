using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FourthDown.Api.Models;
using FourthDown.Api.Parameters;

namespace FourthDown.Api.Repositories
{
    public interface IPlayByPlayRepository
    {
        Task<IEnumerable<PlayByPlay>> GetPlayByPlaysAsync(
            PlayByPlayQueryParameter queryParameter, 
            CancellationToken cancellationToken);
    }
}