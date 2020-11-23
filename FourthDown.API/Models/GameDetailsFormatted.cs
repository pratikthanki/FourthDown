using System.Collections.Generic;
using System.Linq;

namespace FourthDown.Api.Models
{
    public class GameDetailsFormatted
    {
        public GameDetailsFormatted(GameDetail gameDetail)
        {
            Id = gameDetail.Id.First();
            Attendance = gameDetail.Attendance.First();
            Stadium = gameDetail.Stadium.First();
            StartTime = gameDetail.StartTime.First();
            WeatherShortDescription = gameDetail.Weather.ShortDescription.First();

            HomeTeam = new TeamStats
            {
                TeamAbbreviation = gameDetail.HomeTeam.Abbreviation.First(),
                TeamNickName = gameDetail.HomeTeam.NickName.First(),
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
                TeamAbbreviation = gameDetail.VisitorTeam.Abbreviation.First(),
                TeamNickName = gameDetail.VisitorTeam.NickName.First(),
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

        public string Id { get; set; }
        public string Attendance { get; set; }
        public string Stadium { get; set; }
        public string StartTime { get; set; }
        public string WeatherShortDescription { get; set; }
        public TeamStats HomeTeam { get; set; }
        public TeamStats VisitorTeam { get; set; }
        public IList<Drive> Drives { get; set; }
        public IList<Play> Plays { get; set; }
        public IList<ScoringSummary> ScoringSummaries { get; set; }

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

        public GameDrives ParseToGameDrives()
        {
            return new GameDrives()
            {
                Id = Id,
                Attendance = Attendance,
                Stadium = Stadium,
                StartTime = StartTime,
                WeatherShortDescription = WeatherShortDescription,
                HomeTeam = HomeTeam,
                VisitorTeam = VisitorTeam,
                Drives = Drives
            };
        }
        
        public GamePlays ParseToGamePlays()
        {
            return new GamePlays()
            {
                Id = Id,
                Attendance = Attendance,
                Stadium = Stadium,
                StartTime = StartTime,
                WeatherShortDescription = WeatherShortDescription,
                HomeTeam = HomeTeam,
                VisitorTeam = VisitorTeam,
                Plays = Plays
            };
        }
        
        public GameScoringSummaries ParseToGameScoringSummaries()
        {
            return new GameScoringSummaries()
            {
                Id = Id,
                Attendance = Attendance,
                Stadium = Stadium,
                StartTime = StartTime,
                WeatherShortDescription = WeatherShortDescription,
                HomeTeam = HomeTeam,
                VisitorTeam = VisitorTeam,
                ScoringSummaries = ScoringSummaries
            };
        }
    }
    
    public class GameDrives
    {
        public string Id { get; set; }
        public string Attendance { get; set; }
        public string Stadium { get; set; }
        public string StartTime { get; set; }
        public string WeatherShortDescription { get; set; }
        public GameDetailsFormatted.TeamStats HomeTeam { get; set; }
        public GameDetailsFormatted.TeamStats VisitorTeam { get; set; }
        public IList<Drive> Drives { get; set; }
    }

    public class GamePlays
    {
        public string Id { get; set; }
        public string Attendance { get; set; }
        public string Stadium { get; set; }
        public string StartTime { get; set; }
        public string WeatherShortDescription { get; set; }
        public GameDetailsFormatted.TeamStats HomeTeam { get; set; }
        public GameDetailsFormatted.TeamStats VisitorTeam { get; set; }
        public IList<Play> Plays { get; set; }
    }
    
    public class GameScoringSummaries
    {
        public string Id { get; set; }
        public string Attendance { get; set; }
        public string Stadium { get; set; }
        public string StartTime { get; set; }
        public string WeatherShortDescription { get; set; }
        public GameDetailsFormatted.TeamStats HomeTeam { get; set; }
        public GameDetailsFormatted.TeamStats VisitorTeam { get; set; }
        public IList<ScoringSummary> ScoringSummaries { get; set; }
    }
}