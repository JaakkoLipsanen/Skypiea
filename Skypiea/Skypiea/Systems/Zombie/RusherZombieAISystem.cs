using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Zombie
{
    public class RusherZombieAISystem : ComponentProcessingSystem<CRusherZombieAI>
    {
        private const float MinDistanceForRushing = Tile.Size * 6;
        private Entity _player;

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
        }

        public override void Process(UpdateContext updateContext, Entity entity, CRusherZombieAI rusherAI)
        {
            // should only happen at first frame after zombie has spawned
            if (rusherAI.State == RusherZombieState.None)
            {
                this.GenerateWanderingTarget(rusherAI);
                rusherAI.State = RusherZombieState.Wandering;
            }

            if (rusherAI.State == RusherZombieState.Wandering)
            {
                this.UpdateWandering(updateContext, entity, rusherAI);
            }
            else if (rusherAI.State == RusherZombieState.Rushing)
            {
                this.UpdateRushing(updateContext, entity, rusherAI);
            }
            else if (rusherAI.State == RusherZombieState.RushingStun)
            {
                if (rusherAI.RushingStunTimer.HasFinished)
                {
                    rusherAI.State = RusherZombieState.Wandering;
                    rusherAI.NextRushAllowedTimer.Restart();
                    this.GenerateWanderingTarget(rusherAI);
                }
            }
        }

        private void UpdateWandering(UpdateContext updateContext, Entity entity, CRusherZombieAI rusherAI)
        {
            const float Speed = Tile.Size * 1.5f;
            if (Vector2.Distance(entity.Transform.Position, _player.Transform.Position) < RusherZombieAISystem.MinDistanceForRushing && rusherAI.NextRushAllowedTimer.HasFinished)
            {
                rusherAI.Target = _player.Transform.Position - (entity.Transform.Position - _player.Transform.Position) / 4f * 3f; // rush 75% "over" the player
                entity.Transform.LookAt(rusherAI.Target);
                rusherAI.State = RusherZombieState.Rushing;
                return;
            }

            float movement = Speed * updateContext.DeltaSeconds;
            if (Vector2.Distance(entity.Transform.Position, rusherAI.Target) < movement)
            {
                entity.Transform.Position = rusherAI.Target;
                this.GenerateWanderingTarget(rusherAI);
            }
            else
            {
                entity.Transform.Position -= Vector2.Normalize(entity.Transform.Position - rusherAI.Target);
                entity.Transform.LookAt(rusherAI.Target);
            }
        }

        private void UpdateRushing(UpdateContext updateContext, Entity entity, CRusherZombieAI rusherAI)
        {
            const float RushingSpeed = Tile.Size * 8;
            float movement = RushingSpeed * updateContext.DeltaSeconds;
            if (Vector2.Distance(entity.Transform.Position, rusherAI.Target) < movement)
            {
                entity.Transform.Position = rusherAI.Target;

                rusherAI.State = RusherZombieState.RushingStun;
                rusherAI.RushingStunTimer.Restart();
            }
            else
            {
                entity.Transform.Position += Vector2.Normalize(rusherAI.Target - entity.Transform.Position) * movement;
            }
        }

        private void GenerateWanderingTarget(CRusherZombieAI rusherAI)
        {
            const float MaxDistance = MinDistanceForRushing * 1.3f;
            const float MinDistance = MaxDistance / 2f;
            rusherAI.Target = FlaiAlgorithms.GenerateRandomVector2(
                new Range(_player.Transform.Position.X - MaxDistance, _player.Transform.Position.X + MaxDistance),
                new Range(_player.Transform.Position.Y - MaxDistance, _player.Transform.Position.Y + MaxDistance),
                _player.Transform.Position, MinDistance);
        }
    }
}
