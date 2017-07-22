
namespace Flai.Achievements
{
    public static class AchievementExtensions
    {
        public static void InvokeIfNotNull(this AchievementUnlockedDelegate achievementUnlockedDelegate, Achievement achievement)
        {
            if (achievementUnlockedDelegate != null)
            {
                achievementUnlockedDelegate(achievement);
            }
        }
    }
}
