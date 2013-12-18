
using System.IO.IsolatedStorage;
using Flai.Achievements;
using Skypiea.Achievements.Trackers;

namespace Skypiea.Achievements
{
    public static class AchievementHelper
    {
        private const string AchievementFilePath = "achievements.dat";
        public static AchievementManager CreateAchivementManager()
        {
            CreateAchievementTrackerDelegate persistentKillZombieTrackerCreator = (am, ew, achievement) => new PersistentKillZombiesTracker(am, ew, achievement.Name);
            CreateAchievementTrackerDelegate persistentRunningTrackerCreator = (am, ew, achievement) => new PersistentRunningTracker(am, ew, achievement.Name);
            PassiveSkill zombieKillReward = new PassiveSkill("Player shoots faster", passiveStats => passiveStats.FireRateMultiplier += 0.1f);
            PassiveSkill runningReward = new PassiveSkill("Player runs faster", passiveStats => passiveStats.MovementSpeedMultiplier += 0.1f);

            Achievement[] achivements =
            {
                new Achievement("Sunday Killer", "Kill 10 zombies", new IntegerProgression(0, 10)) { Tag = new AchievementInfo(persistentKillZombieTrackerCreator, zombieKillReward) },
                new Achievement("Zombie-nator", "Kill 25 zombies", new IntegerProgression(0, 20)) { Tag = new AchievementInfo(persistentKillZombieTrackerCreator, zombieKillReward) },
                new Achievement("Jesus Fucking Christ Antti", "Kill 100 zombies", new IntegerProgression(0, 100)) { Tag = new AchievementInfo(persistentKillZombieTrackerCreator, zombieKillReward) },
          
                new Achievement("Jogger", "Run 50 meters", new FloatProgression(0, 50)) { Tag = new AchievementInfo(persistentRunningTrackerCreator, runningReward) },
                new Achievement("The Marathoner", "Run 200 meters", new FloatProgression(0, 200)) { Tag = new AchievementInfo(persistentRunningTrackerCreator, runningReward) },
                new Achievement("The Flash", "Run 500 meters", new FloatProgression(0, 500)) { Tag = new AchievementInfo(persistentRunningTrackerCreator, runningReward) },
            };

            return new AchievementManager(achivements, AchievementHelper.AchievementFilePath);
        }

        public static void ResetAchievements()
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(AchievementHelper.AchievementFilePath))
                {
                    isolatedStorage.DeleteFile(AchievementHelper.AchievementFilePath);
                }
            }
        }
    }
}
