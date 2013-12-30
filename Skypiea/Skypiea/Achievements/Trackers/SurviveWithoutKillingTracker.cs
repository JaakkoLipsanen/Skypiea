using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Messages;

namespace Skypiea.Achievements.Trackers
{
    public class SurviveWithoutKillingTracker : CbesSingleAchievementTracker
    {
        private readonly float _time;
        private float _currentTime = 0f;
        private float _lastKillTime = 0f;

        public SurviveWithoutKillingTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName, float time)
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<BooleanProgression>(_achievement.Progression);
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
            if (_currentTime - _lastKillTime > _time)
            {
                _achievement.Progression.Cast<BooleanProgression>().Unlock();
            }
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            _lastKillTime = _currentTime;
        }
    }
}
