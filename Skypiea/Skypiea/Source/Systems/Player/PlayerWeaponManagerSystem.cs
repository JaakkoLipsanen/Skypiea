using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.General;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model;
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

            IPlayerPassiveStats passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
            if (passiveStats.SpawnWithRandomWeapon)
            {
                _weapon.Weapon = WeaponFactory.CreateWeapon(EnumHelper.GetRandom<WeaponType>(Global.Random), passiveStats.AmmoMultiplier);
            }
        }

        protected override void Process(UpdateContext updateContext, Entity player)
        {
            if (_weapon.Weapon.AmmoRemaining == 0)
            {
                WeaponChangedMessage message = this.EntityWorld.FetchMessage<WeaponChangedMessage>();
                message.Initialize(_weapon.Weapon, WeaponFactory.CreateDefaultWeapon());
                _weapon.Weapon = message.NewWeapon;

                this.EntityWorld.BroadcastMessage(message);
            }
        }
    }
}
