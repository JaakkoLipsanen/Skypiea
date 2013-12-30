using Flai.Misc;
using System.IO;

namespace Skypiea.Settings
{
    public static class SettingsHelper
    {
        private const string SettingsFilePath = "Settings.dat";
        public static SkypieaSettingsManager CreateSettingsManager()
        {
            return new SkypieaSettingsManager(SettingsHelper.SettingsFilePath);
        }
    }

    public class SkypieaSettingsManager : SettingsManager<SkypieaSettings>
    {
        public SkypieaSettingsManager(string filePath) 
            : base(filePath)
        {
        }
    }
}
