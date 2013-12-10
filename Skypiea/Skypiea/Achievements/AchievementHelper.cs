
using Flai.Achievements;
namespace Skypiea.Achievements
{
    public static class AchievementHelper
    {
        private const string AchievementFilePath = "achievements.dat";
        public static AchievementManager CreateAchivementManager()
        {
            Achievement[] achivements =
            {
                new Achievement("Sunday Killer", "Kill 10 zombies", new IntegerProgression(0, 10)),
                new Achievement("Zombie-nator", "Kill 25 zombies", new IntegerProgression(0, 20)),
                new Achievement("Jesus Fucking Christ Antti", "Kill 100 zombies", new IntegerProgression(0, 100)),
          
                new Achievement("Jogger", "Run 50 meters", new FloatProgression(0, 50)),
                new Achievement("The Marathoner", "Run 200 meters", new FloatProgression(0, 200)),
                new Achievement("The Flash", "Run 500 meters", new FloatProgression(0, 500)),
            };

            return new AchievementManager(achivements, AchievementHelper.AchievementFilePath);
        }
    }
}
