using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerDeathExplosionSystem : EntitySystem
    {
        private readonly EntityTracker _zombieEntityTracker = EntityTracker.FromAspect(Aspect.All<CZombieInfo>());
        private Entity _player;
        private IPlayerPassiveStats _passiveStats;

        private bool _isExploding = false;
        private float _explosionRange = 0f;

        protected override void Initialize()
        {
            this.EntityWorld.AddEntityTracker(_zombieEntityTracker);
            this.EntityWorld.SubscribeToMessage<PlayerKilledMessage>(this.OnPlayerKilled);

            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            _passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
        }

        protected override void Update(UpdateContext updateContext)
        {
            if (_isExploding)
            {
                _explosionRange += updateContext.DeltaSeconds * SkypieaConstants.MapWidthInPixels / 2f;
                foreach (Entity zombie in _zombieEntityTracker)
                {
                    if (Vector2.Distance(_player.Transform.Position, zombie.Transform.Position) < _explosionRange)
                    {
                        zombie.Delete();

                        Vector2 velocity = FlaiMath.NormalizeOrZero(zombie.Transform.Position - _player.Transform.Position) * SkypieaConstants.PixelsPerMeter * 10;
                        ZombieHelper.TriggerBloodExplosion(zombie.Transform, velocity);
                    }
                }

                if (_explosionRange > SkypieaConstants.MapWidthInPixels)
                {
                    _isExploding = false;
                    _explosionRange = 0;
                }
            }
        }

        private void OnPlayerKilled(PlayerKilledMessage message)
        {
            if (_passiveStats.ChanceToKillEveryoneOnDeath > 0 && Global.Random.NextFromOdds(_passiveStats.ChanceToKillEveryoneOnDeath))
            {
                _isExploding = true;
                _explosionRange = 0;
            }
        }
    }
}
