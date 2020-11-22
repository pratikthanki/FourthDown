using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FourthDown.Api.Parameters
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayByPlayQueryParameter
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Week { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Season { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Team { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GameId { get; set; }

        private bool NonGameIdParameterSet() => Week != null || Season != null || Team != null;

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
            }

            return errors;
        }
#pragma warning restore 1591
    }
}
