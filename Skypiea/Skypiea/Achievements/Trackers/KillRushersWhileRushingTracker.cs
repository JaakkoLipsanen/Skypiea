using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Model;

namespace Skypiea.Achievements.Trackers
{
    public class KillRushersWhileRushingTracker : CbesSingleAchievementTracker
    {
        public KillRushersWhileRushingTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName) 
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
            CZombieInfo zombieInfo = message.Zombie.Get<CZombieInfo>();
            if (zombieInfo.Type == ZombieType.Rusher)
            {
                CRusherZombieAI rusherAI = message.Zombie.Get<CRusherZombieAI>();
                if (rusherAI.State == RusherZombieState.Rushing)
                {
                    _achievement.Progression.Cast<IntegerProgression>().Current++;
                    if (_achievement.IsUnlocked)
                    {
                        _entityWorld.UnsubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
                    }
                }
            }
        }
    }
}
