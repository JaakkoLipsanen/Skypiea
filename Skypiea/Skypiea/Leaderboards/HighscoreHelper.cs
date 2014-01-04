using Flai;
using Flai.Scoreloop;
using System.IO.IsolatedStorage;

namespace Skypiea.Leaderboards
{
    public static class HighscoreHelper
    {
        private const string FilePath = "highscores.dat";
        public static HighscoreManager CreateHighscoreManager()
        {
            return new HighscoreManager(HighscoreHelper.FilePath);
        }

        public static void SubmitScore(int score)
        {
            HighscoreManager highscoreManager = FlaiGame.Current.Services.Get<HighscoreManager>();
            if (highscoreManager.UpdateHighscore(score))
            {
                highscoreManager.IsHighscorePostedToLeaderboards = false;
                highscoreManager.SaveToFile();
            }

            ScoreloopManager leaderboardManager = FlaiGame.Current.Services.Get<ScoreloopManager>(); // todo todo todo jos submission ei onnistu niin myöhemmin (vähän niinkun tehty atm)
            leaderboardManager.SubmitScore(score, 0, response =>
            {
                // if the score was highscore and the submission failed, then make sure it'll be submitted later
                if (response.Success && score == highscoreManager.Highscore)
                {
                    highscoreManager.IsHighscorePostedToLeaderboards = true;
                    highscoreManager.SaveToFile();
                }
            });
        }

        public static void ResubmitHighscoreIfNeeded()
        {
            HighscoreManager highscoreManager = FlaiGame.Current.Services.Get<HighscoreManager>();
            if (!highscoreManager.IsHighscorePostedToLeaderboards)
            {
                HighscoreHelper.SubmitScore(highscoreManager.Highscore);
            }
        }

        public static void ResetHighscores()
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(HighscoreHelper.FilePath))
                {
                    isolatedStorage.DeleteFile(HighscoreHelper.FilePath);
                }
            }
        }

        // checks if the highscore is the actual highscore that is saved to scoreloop
        // basically if the player re-installs the game, the highscore on scoreloop is saved 
        // but the "local" highscore is resetted
        public static void EnsureHighscoreIsUpToDate()
        {
            ScoreloopManager leaderboardManager = FlaiGame.Current.Services.Get<ScoreloopManager>();
            HighscoreManager highscoreManager = FlaiGame.Current.Services.Get<HighscoreManager>();

            leaderboardManager.GetUserScore(LeaderboardScope.AllTime, 0, response =>
            {
                if (response.Success && response.Data != null && highscoreManager.UpdateHighscore((int)response.Data.Result))
                {
                    highscoreManager.SaveToFile();
                }
            });
        }

        public static bool IsHighscore(int score)
        {
            HighscoreManager highscoreManager = FlaiGame.Current.Services.Get<HighscoreManager>();
            return score >= highscoreManager.Highscore;
        }
    }
}
