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

        protected override void Process(UpdateContext updateContext, Entity player)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            const float Speed = Tile.Size * 4.5f;
            const float OffsetFromBorder = -Tile.Size;

            World world = this.EntityWorld.Services.Get<World>();

            // movement
            Vector2 previousPosition = player.Transform.Position;
            player.Transform.Position += _movementThumbstick.ThumbStick.Direction * Speed * updateContext.DeltaSeconds;
            player.Transform.Position = Vector2.Clamp(player.Transform.Position, Vector2.One * OffsetFromBorder, new Vector2(world.Width, world.Height) * Tile.Size - Vector2.One * OffsetFromBorder);
            _playerInfo.MovementVector = (updateContext.DeltaSeconds == 0) ? Vector2.Zero : (player.Transform.Position - previousPosition) / updateContext.DeltaSeconds;

            // rotation
            if (_rotationThumbstick.ThumbStick.Direction != Vector2.Zero)
            {
                player.Transform.RotationVector = _rotationThumbstick.ThumbStick.Direction;
                if (_weapon.Weapon.CanShoot)
                {
                    _weapon.Weapon.Shoot(updateContext, player); // normalized thumbstick direction
                }
            }
            // If rotation thumbstick is not pressed, then the movement determines the direction
            else if (_movementThumbstick.ThumbStick.Direction != Vector2.Zero)
            {
                player.Transform.RotationVector = _movementThumbstick.ThumbStick.Direction;
            }
        }
    }
}
