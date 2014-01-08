using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.DataStructures;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Boosters;

namespace Skypiea.Systems.Zombie
{
    public class BasicZombieAISystem : ProcessingSystem
    {
        private IBoosterState _boosterState;
        private IZombieStatsProvider _zombieStatsProvider;

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Update; }
        }

        public BasicZombieAISystem()
            : base(Aspect.All<CBasicZombieAI>())
        {
        }

        protected override void Initialize()
        {
            _boosterState = this.EntityWorld.Services.Get<IBoosterState>();
            _zombieStatsProvider = this.EntityWorld.Services.Get<IZombieStatsProvider>();
        }

        protected override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            Entity player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            CPlayerInfo playerInfo = player.Get<CPlayerInfo>();
            float speedMultiplier = BoosterHelper.GetZombieSpeedMultiplier(_boosterState) * _zombieStatsProvider.SpeedMultiplier;

            bool isFleeing = !playerInfo.IsAlive;
            for (int i = 0; i < entities.Count; i++)
            {
                Entity zombieEntity = entities[i];

                Vector2 targetVelocity = ((player.Transform.Position - zombieEntity.Transform.Position) * (isFleeing ? -1 : 1)).NormalizeOrZero();
                Vector2 separationVelocity = this.GetSeparationVector(zombieEntity, entities) * 1f;

                float speed = zombieEntity.Get<CBasicZombieAI>().Speed * speedMultiplier * (isFleeing ? 0.75f : 1);
                Vector2 finalVelocity = Vector2.Normalize(targetVelocity + separationVelocity) * speed * updateContext.DeltaSeconds;
                zombieEntity.Transform.Position += finalVelocity;
                zombieEntity.Transform.RotationVector = targetVelocity;
            }
        }

        // this is still not perfect and has some "jittering". also normal zombies don't react to fats or vice versa..
        private Vector2 GetSeparationVector(Entity targetZombie, ReadOnlyBag<Entity> entities)
        {
            const float SeparationDistance = 52;
            CZombieInfo zombieInfo = targetZombie.Get<CZombieInfo>();

            Vector2 nearbyDistanceSum = Vector2.Zero;
            IZombieSpatialMap zombieSpatialMap = this.EntityWorld.Services.Get<IZombieSpatialMap>();
            foreach (Entity other in zombieSpatialMap.GetZombiesWithCenterInRange(targetZombie.Transform, SeparationDistance)) // meh.. using this "CenterInRange" ignores the size of the zombie...
            {
                if (other.Get<CZombieInfo>().Type != zombieInfo.Type || other == targetZombie)
                {
                    continue;
                }

                if (targetZombie.Transform.Position != other.Transform.Position)
                {
                    float distance = Vector2.Distance(targetZombie.Transform.Position, other.Transform.Position);
                    float inverseNormalizedDistance = 1 - distance / SeparationDistance;
                    nearbyDistanceSum += Vector2.Normalize(targetZombie.Transform.Position - other.Transform.Position) * inverseNormalizedDistance * inverseNormalizedDistance;
                }
            }

            return (nearbyDistanceSum == Vector2.Zero) ? Vector2.Zero : nearbyDistanceSum * 12.5f;
        }
    }
}
