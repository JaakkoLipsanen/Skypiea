using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Achievements.Trackers
{
    public class ScoreTracker : CbesSingleAchievementTracker
    {
        private readonly int _scoreTarget;
        private readonly CPlayerInfo _playerInfo;
        public ScoreTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName, int scoreTarget) 
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<BooleanProgression>(_achievement.Progression);
            _scoreTarget = scoreTarget;
            _playerInfo = entityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
        }

        public override void Update(UpdateContext updateContext)
        {
            if (_achievement.IsUnlocked)
            {
                return;
            }

            if (_playerInfo.Score >= _scoreTarget)
            {
                _achievement.Progression.Cast<BooleanProgression>().Unlock();
            }
        }
    }
}
