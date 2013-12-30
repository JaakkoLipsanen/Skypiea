using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Boosters;

namespace Skypiea.Systems.Player
{
    public class PlayerControllerSystem : NameProcessingSystem
    {
        private CPlayerInfo _playerInfo;
        private CWeapon _weapon;
        private VirtualThumbstick _movementThumbstick;
        private VirtualThumbstick _rotationThumbstick;
        private IBoosterState _boosterState;
        private IPlayerPassiveStats _playerPassiveStats;

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
            _movementThumbstick = this.EntityWorld.FindEntityByName(EntityNames.MovementThumbStick).Get<CVirtualThumbstick>().Thumbstick;
            _rotationThumbstick = this.EntityWorld.FindEntityByName(EntityNames.RotationThumbStick).Get<CVirtualThumbstick>().Thumbstick;

            _boosterState = this.EntityWorld.Services.Get<IBoosterState>();
            _playerPassiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
        }

        protected override void Process(UpdateContext updateContext, Entity player)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

        //  player.Transform.Position = SkypieaConstants.MapSizeInPixels / 2f + (Vector2.UnitX * 400).Rotate(updateContext.TotalSeconds / 4f);
          
            const float PositionClampOffset = -SkypieaConstants.PixelsPerMeter;
            const float DefaultSpeed = SkypieaConstants.PixelsPerMeter * 4.5f;

            float boosterMovementSpeedMultiplier = BoosterHelper.GetPlayerSpeedMultiplier(_boosterState);
            float passiveMovementSpeedMultiplier = _playerPassiveStats.MovementSpeedMultiplier;
            float speed = DefaultSpeed * boosterMovementSpeedMultiplier * passiveMovementSpeedMultiplier;

            // movement
            Vector2 previousPosition = player.Transform.Position;
            player.Transform.Position += _movementThumbstick.Direction.GetValueOrDefault() * speed * updateContext.DeltaSeconds;
            player.Transform.Position = Vector2.Clamp(player.Transform.Position, Vector2.One * PositionClampOffset, SkypieaConstants.MapSizeInPixels - Vector2.One * PositionClampOffset);
            _playerInfo.MovementVector = (updateContext.DeltaSeconds == 0) ? Vector2.Zero : (player.Transform.Position - previousPosition) / updateContext.DeltaSeconds;

            // rotation
            if (_rotationThumbstick.Direction.HasValue && _rotationThumbstick.Direction != Vector2.Zero)
            {
                player.Transform.RotationVector = FlaiMath.NormalizeOrZero(_rotationThumbstick.Direction.GetValueOrDefault());
                if (_weapon.Weapon.CanShoot)
                {
                    _weapon.Weapon.Shoot(updateContext, player); // normalized thumbstick direction
                }
            }
            // If rotation thumbstick is not pressed, then the movement determines the direction
            else if (_movementThumbstick.Direction.HasValue && _movementThumbstick.Direction != Vector2.Zero)
            {
                player.Transform.RotationVector = FlaiMath.NormalizeOrZero(_movementThumbstick.Direction.GetValueOrDefault());
            }
        }
    }
}
