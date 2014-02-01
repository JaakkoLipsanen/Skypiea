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
        private CTransform2D _playerTransform;
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
            _playerTransform = this.EntityWorld.FindEntityByName(EntityNames.Player).Transform;
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
            if (goldenGoblinAI.State != GoldenGoblinState.EscapingFromPlayer && (this.HasBeenHit(zombie) || this.IsPlayerTooClose(zombie)))
            {
                goldenGoblinAI.State = GoldenGoblinState.EscapingFromPlayer;
            }

            if (goldenGoblinAI.State == GoldenGoblinState.TravellingToWaypoint)
            {
                this.UpdateTravelling(updateContext, zombie.Transform, goldenGoblinAI, speedMultiplier);
            }
            else if (goldenGoblinAI.State == GoldenGoblinState.WaypointStun)
            {
                this.UpdateWaypointStun(updateContext, goldenGoblinAI, zombie.Transform);
            }
            else if (goldenGoblinAI.State == GoldenGoblinState.EscapingFromPlayer)
            {
                this.UpdateEscaping(updateContext, goldenGoblinAI, zombie.Transform, speedMultiplier);
            }
        }

        private void UpdateTravelling(UpdateContext updateContext, CTransform2D transform, CGoldenGoblinAI goldenGoblinAI, float speedMultiplier)
        {
            const float Speed = SkypieaConstants.PixelsPerMeter * 3.5f;

            // calculate the new velocity
            Vector2 velocity = FlaiMath.NormalizeOrZero(goldenGoblinAI.CurrentWaypoint - transform.Position) * Speed * speedMultiplier; // * updateContext.DeltaSeconds;
            goldenGoblinAI.Velocity = FlaiMath.ClampLength(goldenGoblinAI.Velocity + velocity * updateContext.DeltaSeconds, Speed);
            goldenGoblinAI.Velocity += new Vector2(velocity.Y, velocity.X) * FlaiMath.Sin(updateContext.DeltaSeconds * 3) / 8;

            // if the goblin is near enough to a waypoint, move to the next waypoint
            if (Vector2.Distance(transform.Position, goldenGoblinAI.CurrentWaypoint) < SkypieaConstants.PixelsPerMeter * 2)
            {
                // if the current waypoint is the last one, then change the state to escape
                if (goldenGoblinAI.CurrentWaypointIndex - 1 >= goldenGoblinAI.Waypoints.Count)
                {
                    goldenGoblinAI.State = GoldenGoblinState.EscapingFromPlayer;
                    return;
                }

                // change state to WaypointStun
                goldenGoblinAI.CurrentWaypointIndex++;
                goldenGoblinAI.WaypointStunTimer.Restart();
                goldenGoblinAI.State = GoldenGoblinState.WaypointStun;
            }
            else
            {
                // otherwise move to the current waypoint
                transform.Position += goldenGoblinAI.Velocity * updateContext.DeltaSeconds;
                transform.RotationVector = goldenGoblinAI.Velocity;
            }
        }

        // waypoint stun is atm 0 so this code doesn't even matter at all. but it should work just fine
        private void UpdateWaypointStun(UpdateContext updateContext, CGoldenGoblinAI goldenGoblinAI, CTransform2D transform)
        {
            // this is frame-rate dependent but shouldnt matter too much
            goldenGoblinAI.Velocity *= 0.98f;
            transform.Position += goldenGoblinAI.Velocity * updateContext.DeltaSeconds;

            if (goldenGoblinAI.WaypointStunTimer.HasFinished)
            {
                goldenGoblinAI.State = GoldenGoblinState.TravellingToWaypoint;
            }
        }

        private void UpdateEscaping(UpdateContext updateContext, CGoldenGoblinAI goldenGoblinAI, CTransform2D transform, float speedMultiplier)
        {
            // Escape speed is a bit faster than normal speed (3.5f)
            const float Speed = SkypieaConstants.PixelsPerMeter * 4f;

            Vector2 velocity = FlaiMath.NormalizeOrZero(transform.Position - _playerTransform.Transform.Position) * Speed * speedMultiplier * updateContext.DeltaSeconds;
            goldenGoblinAI.Velocity = FlaiMath.ClampLength(goldenGoblinAI.Velocity + velocity, Speed);

            transform.Position += goldenGoblinAI.Velocity * updateContext.DeltaSeconds;
            transform.RotationVector = goldenGoblinAI.Velocity;

            // if the goblin is outside the map, delete it
            if (!SkypieaConstants.MapAreaInPixels.Inflate(128, 128).Contains(transform.Position))
            {
                goldenGoblinAI.Entity.Delete();
            }
        }

        private bool HasBeenHit(Entity zombie)
        {
            return zombie.Get<CHealth>().HasBeenHit;
        }

        private bool IsPlayerTooClose(Entity zombie)
        {
            const float Distance = SkypieaConstants.PixelsPerMeter * 3;
            return Vector2.Distance(_playerTransform.Position, zombie.Transform.Position) < Distance;
        }
    }
}
