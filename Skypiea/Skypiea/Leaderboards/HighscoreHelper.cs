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
    }
}
