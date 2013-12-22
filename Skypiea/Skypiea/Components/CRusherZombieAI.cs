using Flai;
using Flai.CBES;
using Flai.General;
using Microsoft.Xna.Framework;

namespace Skypiea.Components
{
    public enum RusherZombieState
    {
        None,
        Wandering,
        WanderingAwayFromPlayer,
        Rushing,
        RushingStun,
    }

    public class CRusherZombieAI : PoolableComponent
    {
        private const float RushingStunTime = 1f;
        private const float RushingMinInterval = 1.5f;
        private readonly Timer _rushingStunTimer = new Timer(CRusherZombieAI.RushingStunTime);
        private readonly Timer _rushingAllowedTimer = new Timer(CRusherZombieAI.RushingMinInterval);

        public float RushingSpeed { get; set; }
        public RusherZombieState State { get; set; }
        public Vector2 Target { get; set; } // walking tager, wandering target, rushing target. whatever!

        // okay awful name. but bascially after rushing, there should be a small "waiting period" where the zombies just stands
        public Timer RushingStunTimer
        {
            get { return _rushingStunTimer; }
        }

        public Timer NextRushAllowedTimer
        {
            get { return _rushingAllowedTimer; }
        }

        protected override void PreUpdate(UpdateContext updateContext)
        {
            _rushingStunTimer.Update(updateContext);
            _rushingAllowedTimer.Update(updateContext);
        }

        protected override void Cleanup()
        {
            _rushingStunTimer.Restart();
            _rushingAllowedTimer.Restart();
            this.State = RusherZombieState.None;
            this.Target = Vector2.Zero;
        }
    }
}
