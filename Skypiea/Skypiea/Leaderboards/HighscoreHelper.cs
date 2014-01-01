using Flai;
using Flai.Scoreloop;
using System.IO.IsolatedStorage;

namespace Skypiea.Leaderboards
{
    public static class HighscoreHelper
    {
        private const string HighscoreFilePath = "highscores.dat";
        public static HighscoreManager CreateHighscoreManager()
        {
            return new HighscoreManager(HighscoreHelper.HighscoreFilePath);
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
                if (isolatedStorage.FileExists(HighscoreHelper.HighscoreFilePath))
                {
                    isolatedStorage.DeleteFile(HighscoreHelper.HighscoreFilePath);
                }
            }
        }

        public static bool IsHighscore(int score)
        {
            HighscoreManager highscoreManager = FlaiGame.Current.Services.Get<HighscoreManager>();
            return score >= highscoreManager.Highscore;
        }
    }
}
