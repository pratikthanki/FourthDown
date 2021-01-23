using System.Collections.Generic;
using System.Threading.Tasks;
using FourthDown.Api.Models;

namespace FourthDown.Client.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Team>> GetTeams();

        Task<IEnumerable<Game>> GetGames();
    }
}