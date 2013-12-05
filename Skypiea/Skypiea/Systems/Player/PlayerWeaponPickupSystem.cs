using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerWeaponPickupSystem : ComponentProcessingSystem<CWeaponDrop>
    {
        private Entity _player;
        private CWeapon _playerWeapon;

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            _playerWeapon = _player.Get<CWeapon>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, CWeaponDrop weaponDrop)
        {
            const float MinDistance = Tile.Size;
            if (Vector2.Distance(_player.Transform.Position, entity.Transform.Position) < MinDistance)
            {
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
        }
    }
}
