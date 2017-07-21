using Flai;
using Flai.Misc;
using Flai.Scoreloop;
using Skypiea.Settings;

namespace Skypiea.Leaderboards
{
    public static class LeaderboardHelper
    {
        private const string GameID = "a6aef0a2-6df9-47f0-a685-3c8b3a2025a7";
        private const string Secret = "80spobqzeSFSeDQpMdPHXox12m+0HhznOzlF2qdLulhjzylk2ZLkrw==";
        private const string Currency = "BWK";

        // hacks... but otherwise I'd need to add a new object to Settings
        private static bool IsAskUsernameShownOnce = false;

        // meh.. abstract the ScoreloopManager functionality to "ILeaderboardManager" tms interface and don't expose to Scoreloop stuff at all..?
        // could be a bit too hard vs profits so maybe not.. but think about it
        public static ScoreloopManager CreateLeaderboardManager()
        {
            return new ScoreloopManager(LeaderboardHelper.GameID, LeaderboardHelper.Secret, LeaderboardHelper.Currency);
        }

        public static void ShowAskUsername()
        {
            if (LeaderboardHelper.IsAskUsernameShownOnce)
            {
                return;
            }

            LeaderboardHelper.IsAskUsernameShownOnce = true;
            SkypieaSettingsManager settingsManager = FlaiGame.Current.Services.Get<SkypieaSettingsManager>();
            if (settingsManager.Settings.IsFirstLaunch)
            {
                if (DeviceInfo.IsNetworkAvailable)
                {
                    GuideHelper.ShowKeyboardInput("Username for leaderboards", "Give your preferred username for the global leaderboards. Username can be changed in options later", input =>
                    {
                        if (input == null)
                        {
                            return;
                        }

                        if (input.Length < 4 || input.Length > 26)
                        {
                            GuideHelper.ShowMessageBox("Invalid username!", "Username must be 4-26 characters long. You can change the username in options", new[] { "Ok!"}, index => { });
                        }
                        else
                        {
                            ScoreloopManager scoreloopManager = FlaiGame.Current.Services.Get<ScoreloopManager>();
                            scoreloopManager.RenameUser(input, response => { });
                        }
                    });
                }
            }
        }
    }
}
