using Flai;
using Flai.CBES;
using Flai.General;
using Microsoft.Xna.Framework;
using Skypiea.Messages;
using Skypiea.Model.Boosters;

namespace Skypiea.Components
{
    // I could do a lot more specific components (PlayerLivesComponent etc), but atm can't see a reason to do so
    public class CPlayerInfo : Component
    {
        private const float RespawnTime = 2.5f;
        private const float RespawnInvulnerabilityTime = 8;

        private readonly Timer _spawnTimer = new Timer(CPlayerInfo.RespawnTime);
        private readonly Timer _respawnInvulnerabilityTimer = new Timer(CPlayerInfo.RespawnInvulnerabilityTime);
        private IBoosterState _boosterState;

        public int TotalLives { get; private set; }
        public int LivesRemaining { get; private set; }
        public int Score { get; set; }

        public bool IsAlive
        {
            get { return this.LivesRemaining > 0 && _spawnTimer.HasFinished; }
        }

        public bool IsInvulnerable
        {
            get
            {
                _boosterState = _boosterState ?? this.EntityWorld.Services.Get<IBoosterState>();
                return !_respawnInvulnerabilityTimer.HasFinished || BoosterHelper.IsPlayerInvulnerable(_boosterState);
            }
        }

        public bool IsVisuallyInvulnerable
        {
            get
            {
                _boosterState = _boosterState ?? this.EntityWorld.Services.Get<IBoosterState>();
                return BoosterHelper.IsPlayerInvulnerable(_boosterState) || (_respawnInvulnerabilityTimer.ElapsedTime < CPlayerInfo.RespawnInvulnerabilityTime - 1f);
            }
        }

        // current movement per second vector.
        public Vector2 MovementPerSecond { get; set; }

        public CPlayerInfo(int totalLives)
        {
            this.TotalLives = totalLives;
            this.LivesRemaining = totalLives;
            _spawnTimer.ForceFinish();
            _respawnInvulnerabilityTimer.ForceFinish();
        }

        protected internal override void PreUpdate(UpdateContext updateContext)
        {
            _spawnTimer.Update(updateContext);
            _respawnInvulnerabilityTimer.Update(updateContext);
        }

        public void AddLife()
        {
            this.LivesRemaining++;
            this.TotalLives = FlaiMath.Max(this.TotalLives, this.LivesRemaining);
        }

        public void KillPlayer()
        {
            if (this.IsInvulnerable)
            {
                return;
            }

            this.LivesRemaining--;
            _spawnTimer.Restart();
            _respawnInvulnerabilityTimer.Restart();

            this.Entity.EntityWorld.BroadcastMessage(this.Entity.EntityWorld.FetchMessage<PlayerKilledMessage>().Initialize(this)); // blaarghh.. ugly...
            if (this.LivesRemaining == 0)
            {
                this.EntityWorld.BroadcastMessage(new GameOverMessage());
            }
        }
    }
}
