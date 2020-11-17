using System.Collections.Generic;
using System.Threading.Tasks;
using FourthDown.Api.Models;

namespace FourthDown.Api.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeams();
    }
}