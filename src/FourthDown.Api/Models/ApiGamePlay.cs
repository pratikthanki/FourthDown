using System.Collections.Generic;
using System.Linq;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace FourthDown.Api.Models
{
    public class ApiGamePlay
    {
        public ApiGamePlay(GameDetail gameDetail)
        {
            Game = gameDetail.Game;

            HomeTeam = new TeamStats
            {
                TeamAbbreviation = gameDetail.HomeGameTeam.Abbreviation.First(),
                TeamNickName = gameDetail.HomeGameTeam.NickName.First(),
                PointsQ1 = gameDetail.HomePointsQ1.Sum(),
                PointsQ2 = gameDetail.HomePointsQ2.Sum(),
                PointsQ3 = gameDetail.HomePointsQ3.Sum(),
                PointsQ4 = gameDetail.HomePointsQ4.Sum(),
                PointsOvertimeTotal = gameDetail.HomePointsOvertimeTotal.Sum(),
                PointsTotal = gameDetail.HomePointsTotal.Sum(),
                TimeoutsUsed = gameDetail.HomeTimeoutsUsed.Sum(),
                TimeoutsRemaining = gameDetail.HomeTimeoutsRemaining.Sum()
            };

            VisitorTeam = new TeamStats
            {
                TeamAbbreviation = gameDetail.VisitorGameTeam.Abbreviation.First(),
                TeamNickName = gameDetail.VisitorGameTeam.NickName.First(),
                PointsQ1 = gameDetail.VisitorPointsQ1.Sum(),
                PointsQ2 = gameDetail.VisitorPointsQ2.Sum(),
                PointsQ3 = gameDetail.VisitorPointsQ3.Sum(),
                PointsQ4 = gameDetail.VisitorPointsQ4.Sum(),
                PointsOvertimeTotal = gameDetail.VisitorPointsOvertimeTotal.Sum(),
                PointsTotal = gameDetail.VisitorPointsTotal.Sum(),
                TimeoutsUsed = gameDetail.VisitorTimeoutsUsed.Sum(),
                TimeoutsRemaining = gameDetail.VisitorTimeoutsRemaining.Sum()
            };

            Drives = gameDetail.Drives;
            Plays = gameDetail.Plays;
            ScoringSummaries = gameDetail.ScoringSummaries;
        }

        private Game Game { get; }
        private TeamStats HomeTeam { get; }
        private TeamStats VisitorTeam { get; }
        private IList<Drive> Drives { get; }
        private IList<Play> Plays { get; }
        private IList<ScoringSummary> ScoringSummaries { get; }

        public GameDrives ToGameDrives()
        {
            return new GameDrives
            {
                Game = Game,
                HomeTeam = HomeTeam,
                VisitorTeam = VisitorTeam,
                Drives = Drives
            };
        }

        public GamePlays ToGamePlays()
        {
            return new GamePlays
            {
                Game = Game,
                HomeTeam = HomeTeam,
                VisitorTeam = VisitorTeam,
                Plays = Plays
            };
        }

        public GameScoringSummaries ToGameScoringSummaries()
        {
            return new GameScoringSummaries
            {
                Game = Game,
                HomeTeam = HomeTeam,
                VisitorTeam = VisitorTeam,
                ScoringSummaries = ScoringSummaries
            };
        }

        public class TeamStats
        {
            public string TeamAbbreviation { get; set; }
            public string TeamNickName { get; set; }
            public int PointsQ1 { get; set; }
            public int PointsQ2 { get; set; }
            public int PointsQ3 { get; set; }
            public int PointsQ4 { get; set; }
            public int PointsOvertimeTotal { get; set; }
            public int PointsTotal { get; set; }
            public int TimeoutsUsed { get; set; }
            public int TimeoutsRemaining { get; set; }
        }
    }

    public class GameDrives
    {
        public Game Game { get; set; }
        public ApiGamePlay.TeamStats HomeTeam { get; set; }
        public ApiGamePlay.TeamStats VisitorTeam { get; set; }
        public IList<Drive> Drives { get; set; }
    }

    public class GamePlays
    {
        public Game Game { get; set; }
        public ApiGamePlay.TeamStats HomeTeam { get; set; }
        public ApiGamePlay.TeamStats VisitorTeam { get; set; }
        public IList<Play> Plays { get; set; }
    }

    public class GameScoringSummaries
    {
        public Game Game { get; set; }
        public ApiGamePlay.TeamStats HomeTeam { get; set; }
        public ApiGamePlay.TeamStats VisitorTeam { get; set; }
        public IList<ScoringSummary> ScoringSummaries { get; set; }
    }
}