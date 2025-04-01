using System.Collections.Generic;
using FourthDown.Shared.Utilities;

namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// A combination of `Season`, `Week` or `Team` can be supplied.
    /// Alternatively, a `GameId` can be given.
    /// One of, `Week` or `Team` must be provided. 
    /// Default to the current season if not provided.
    /// Returns all teams if not provided.
    /// </summary>
    public class PlayByPlayQueryParameter : QueryParameterBase
    {
        /// <summary>
        /// Single Id for specific game to retrieve.
        /// </summary>
        /// <remarks>
        /// GameId should be in the format `Season_Week_VisitorAbr_HomeAbr`.
        /// </remarks>
        /// <example>
        /// `2020_17_DAL_NYG`
        /// </example>
        public string GameId { get; set; }

        public Dictionary<string, string[]> Validate()
        {
            var errors = new Dictionary<string, string[]>();
            var currentSeason = StringParser.GetCurrentSeason();

            if (Season > currentSeason || Season < 1999)
                errors["season"] = new[] { $"Season must be between 2001 and {currentSeason}" };

            if (string.IsNullOrWhiteSpace(GameId) && string.IsNullOrWhiteSpace(Team) && Week is null && Season is null)
            {
                errors["query"] = new[]
                    { "Either gameId or a combination of Week, Season or Team must be provided" };
            }

            return errors;
        }

        public ScheduleQueryParameter ToScheduleQueryParameters()
        {
            if (string.IsNullOrWhiteSpace(GameId))
            {
                return new ScheduleQueryParameter()
                {
                    Season = Season, Week = Week, Team = Team
                };
            }

            var split = GameId.Split("_");

            return new ScheduleQueryParameter()
            {
                Season = StringParser.ToInt(split[0]), Week = StringParser.ToInt(split[1]), Team = string.Empty
            };
        }
    }
}