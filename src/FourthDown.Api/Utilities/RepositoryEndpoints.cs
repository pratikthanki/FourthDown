namespace FourthDown.Api.Utilities
{
    public static class RepositoryEndpoints
    {
        public static readonly string GamesEndpoint =
            "https://github.com/leesharpe/nfldata/blob/master/data/games.csv?raw=true";

        public static readonly string GamePlayEndpoint =
            "https://github.com/pratikthanki/nflfastR-raw/blob/upstream";

        public static readonly string PlayByPlayEndpoint =
            "https://github.com/pratikthanki/nflfastR-data/blob/upstream/data";
    }
}