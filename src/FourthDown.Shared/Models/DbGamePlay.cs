namespace FourthDown.Shared.Models
{
    public class DbGamePlay
    {
        private readonly GameDetail _gameDetail;
        public DbGamePlay(GameDetail gameDetail)
        {
            _gameDetail = gameDetail;
        }

        public class GameTeams : ApiGamePlay.TeamStats
        {
            public bool IsHome { get; set; }
        }

        public class DbGameDrives : Drive
        {
            public string GameId { get; set; }
        }
    }
}