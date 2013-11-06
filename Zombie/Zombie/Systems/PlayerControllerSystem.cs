using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model;

namespace Zombie.Systems
{
    public class PlayerControllerSystem : NameProcessingSystem
    {
        private TransformComponent _transform;
        private PlayerInfoComponent _playerInfo;
        private WeaponComponent _weapon;
        private VirtualThumbStickComponent _movementThumbStick;
        private VirtualThumbStickComponent _rotationThumbStick;

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Input; }
        }

        public PlayerControllerSystem()
            : base(EntityTags.Player)
        {
        }

        protected override void Initialize()
        {
            _transform = base.Entity.Get<TransformComponent>();
            _playerInfo = base.Entity.Get<PlayerInfoComponent>();
            _weapon = base.Entity.Get<WeaponComponent>();
            _movementThumbStick = this.EntityWorld.FindEntityByName(EntityTags.MovementThumbStick).Get<VirtualThumbStickComponent>();
            _rotationThumbStick = this.EntityWorld.FindEntityByName(EntityTags.RotationThumbStick).Get<VirtualThumbStickComponent>();
        }

        protected override void Process(UpdateContext updateContext, Entity entity)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            const float Speed = Tile.Size * 4f;

            World world = this.EntityWorld.GetService<World>();
            _transform.Position += _movementThumbStick.ThumbStick.Direction * Speed * updateContext.DeltaSeconds;
            _transform.Position = Vector2.Clamp(_transform.Position, Vector2.Zero, new Vector2(world.Width, world.Height) * Tile.Size);

            if (_rotationThumbStick.ThumbStick.Direction != Vector2.Zero)
            {
                _transform.RotationAsVector = _rotationThumbStick.ThumbStick.Direction;
                if (_weapon.Weapon.CanShoot)
                {
                    _weapon.Weapon.Shoot(this.EntityWorld, _transform); // normalized thumbstick direction
                }
            }
            // If rotation thumbstick is not pressed, then the movement determines the direction
            else if (_movementThumbStick.ThumbStick.Direction != Vector2.Zero)
            {
                _transform.RotationAsVector = _movementThumbStick.ThumbStick.Direction;
            }
        }
    }
}
