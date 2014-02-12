using System;
using Flai;
using Flai.CBES;
using Flai.DataStructures;
using Flai.General;
using Microsoft.Xna.Framework;
using Skypiea.Misc;

namespace Skypiea.Components
{
    public enum GoldenGoblinState
    {
        TravellingToWaypoint,
        WaypointStun,

        EscapingFromPlayer,
    }

    public class CGoldenGoblinAI : PoolableComponent
    {
        private const int WaypointCount = 5;
        public GoldenGoblinState State { get; set; }
        public int CurrentWaypointIndex { get; set; }
        public Vector2 Velocity { get; set; }
        public Timer WaypointStunTimer { get; private set; }

        public Vector2 CurrentWaypoint
        {
            get { return _waypoints[this.CurrentWaypointIndex]; }
        }

        private readonly Vector2[] _waypoints = new Vector2[CGoldenGoblinAI.WaypointCount];
        private readonly ReadOnlyArray<Vector2> _readOnlyWaypoints;

        public ReadOnlyArray<Vector2> Waypoints
        {
            get { return _readOnlyWaypoints; }
        }

        public CGoldenGoblinAI()
        {
            _readOnlyWaypoints = new ReadOnlyArray<Vector2>(_waypoints);
            this.WaypointStunTimer = new Timer(0.001f); // ????;
        }

        public void Initialize(Vector2 position)
        {
            this.GenerateWaypoints(position);
        }

        protected override void PostUpdate(UpdateContext updateContext)
        {
            this.WaypointStunTimer.Update(updateContext);
        }

        protected override void Cleanup()
        {
            this.State = GoldenGoblinState.TravellingToWaypoint;
            this.CurrentWaypointIndex = 0;
            this.Velocity = Vector2.Zero;
            this.WaypointStunTimer.ForceFinish();

            Array.Clear(_waypoints, 0, _waypoints.Length);
        }

        private void GenerateWaypoints(Vector2 startPosition)
        {
            Vector2 previousPosition = startPosition;
            Vector2 currentPosition = startPosition;
            for (int i = 0; i < _waypoints.Length; i++)
            {
                int testCount = 0;
                Vector2 newPosition;
                float angle;
                do
                {
                    newPosition = FlaiAlgorithms.GenerateRandomVector2(SkypieaConstants.MapWidthInPixelsRange.AsInflated(-128), SkypieaConstants.MapHeightInPixelsRange.AsInflated(-128), currentPosition, SkypieaConstants.MapHeightInPixels / 6f);
                    angle = FlaiMath.AngleBetweenVectors(newPosition - currentPosition, currentPosition - previousPosition);
                    testCount++;

                } while (FlaiMath.Abs(angle) > FlaiMath.Pi * 0.5f && testCount < 10);

                previousPosition = currentPosition;
                currentPosition = newPosition;
                _waypoints[i] = currentPosition;
            }
        }
    }
}
