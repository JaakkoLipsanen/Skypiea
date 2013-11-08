using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model;

namespace Zombie.Systems
{
    public class RusherZombieAISystem : ComponentProcessingSystem<TransformComponent, RusherZombieAIComponent>
    {
        private const float MinDistanceForRushing = Tile.Size * 6;
        private Entity _player;
        private TransformComponent _playerTransform;

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityTags.Player);
            _playerTransform = _player.Get<TransformComponent>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, TransformComponent transform, RusherZombieAIComponent rusherAi)
        {
            // should only happen at first frame after zombie has spawned
            if (rusherAi.State == RusherZombieState.None)
            {
                this.GenerateWanderingTarget(rusherAi);
                rusherAi.State = RusherZombieState.Wandering;
            }

            if (rusherAi.State == RusherZombieState.Wandering)
            {
                this.UpdateWandering(updateContext, entity, transform, rusherAi);
            }
            else if (rusherAi.State == RusherZombieState.Rushing)
            {
                this.UpdateRushing(updateContext, entity, transform, rusherAi);
            }
            else if (rusherAi.State == RusherZombieState.RushingStun)
            {
                if (rusherAi.RushingStunTimer.HasFinished)
                {
                    rusherAi.State = RusherZombieState.Wandering;
                    rusherAi.NextRushAllowedTimer.Restart();
                    this.GenerateWanderingTarget(rusherAi);
                }
            }
        }

        private void UpdateWandering(UpdateContext updateContext, Entity entity, TransformComponent transform, RusherZombieAIComponent rusherAi)
        {
            const float Speed = Tile.Size * 1.5f;
          
            if (Vector2.Distance(transform.Position, _playerTransform.Position) < RusherZombieAISystem.MinDistanceForRushing && rusherAi.NextRushAllowedTimer.HasFinished)
            {
                rusherAi.Target = _playerTransform.Position - (transform.Position - _playerTransform.Position) / 4f * 3f; // rush 75% "over" the player
                transform.LookAt(rusherAi.Target);
                rusherAi.State = RusherZombieState.Rushing;
                return;
            }
            
            float movement = Speed*updateContext.DeltaSeconds;
            if(Vector2.Distance(transform.Position, rusherAi.Target) < movement)
            {
                transform.Position = rusherAi.Target;
                this.GenerateWanderingTarget(rusherAi);
            }
            else
            {
                transform.Position -= Vector2.Normalize(transform.Position - rusherAi.Target);
                transform.LookAt(rusherAi.Target);
            }
        }

        private void UpdateRushing(UpdateContext updateContext, Entity entity, TransformComponent transform, RusherZombieAIComponent rusherAi)
        {
            const float RushingSpeed = Tile.Size * 4;
            float movement = RushingSpeed * updateContext.DeltaSeconds;
            if (Vector2.Distance(transform.Position, rusherAi.Target) < movement)
            {
                transform.Position = rusherAi.Target;

                rusherAi.State = RusherZombieState.RushingStun;
                rusherAi.RushingStunTimer.Restart();
            }
            else
            {
                transform.Position += Vector2.Normalize(rusherAi.Target - transform.Position) * movement;
            }
        }

        private void GenerateWanderingTarget(RusherZombieAIComponent rusherAi)
        {
            const float MaxDistance = MinDistanceForRushing * 1.3f;
            const float MinDistance = MaxDistance / 2f;
            rusherAi.Target = FlaiAlgorithms.GenerateRandomVector2(
                new Range(_playerTransform.Position.X - MaxDistance, _playerTransform.Position.X + MaxDistance),
                new Range(_playerTransform.Position.Y - MaxDistance, _playerTransform.Position.Y + MaxDistance),
                _playerTransform.Position, MinDistance);
        }
    }
}
