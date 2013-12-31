using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Messages;

namespace Skypiea.Achievements.Trackers
{
    public class KillEnemiesWithSingleRocket : CbesSingleAchievementTracker
    {
        private readonly int _killsNeeded;
        public KillEnemiesWithSingleRocket(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName, int killsNeeded) 
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<BooleanProgression>(_achievement.Progression);
            _killsNeeded = killsNeeded;

            if (!_achievement.IsUnlocked)
            {
                entityWorld.SubscribeToMessage<RocketExplodedMessage>(this.OnRocketExploded);
            }
        }

        private void OnRocketExploded(RocketExplodedMessage message)
        {
            if (_achievement.IsUnlocked)
            {
                return;
            }

            if (message.KilledZombies.Count >= _killsNeeded)
            {
                _achievement.Progression.Cast<BooleanProgression>().Unlock();
                _entityWorld.UnsubscribeToMessage<RocketExplodedMessage>(this.OnRocketExploded);
            }
        }
    }
}
