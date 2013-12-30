using Flai;
using Flai.Achievements;
using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Misc;

namespace Skypiea.Achievements.Trackers
{
    public class PersistentRunningTracker : CbesSingleAchievementTracker
    {
        private readonly FloatProgression _progression;
        private readonly CTransform2D _playerTransform;
        private Vector2 _previousPosition;

        public PersistentRunningTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName)
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<FloatProgression>(_achievement.Progression);
            _progression = (FloatProgression)_achievement.Progression;
            _playerTransform = _entityWorld.FindEntityByName(EntityNames.Player).Transform;
            _previousPosition = _playerTransform.Position;
        }

        public override void Update(UpdateContext updateContext)
        {
            if (!_achievement.IsUnlocked)
            {
                _progression.Current = FlaiMath.Min(_progression.Max, _progression.Current + Vector2.Distance(_previousPosition, _playerTransform.Position) / SkypieaConstants.PixelsPerMeter);
                _previousPosition = _playerTransform.Position;
            }
        }
    }
}
