using System.Collections.Generic;

namespace FourthDown.Api.Parameters
{
    public class PlayByPlayQueryParameter
    {
        public int? Week { get; set; }
        public int? Season { get; set; }
        public string Team { get; set; }
        public string GameId { get; set; }

        private bool NonGameIdParameterSet() => Week != null || Season != null || Team != null;

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
    }
}