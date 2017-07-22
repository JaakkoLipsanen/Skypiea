using Flai;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Stats;

namespace Skypiea.Systems
{
    public class StatsTrackerSystem : EntitySystem
    {
        private readonly StatsManager _statsManager;
        private CTransform2D _playerTransform;
        private Vector2 _previousPosition;

        protected internal override int ProcessOrder
        {
            get { return SystemProcessOrder.PostFrame; }
        }

        public StatsTrackerSystem()
        {
            _statsManager = FlaiGame.Current.Services.Get<StatsManager>();
        }

        protected override void Initialize()
        {
            _playerTransform = this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CTransform2D>();
            _previousPosition = _playerTransform.Transform.Position;

            this.EntityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            this.EntityWorld.SubscribeToMessage<GameExitMessage>(this.OnGameExit);
            this.EntityWorld.SubscribeToMessage<GameOverMessage>(this.OnGameOver);
        }

        private void OnGameOver(GameOverMessage message)
        {
            _statsManager.Save();
        }

        private void OnGameExit(GameExitMessage message)
        {
            _statsManager.Save();
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            _statsManager.Kills++;
        }

        protected override void Update(UpdateContext updateContext)
        {
            _statsManager.TimePlayed += updateContext.DeltaSeconds;
            _statsManager.TotalMovement += Vector2.Distance(_previousPosition, _playerTransform.Position) / SkypieaConstants.PixelsPerMeter * SkypieaConstants.MeterMultiplier;
            _previousPosition = _playerTransform.Position;
        }
    }
}
