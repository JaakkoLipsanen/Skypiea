using Flai;
using Flai.Achievements;
using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Misc;

namespace Skypiea.Achievements.Trackers
{
    public class SurviveWithoutMovingTracker : CbesSingleAchievementTracker
    {
        private readonly float _targetSeconds;
        private readonly CTransform2D _playerTransform;

        private Vector2 _previousPosition;
        private float _currentSeconds;

        public SurviveWithoutMovingTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName, float seconds)
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<BooleanProgression>(_achievement.Progression);
            _targetSeconds = seconds;
            _playerTransform = entityWorld.FindEntityByName(EntityNames.Player).Transform;
            _previousPosition = _playerTransform.Position;
        }

        public override void Update(UpdateContext updateContext)
        {
            if (_achievement.IsUnlocked)
            {
                return;
            }

            const float MaxAllowedMovement = SkypieaConstants.PixelsPerMeter / 60f / 5f;
            if (Vector2.Distance(_previousPosition, _playerTransform.Position) > MaxAllowedMovement)
            {
                _currentSeconds = 0;
            }

            _previousPosition = _playerTransform.Position;
            _currentSeconds += updateContext.DeltaSeconds;
            if (_currentSeconds >= _targetSeconds)
            {
                _achievement.Progression.Cast<BooleanProgression>().Unlock();
            }
        }
    }
}
