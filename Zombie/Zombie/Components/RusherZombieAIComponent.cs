using Flai;
using Flai.CBES;
using Flai.Misc;
using Microsoft.Xna.Framework;

namespace Zombie.Components
{
    public enum RusherZombieState
    {
        None,
        Wandering,
        Rushing,
        RushingStun,
    }

    public class RusherZombieAIComponent : Component
    {
        private const float RushingStunTime = 1f;
        private const float RushingMinInterval = 1f;
        private readonly Timer _rushingStunTimer = new Timer(RusherZombieAIComponent.RushingStunTime);
        private readonly Timer _rushingAllowedTimer = new Timer(RusherZombieAIComponent.RushingMinInterval);

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

        // todo: time-until-new-rush-can-be-made -period

        protected override void PreUpdate(UpdateContext updateContext)
        {
            _rushingStunTimer.Update(updateContext);
            _rushingAllowedTimer.Update(updateContext);
        }
    }
}
