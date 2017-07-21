using System.IO.IsolatedStorage;

namespace Skypiea.Stats
{
    public static class StatsHelper
    {
        private const string FilePath = "stats.dat";
        public static StatsManager CreateStatsManager()
        {
            return new StatsManager(StatsHelper.FilePath);
        }

        public static void ResetStats()
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(StatsHelper.FilePath))
                {
                    isolatedStorage.DeleteFile(StatsHelper.FilePath);
                }
            }
        }
    }
}
