using System.Collections.Generic;
using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Messages;

namespace Skypiea.Achievements.Trackers
{
    public class KillZombiesInTimeTracker : CbesSingleAchievementTracker
    {
        private readonly int _count;
        private readonly float _time;
        private readonly Queue<float> _killTimes = new Queue<float>();
 
        private float _currentTime;

        public KillZombiesInTimeTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName, int count, float time) 
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<BooleanProgression>(_achievement.Progression); // no progression because not persistent
            _count = count;
            _time = time;

            if (!_achievement.IsUnlocked)
            {
                entityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            }
        }

        public override void Update(UpdateContext updateContext)
        {
            if (_achievement.IsUnlocked)
            {
                return;
            }

            _currentTime += updateContext.DeltaSeconds;
            while (_killTimes.Count != 0 && _killTimes.Peek() < _currentTime - _time)
            {
                _killTimes.Dequeue();
            }
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            _killTimes.Enqueue(_currentTime);
            if (_killTimes.Count >= _count)
            {
                _achievement.Progression.Cast<BooleanProgression>().Unlock();
                _entityWorld.UnsubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            }
        }
    }
}
