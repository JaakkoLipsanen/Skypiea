using Flai;
using Flai.CBES;
using Flai.Misc;
using Zombie.Messages;

namespace Zombie.Components
{
    // I could do a lot more specific components (PlayerLivesComponent etc), but atm can't see a reason to do so
    public class PlayerInfoComponent : Component
    {
        private const float PlayerRespawnTime = 3f;
        private readonly Timer _spawnTimer = new Timer(PlayerInfoComponent.PlayerRespawnTime);

        public int TotalLives { get; private set; }
        public int LivesRemaining { get; private set; }
        public int Score { get; set; }

        public bool IsAlive
        {
            get { return this.LivesRemaining > 0 && _spawnTimer.HasFinished; }
        }

        public PlayerInfoComponent(int totalLives)
        {
            this.TotalLives = totalLives;
            this.LivesRemaining = totalLives;
            _spawnTimer.ForceFinish();
        }

        protected override void PreUpdate(UpdateContext updateContext)
        {
            _spawnTimer.Update(updateContext);
        }

        // todo: subscribe to message?
        public void OnKilled()
        {
            this.LivesRemaining--;
            _spawnTimer.Restart();

            this.Parent.EntityWorld.BroadcastMessage(new PlayerKilledMessage(this));
        }
    }
}
