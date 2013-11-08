using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model;

namespace Zombie.Systems
{
    public class PlayerWeaponDropPickupSystem : ComponentProcessingSystem<WeaponDropComponent, TransformComponent>
    {
        private Entity _player;
        private TransformComponent _playerTransform;
        private WeaponComponent _playerWeapon;

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityTags.Player);
            _playerTransform = _player.Get<TransformComponent>();
            _playerWeapon = _player.Get<WeaponComponent>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, WeaponDropComponent weaponDrop, TransformComponent transform)
        {
            const float MinDistance = Tile.Size;
            if (Vector2.Distance(_playerTransform.Position, transform.Position) < MinDistance)
            {
                _playerWeapon.Weapon = weaponDrop.BuildWeapon();
                entity.Delete();
            }
        }
    }
}
