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
        /// Single or comma-separated list of GameId's in the format `Season_Week_VisitorAbr_HomeTAbr`
        /// One game: `2020_17_DAL_NYG`
        /// Multiple games: `2020_01_DAL_LA,2020_02_ATL_DAL`
        /// </summary>
        public string GameId { get; set; }

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
            var keys = new Dictionary<string, string[]>
            {
                [nameof(PlayByPlayQueryParameter)] = new[]
                {
                    $"{nameof(Week)}: {Week.ToString()}, " +
                    $"{nameof(Season)}: {Week.ToString()}, " +
                    $"{nameof(Team)}: {Team}, " +
                    $"{nameof(GameId)}: {GameId}"
                }
            };

            return keys;
        }

#pragma warning disable 1591
        public Dictionary<string, string[]> Validate()
        {
            var errors = base.ValidateBase();

            if (GameId == null)
            {
                if (!NonGameIdParameterSet())
                    errors["query"] = new[] {"If gameId is not used then one of Week, Season or Team must be provided"};
            }
            else
            {
                if (NonGameIdParameterSet())
                    errors["query"] = new[] {"If gameId is used then Week, Season and Team do not need to be provided"};

                // var games = GameId.Split(",");
                // if (games.Any(x => x.Length != 15))
                //     errors["gameId"] = new[] {"GameId's provided should be 15 characters long"};
            }

            return errors;
        }
#pragma warning restore 1591
    }
}