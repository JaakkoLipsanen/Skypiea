using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerDropPickupSystem : ComponentProcessingSystem<CDrop>
    {
        private Entity _player;
        private CWeapon _playerWeapon;
        private CPlayerInfo _playerInfo;

        public PlayerDropPickupSystem()
            : base(Aspect.Any<CWeaponDrop, CLifeDrop>())
        {          
        }

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            _playerWeapon = _player.Get<CWeapon>();
            _playerInfo = _player.Get<CPlayerInfo>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, CDrop velocity)
        {
            const float MinDistance = SkypieaConstants.PixelsPerMeter;
            if (Vector2.Distance(_player.Transform.Position, entity.Transform.Position) < MinDistance)
            {
                if (velocity.DropType == DropType.Weapon)
                {
                    this.OnWeaponDropPicked(entity);
                }
                else
                {
                    this.OnLifeDropPicked(entity);
                }
            }
        }

        private void OnWeaponDropPicked(Entity entity)
        {
            CWeaponDrop weaponDrop = entity.Get<CWeaponDrop>();
            if (weaponDrop.Type == _playerWeapon.Weapon.Type)
            {
                _playerWeapon.Weapon.OnNewInstancePickedUp(); // awful name
            }
            else
            {
                _playerWeapon.Weapon = weaponDrop.BuildWeapon();
            }

            entity.Delete();
        }

        private void OnLifeDropPicked(Entity entity)
        {
            _playerInfo.AddLife();
            entity.Delete();
        }
    }
}
