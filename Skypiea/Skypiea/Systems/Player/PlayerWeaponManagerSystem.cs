using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.Systems.Player
{
    public class PlayerWeaponManagerSystem : NameProcessingSystem
    {
        private CWeapon _weapon;
        public PlayerWeaponManagerSystem()
            : base(EntityNames.Player)
        {
        }

        protected override void Initialize()
        {
            _weapon = base.Entity.Get<CWeapon>();
        }

        protected override void Process(UpdateContext updateContext, Entity entity)
        {
            if (_weapon.Weapon.AmmoRemaining == 0)
            {
                _weapon.Weapon = WeaponFactory.CreateDefaultWeapon();
            }
        }
    }
}
