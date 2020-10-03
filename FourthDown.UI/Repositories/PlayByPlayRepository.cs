using System.Collections.Generic;
using FourthDown.UI.Models;

namespace FourthDown.UI.Repositories
{
    public class PlayByPlayRepository : IPlayByPlayRepository
    {
        public IEnumerable<PlayByPlay> GetAllPlays()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PlayByPlay> GetGamePlays(string gameId)
        {
            throw new System.NotImplementedException();
        }

        public PlayByPlay GetGamePlayByPlay(string gameId, string playId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<PlayByPlay> GetTeamPlays(string gameId, string team)
        {
            throw new System.NotImplementedException();
        }

        private static IEnumerable<PlayByPlay> ReadCsv()
        {
            throw new System.NotImplementedException();
        }
    }
}