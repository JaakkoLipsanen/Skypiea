using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.DataStructures;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Boosters;

namespace Skypiea.Systems.Zombie.AI
{
    public class GoldenGoblinAISystem : ProcessingSystem
    {
        private IBoosterState _boosterState;
        private IZombieStatsProvider _zombieStatsProvider;

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Update; }
        }

        public GoldenGoblinAISystem()
            : base(Aspect.All<CGoldenGoblinAI>())
        {
        }

        protected override void Initialize()
        {
            _boosterState = this.EntityWorld.Services.Get<IBoosterState>();
            _zombieStatsProvider = this.EntityWorld.Services.Get<IZombieStatsProvider>();
        }

        protected override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            float speedMultiplier = BoosterHelper.GetZombieSpeedMultiplier(_boosterState) * _zombieStatsProvider.SpeedMultiplier;

            for (int i = 0; i < entities.Count; i++)
            {
                Entity zombie = entities[i];
                CGoldenGoblinAI goldenGoblinAI = zombie.Get<CGoldenGoblinAI>();

                this.UpdateGoblin(updateContext, zombie, speedMultiplier);
            }
        }

        private void UpdateGoblin(UpdateContext updateContext, Entity zombie, float speedMultiplier)
        {
            CGoldenGoblinAI goldenGoblinAI = zombie.Get<CGoldenGoblinAI>();
            if (goldenGoblinAI.State == GoldenGoblinState.TravellingToWaypoint)
            {
                this.UpdateTravelling(updateContext, zombie.Transform, goldenGoblinAI, speedMultiplier);
            }
            else
            {
                goldenGoblinAI.Velocity *= 0.98f;
                zombie.Transform.Position += goldenGoblinAI.Velocity * updateContext.DeltaSeconds;

                if (goldenGoblinAI.WaypointStunTimer.HasFinished)
                {
                    goldenGoblinAI.State = GoldenGoblinState.TravellingToWaypoint;
                }
            }
        }

        private void UpdateTravelling(UpdateContext updateContext, CTransform2D transform, CGoldenGoblinAI goldenGoblinAI, float speedMultiplier)
        {
            const float Speed = SkypieaConstants.PixelsPerMeter * 3.5f;

            // calculate the new velocity
            Vector2 velocity = Vector2.Normalize(goldenGoblinAI.CurrentWaypoint - transform.Position) * Speed * speedMultiplier; // * updateContext.DeltaSeconds;
            goldenGoblinAI.Velocity = FlaiMath.ClampLength(goldenGoblinAI.Velocity + velocity * updateContext.DeltaSeconds, Speed);
            goldenGoblinAI.Velocity += new Vector2(velocity.Y, velocity.X) * FlaiMath.Sin(updateContext.DeltaSeconds * 3) / 8;

            // if the goblin is near enough to a waypoint, move to the next waypoint
            if (Vector2.Distance(transform.Position, goldenGoblinAI.CurrentWaypoint) < SkypieaConstants.PixelsPerMeter * 2)
            {
                goldenGoblinAI.CurrentWaypointIndex++;
                if (goldenGoblinAI.CurrentWaypointIndex >= goldenGoblinAI.Waypoints.Count)
                {
                    goldenGoblinAI.CurrentWaypointIndex = 0;
                }

                // change state to WaypointStun
                goldenGoblinAI.WaypointStunTimer.Restart();
                goldenGoblinAI.State = GoldenGoblinState.WaypointStun;
            }
            else
            {
                // otherwise move to the current waypoint
                transform.Position += goldenGoblinAI.Velocity * updateContext.DeltaSeconds;
            }
        }
    }
}
