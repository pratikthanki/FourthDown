using System.Collections.Generic;
using System.Threading;
using FourthDown.Api.Models;

namespace FourthDown.Api.Repositories
{
    public interface ITeamRepository
    {
        IAsyncEnumerable<Team> GetTeamsAsync(CancellationToken cancellationToken);
    }
}