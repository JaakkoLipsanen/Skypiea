using Flai;
using Flai.Misc;
using Flai.ScreenManagement;
using Skypiea.Screens;
using Skypiea.Settings;

namespace Skypiea.Misc
{
    public static class RateHelper
    {
        public static void ShowRateMessageIfNeeded(GameScreen asker)
        {
            const int ShowAfterLaunches = 2;
            SkypieaSettingsManager settingsManager = FlaiGame.Current.Services.Get<SkypieaSettingsManager>();
            if (settingsManager.Settings.LaunchCounts >= ShowAfterLaunches && !settingsManager.Settings.IsRateWindowShown)
            {
                // this can cause a small lag-spike, so call it only if the message box really would be shown
                if (!DeviceInfo.IsNetworkAvailable)
                {
                    return;
                }

                asker.ScreenManager.AddScreen(new RateScreen());

                settingsManager.Settings.IsRateWindowShown = true;
                settingsManager.Save();
            }
        }
    }
}
