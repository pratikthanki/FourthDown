using System;
using System.Collections.Generic;

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

            var Today = DateTime.UtcNow;
            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;

            if (Week == null && string.IsNullOrWhiteSpace(Team))
                errors["requiredField"] = new[] {$"Week or team must be provided."};

            if (Season > currentSeason || Season < 1999)
                errors["season"] = new[] {$"Season must be between 2001 and {currentSeason}"};

            if (Week < 1 || Week > 21)
                errors["week"] = new[]
                    {"Week must be between 1-17 (REG), Divisional (19), Conference (20) and Super Bowl (21)"};

            if (GameId == null)
            {
                if (!NonGameIdParameterSet())
                    errors["query"] = new[] {"If gameId is not used then one of Week, Season or Team must be provided"};
            }
            else
            {
                if (NonGameIdParameterSet())
                    errors["query"] = new[] {"If gameId is used then Week, Season and Team do not need to be provided"};
            }

            return errors;
        }

        public ScheduleQueryParameter ToScheduleQueryParameters()
        {
            return new ScheduleQueryParameter()
            {
                Season = Season, Week = Week, Team = Team
            };
        }

        private bool NonGameIdParameterSet()
        {
            return Week != null || Season != null || string.IsNullOrWhiteSpace(Team);
        }
    }
}