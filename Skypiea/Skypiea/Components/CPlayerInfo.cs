using Flai;
using Flai.CBES;
using Flai.Misc;
using Skypiea.Messages;

namespace Skypiea.Components
{
    // I could do a lot more specific components (PlayerLivesComponent etc), but atm can't see a reason to do so
    public class CPlayerInfo : Component
    {
        private const float RespawnTime = 1.5f;
        private const float RespawnInvulnerabilityTime = 7;

        private readonly Timer _spawnTimer = new Timer(CPlayerInfo.RespawnTime);
        private readonly Timer _invulnerabilityTimer = new Timer(CPlayerInfo.RespawnInvulnerabilityTime);

        public int TotalLives { get; private set; }
        public int LivesRemaining { get; private set; }
        public int Score { get; set; }

        public bool IsAlive
        {
            get { return this.LivesRemaining > 0 && _spawnTimer.HasFinished; }
        }

        public bool IsInvulnerable
        {
            get { return !_invulnerabilityTimer.HasFinished; }
        }

        public bool IsVisuallyInvulnerable
        {
            get { return _invulnerabilityTimer.ElapsedTime < CPlayerInfo.RespawnInvulnerabilityTime - 1.5f; }
        }

        public CPlayerInfo(int totalLives)
        {
            this.TotalLives = totalLives;
            this.LivesRemaining = totalLives;
            _spawnTimer.ForceFinish();
            _invulnerabilityTimer.ForceFinish();
        }

        protected override void PreUpdate(UpdateContext updateContext)
        {
            _spawnTimer.Update(updateContext);
            _invulnerabilityTimer.Update(updateContext);
        }

        // todo: subscribe to message?
        public void KillPlayer()
        {
            if (this.IsInvulnerable)
            {
                return;
            }

            this.LivesRemaining--;
            _spawnTimer.Restart();
            _invulnerabilityTimer.Restart();

            this.Entity.EntityWorld.BroadcastMessage(new PlayerKilledMessage(this));
        }
    }
}
