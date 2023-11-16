namespace FourthDown.Shared.Utilities
{
    public static class RepositoryEndpoints
    {
        /// <summary>
        /// List of Games going back to 1999
        /// </summary>
        public const string GamesEndpoint = "https://github.com/leesharpe/nfldata/blob/master/data/games.csv?raw=true";

        /// <summary>
        /// Raw json which containing Scoring Summaries, Game Plays and Drives
        /// </summary>
        public const string GamePlayEndpoint = "https://github.com/nflverse/nflfastR-raw/blob/master/raw";

        /// <summary>
        /// NflfastR play-by-play data endpoint
        /// </summary>
        public const string PlayByPlayEndpoint = "https://github.com/nflverse/nflverse-data/releases/download/pbp";
    }
}