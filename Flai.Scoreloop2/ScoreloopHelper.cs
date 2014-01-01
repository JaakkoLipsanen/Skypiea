using Scoreloop.CoreSocial.API;
using Scoreloop.CoreSocial.API.Model;

namespace Flai.Scoreloop
{
    internal static class ScoreloopHelper
    {
        public static ScoreSearchList GetSearchList(LeaderboardScope leaderboardScope, IScoreSearchListProvider scoreSearchListProvider)
        {
            return (leaderboardScope == LeaderboardScope.AllTime) ? scoreSearchListProvider.GlobalSearchList : scoreSearchListProvider.Last24hTimeSearchList;
        }

        public static void InvokeIfNotNull(this ScoreloopCallback callback, ScoreloopResponse response)
        {
            if (callback != null)
            {
                callback(response);
            }
        }

        public static void InvokeIfNotNull<T>(this ScoreloopCallback<T> callback, ScoreloopResponse<T> response)
        {
            if (callback != null)
            {
                callback(response);
            }
        }
    }
}
