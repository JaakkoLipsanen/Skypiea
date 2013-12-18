using Flai.Scoreloop;

namespace Skypiea.Leaderboards
{
    public static class LeaderboardHelper
    {
        private const string GameID = "a6aef0a2-6df9-47f0-a685-3c8b3a2025a7";
        private const string Secret = "80spobqzeSFSeDQpMdPHXox12m+0HhznOzlF2qdLulhjzylk2ZLkrw==";
        private const string Currency = "BWK";

        // meh.. abstract the ScoreloopManager functionality to "ILeaderboardManager" tms interface and don't expose to Scoreloop stuff at all..?
        // could be a bit too hard vs profits so maybe not.. but think about it
        public static ScoreloopManager CreateLeaderboardManager()
        {
            return new ScoreloopManager(LeaderboardHelper.GameID, LeaderboardHelper.Secret, LeaderboardHelper.Currency);
        }
    }
}
