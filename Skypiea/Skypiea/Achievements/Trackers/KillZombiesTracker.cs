using System.Diagnostics;
using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Messages;

namespace Skypiea.Achievements.Trackers
{
    public class PersistentKillZombiesTracker : CbesSingleAchievementTracker
    {
        public PersistentKillZombiesTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName)
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
            _achievement.Progression.Cast<IntegerProgression>().Current++;
            if (_achievement.IsUnlocked)
            {
                _entityWorld.UnsubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            }
        }
    }
}
