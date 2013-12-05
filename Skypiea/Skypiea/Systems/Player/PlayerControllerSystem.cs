using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerControllerSystem : NameProcessingSystem
    {
        private CPlayerInfo _playerInfo;
        private CWeapon _weapon;
        private CVirtualThumbstick _movementThumbstick;
        private CVirtualThumbstick _rotationThumbstick;

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Input; }
        }

        public PlayerControllerSystem()
            : base(EntityNames.Player)
        {
        }

        protected override void Initialize()
        {
            _playerInfo = base.Entity.Get<CPlayerInfo>();
            _weapon = base.Entity.Get<CWeapon>();
            _movementThumbstick = this.EntityWorld.FindEntityByName(EntityNames.MovementThumbStick).Get<CVirtualThumbstick>();
            _rotationThumbstick = this.EntityWorld.FindEntityByName(EntityNames.RotationThumbStick).Get<CVirtualThumbstick>();
        }

        protected override void Process(UpdateContext updateContext, Entity entity)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            const float Speed = Tile.Size * 4f;

            World world = this.EntityWorld.Services.Get<World>();
            entity.Transform.Position += _movementThumbstick.ThumbStick.Direction * Speed * updateContext.DeltaSeconds;
            entity.Transform.Position = Vector2.Clamp(entity.Transform.Position, Vector2.Zero, new Vector2(world.Width, world.Height) * Tile.Size);

            if (_rotationThumbstick.ThumbStick.Direction != Vector2.Zero)
            {
                entity.Transform.RotationVector = _rotationThumbstick.ThumbStick.Direction;
                if (_weapon.Weapon.CanShoot)
                {
                    _weapon.Weapon.Shoot(updateContext, entity); // normalized thumbstick direction
                }
            }
            // If rotation thumbstick is not pressed, then the movement determines the direction
            else if (_movementThumbstick.ThumbStick.Direction != Vector2.Zero)
            {
                entity.Transform.RotationVector = _movementThumbstick.ThumbStick.Direction;
            }
        }
    }
}
