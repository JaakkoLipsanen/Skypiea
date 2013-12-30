using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Achievements.Trackers
{
    public class LivesAtAnyPointOfGameTracker : CbesSingleAchievementTracker
    {
        private readonly int _lives;
        private readonly CPlayerInfo _playerInfo;
        public LivesAtAnyPointOfGameTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName, int lives) 
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<BooleanProgression>(_achievement.Progression);
            _lives = lives;
            _playerInfo = entityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
        }

        public override void Update(UpdateContext updateContext)
        {
            if (_achievement.IsUnlocked)
            {
                return;
            }

            if (_playerInfo.LivesRemaining >= _lives)
            {
                _achievement.Progression.Cast<BooleanProgression>().Unlock();
            }
        }
    }
}
