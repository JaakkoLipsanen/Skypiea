using Flai;
using Flai.Misc;
using Skypiea.Settings;

namespace Skypiea.Misc
{
    public static class RateHelper
    {
        public static void ShowRateMessageIfNeeded()
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

                GuideHelper.ShowMessageBox("Rate the game", "Please rate the game if you like it! Rating helps us make the game even better!", new[] { "rate now!", "no thanks" }, result =>
                {
                    // result == null -> the message box was canceled (back button)
                    if (result != null)
                    {
                        settingsManager.Settings.IsRateWindowShown = true;
                        settingsManager.Save();

                        // result == 0 -> "rate now!" pressed
                        if (result == 0)
                        {
                            ApplicationInfo.OpenApplicationReviewPage();
                        }

                        // else "no thanks" was pressed
                    }
                });
            }
        }
    }
}
