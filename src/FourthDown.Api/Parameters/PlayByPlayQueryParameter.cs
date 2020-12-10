using System;
using System.Collections.Generic;
using System.Linq;
using FourthDown.Api.Utilities;

namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// </summary>
    public class PlayByPlayQueryParameter : QueryParameterBase
    {
        /// <summary>
        /// GameId or comma-separated list in the format `Season_Week_VisitorAbr_HomeTAbr`
        /// </summary>
        /// <example>
        /// One game: `2020_17_DAL_NYG`
        /// Multiple games: `2020_01_DAL_LA,2020_02_ATL_DAL`
        /// </example>
        public string GameId { get; set; }

        private bool NonGameIdParameterSet()
        {
            return Week != null || Season != null || Team != null;
        }

        public Dictionary<int, List<string>> GetGameIdsBySeason()
        {
            var gameIds = GameId.Split(",");
            var seasonGameIds = gameIds.Select(g => (g.Substring(0, 4), g)).Distinct();

            var gameIdsBySeason = seasonGameIds
                .GroupBy(x => StringParser.ToInt(x.Item1))
                .ToDictionary(x => x.Key, x => x.Select(x => x.g).ToList());

            return gameIdsBySeason;
        }

        public Dictionary<string, string[]> ToKeyValues()
        {
            var keys = new Dictionary<string, string[]>();

            keys[nameof(PlayByPlayQueryParameter)] =
                new[]
                {
                    $"{nameof(Week)}: {Week.ToString()}, {nameof(Season)}: {Week.ToString()}, {nameof(Team)}: {Team}, {nameof(GameId)}: {GameId}"
                };

            return keys;
        }

#pragma warning disable 1591
        public Dictionary<string, string[]> Validate()
        {
            var errors = new Dictionary<string, string[]>();

            if (GameId == null)
            {
                if (!NonGameIdParameterSet())
                    errors["query"] = new[] {"If gameId is not used then one of Week, Season or Team must be provided"};
            }
            else
            {
                if (NonGameIdParameterSet())
                    errors["query"] = new[] {"If gameId is used then Week, Season and Team do not need to be provided"};

                var games = GameId.Split(",");
                if (games.Any(x => x.Length != 15))
                    errors["gameId"] = new[] {"GameId's provided should be 15 characters long"};
            }

            var Today = DateTime.UtcNow;
            var currentSeason = Today.Month > 8 ? Today.Year : Today.Year - 1;

            if (Season > currentSeason || Season < 1999)
                errors["query"] = new[] {$"Season must be between 1999 and {currentSeason}"};

            return errors;
        }
#pragma warning restore 1591
    }
}