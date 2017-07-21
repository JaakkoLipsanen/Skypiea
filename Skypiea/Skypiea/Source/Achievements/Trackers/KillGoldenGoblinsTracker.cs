using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Prefabs;

namespace Skypiea.Achievements.Trackers
{
    public class KillGoldenGoblinsTracker : CbesSingleAchievementTracker
    {
        public KillGoldenGoblinsTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName)
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<IntegerProgression>(_achievement.Progression);
            if (!_achievement.IsUnlocked)
            {
                entityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            }
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            if (message.Zombie.Get<CZombieInfo>().Type == ZombieType.GoldenGoblin)
            {
                _achievement.Progression.As<IntegerProgression>().Current++;
                if (_achievement.IsUnlocked)
                {
                    _entityWorld.UnsubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
                }
            }
        }
    }
}
