using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Boosters;

namespace Skypiea.Systems.Zombie.AI
{
    public class RusherZombieAISystem : ComponentProcessingSystem<CRusherZombieAI>
    {
        private const float MinDistanceForRushing = SkypieaConstants.PixelsPerMeter * 6;
        private const float WanderingSpeed = SkypieaConstants.PixelsPerMeter * 1.5f;
        private Entity _player;
        private CPlayerInfo _playerInfo;

        private IBoosterState _boosterState;
        private IZombieStatsProvider _zombieStatsProvider;

        protected internal override int ProcessOrder
        {
            get { return SystemProcessOrder.Update; }
        }

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            _playerInfo = _player.Get<CPlayerInfo>();

            _boosterState = this.EntityWorld.Services.Get<IBoosterState>();
            _zombieStatsProvider = this.EntityWorld.Services.Get<IZombieStatsProvider>();
        }

        public override void Process(UpdateContext updateContext, Entity zombie, CRusherZombieAI rusherAI)
        {
            // should only happen at first frame after zombie has spawned
            if (rusherAI.State == RusherZombieState.None)
            {
                this.GenerateWanderingTarget(rusherAI);
                rusherAI.State = RusherZombieState.Wandering;
            }

            if (rusherAI.State == RusherZombieState.Wandering)
            {
                this.UpdateWandering(updateContext, zombie, rusherAI);
            }
            else if (rusherAI.State == RusherZombieState.WanderingAwayFromPlayer)
            {
                this.UpdateWanderingAwayFromPlayer(updateContext, zombie, rusherAI);
            }
            else if (rusherAI.State == RusherZombieState.Rushing)
            {
                this.UpdateRushing(updateContext, zombie, rusherAI);
            }
            else if (rusherAI.State == RusherZombieState.RushingStun)
            {
                if (rusherAI.RushingStunTimer.HasFinished)
                {
                    rusherAI.NextRushAllowedTimer.Restart();
                    this.GenerateWanderingTarget(rusherAI);
                    rusherAI.State = RusherZombieState.Wandering;
                }
            }
        }

        #region AI States

        private void UpdateWanderingAwayFromPlayer(UpdateContext updateContext, Entity zombie, CRusherZombieAI rusherAI)
        {
            // if player is alive, set the state back to wandering
            if (_playerInfo.IsAlive)
            {
                // what this does is that basically for a short time 
                // after the player has come out of being dead, the rusher can't rush
                rusherAI.NextRushAllowedTimer.Restart();
                this.GenerateWanderingTarget(rusherAI);
                rusherAI.State = RusherZombieState.Wandering;
                return;
            }
            
            float speedMultiplier = BoosterHelper.GetZombieSpeedMultiplier(_boosterState) * _zombieStatsProvider.SpeedMultiplier;
            this.MoveTowardsTarget(zombie, rusherAI, WanderingSpeed * speedMultiplier * updateContext.DeltaSeconds);
        }

        private void UpdateWandering(UpdateContext updateContext, Entity zombie, CRusherZombieAI rusherAI)
        {
            if (!_playerInfo.IsAlive)
            {
                this.GenerateWanderingAwayTarget(rusherAI);
                rusherAI.State = RusherZombieState.WanderingAwayFromPlayer;
                return;
            }
            else if (this.ShouldRush(zombie, rusherAI))
            {
                this.StartRushing(zombie, rusherAI);
                return;
            }

            // if the rusher target is too far away from player, generate new target
            const float AllowedDistanceFromTargetToPlayer = SkypieaConstants.MapHeight * SkypieaConstants.PixelsPerMeter * 0.5f;
            float distanceFromTargetToPlayer = Vector2.Distance(rusherAI.Target, _player.Transform.Position);
            if (distanceFromTargetToPlayer > AllowedDistanceFromTargetToPlayer)
            {
                this.GenerateWanderingTarget(rusherAI);
            }

            // move rusher towards the target
            float speedMultiplier = BoosterHelper.GetZombieSpeedMultiplier(_boosterState) * _zombieStatsProvider.SpeedMultiplier;
            if (this.MoveTowardsTarget(zombie, rusherAI, WanderingSpeed * speedMultiplier * updateContext.DeltaSeconds))
            {
                // target reached -> generate new target
                this.GenerateWanderingTarget(rusherAI);
            }
        }

        private void UpdateRushing(UpdateContext updateContext, Entity zombie, CRusherZombieAI rusherAI)
        {
            const float MaxRushingSpeed = SkypieaConstants.PixelsPerMeter * 12;
            const float RushingAcceleration = SkypieaConstants.PixelsPerMeter * 12;

            // update the rushing speed (it is accelerating)
            rusherAI.RushingSpeed = FlaiMath.Min(MaxRushingSpeed, rusherAI.RushingSpeed + RushingAcceleration * updateContext.DeltaSeconds);

            // move rusher towards target
            float speedMultiplier = BoosterHelper.GetZombieSpeedMultiplier(_boosterState) * _zombieStatsProvider.SpeedMultiplier;
            if (this.MoveTowardsTarget(zombie, rusherAI, rusherAI.RushingSpeed * speedMultiplier * updateContext.DeltaSeconds))
            {
                this.StartRushingStun(rusherAI);
            }
        }

        #endregion

        #region Misc Private Methods

        // returns true if the target is reached
        private bool MoveTowardsTarget(Entity zombie, CRusherZombieAI rusherAI, float movementAmount)
        {
            float distanceToTarget = Vector2.Distance(zombie.Transform.Position, rusherAI.Target);
            if (distanceToTarget < movementAmount)
            {
                zombie.Transform.Position = rusherAI.Target;
                return true;
            }
            else
            {
                // move towards target
                zombie.Transform.Position += Vector2.Normalize(rusherAI.Target - zombie.Transform.Position) * movementAmount;
                zombie.Transform.LookAt(rusherAI.Target);
                return false;
            }
        }

        private bool ShouldRush(Entity zombie, CRusherZombieAI rusherAI)
        {
            float distanceToPlayer = Vector2.Distance(zombie.Transform.Position, _player.Transform.Position);
            return rusherAI.State == RusherZombieState.Wandering && distanceToPlayer < RusherZombieAISystem.MinDistanceForRushing && rusherAI.NextRushAllowedTimer.HasFinished;
        }

        private void StartRushing(Entity zombie, CRusherZombieAI rusherAI)
        {
            // start rushing
            Vector2 target = _player.Transform.Position + _playerInfo.MovementPerSecond * 0.25f; // target where the player would be in 0.25s 
            rusherAI.Target = target - (zombie.Transform.Position - target) * 0.75f; // rush 75% "over" the player
            zombie.Transform.LookAt(rusherAI.Target);

            rusherAI.RushingSpeed = 0;
            rusherAI.State = RusherZombieState.Rushing;
        }

        private void StartRushingStun(CRusherZombieAI rusherAI)
        {
            rusherAI.State = RusherZombieState.RushingStun;
            rusherAI.RushingStunTimer.Restart();
            rusherAI.RushingSpeed = 0;
        }

        private void GenerateWanderingTarget(CRusherZombieAI rusherAI)
        {
            // generate wandering target that is near the player
            const float MaxDistance = MinDistanceForRushing * 1.3f;
            const float MinDistance = MaxDistance / 3f;
            rusherAI.Target = FlaiAlgorithms.GenerateRandomVector2(
                new Range(_player.Transform.Position.X - MaxDistance, _player.Transform.Position.X + MaxDistance),
                new Range(_player.Transform.Position.Y - MaxDistance, _player.Transform.Position.Y + MaxDistance),
                _player.Transform.Position, MinDistance);
        }

        private void GenerateWanderingAwayTarget(CRusherZombieAI rusherAI)
        {
            // such corner case that it's okay if rusher doesn't do anything in this case
            if (rusherAI.Transform.Position == _player.Transform.Position)
            {
                return;
            }
          
            const float MinTargetDistance = 500;
            const float MaxTargetDistance = 750;

            float rotationPlayerToRusher = FlaiMath.GetAngle(rusherAI.Transform.Position - _player.Transform.Position);
            float rotationOffset = Global.Random.NextFloat(-0.25f, 0.25f);

            // generates target that is away from the player with [-0.25, 0.25] rotational offset (radians)
            rusherAI.Target = rusherAI.Transform.Position + FlaiMath.GetAngleVector(rotationPlayerToRusher + rotationOffset)*Global.Random.NextFloat(MinTargetDistance, MaxTargetDistance);
        }

        #endregion
    }
}
