using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model.Weapons;

namespace Zombie.Systems
{
    public class PlayerWeaponManagerSystem : NameProcessingSystem
    {
        private WeaponComponent _weaponComponent;
        public PlayerWeaponManagerSystem()
            : base(EntityTags.Player)
        {
        }

        protected override void Initialize()
        {
            _weaponComponent = base.Entity.Get<WeaponComponent>();
        }

        protected override void Process(UpdateContext updateContext, Entity entity)
        {
            if (_weaponComponent.Weapon.BulletsRemaining == 0)
            {
                _weaponComponent.Weapon = WeaponFactory.CreateDefaultWeapon();
            }
        }
    }
}
