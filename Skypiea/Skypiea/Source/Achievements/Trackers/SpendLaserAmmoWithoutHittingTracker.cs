using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.Achievements.Trackers
{
    public class SpendLaserAmmoWithoutHittingTracker : CbesSingleAchievementTracker
    {
        private bool _isActive = false;
        public SpendLaserAmmoWithoutHittingTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName) 
            : base(achievementManager, entityWorld, achievementName)
        {
            Ensure.Is<BooleanProgression>(_achievement.Progression);
            if (!_achievement.IsUnlocked)
            {
                entityWorld.SubscribeToMessage<WeaponChangedMessage>(this.OnWeaponChanged);
                entityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            }

            // if the player starts with laser, then the tracking should be active from the start
            if (entityWorld.FindEntityByName(EntityNames.Player).Get<CWeapon>().Weapon.Type == WeaponType.Laser)
            {
                _isActive = true;
            }
        }

        private void OnWeaponChanged(WeaponChangedMessage message)
        {
            if (message.OldWeapon.Type != WeaponType.Laser && message.NewWeapon.Type == WeaponType.Laser)
            {
                _isActive = true;
            }
            else if (_isActive && message.OldWeapon.Type == WeaponType.Laser && message.OldWeapon.AmmoRemaining <= 0)
            {
                _achievement.Progression.Cast<BooleanProgression>().Unlock();
                _entityWorld.UnsubscribeToMessage<WeaponChangedMessage>(this.OnWeaponChanged);
                _entityWorld.UnsubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            }
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            _isActive = false;
        }
    }
}
