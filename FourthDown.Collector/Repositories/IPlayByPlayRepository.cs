using System.Collections.Generic;
using FourthDown.Collector.Models;

namespace FourthDown.Collector.Repositories
{
    public interface IPlayByPlayRepository
    {
        public IEnumerable<PlayByPlay> ReadPlays();
    }
}