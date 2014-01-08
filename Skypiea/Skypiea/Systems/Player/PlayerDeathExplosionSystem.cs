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
        private IPlayerPassiveStats _passiveStats;

        protected override void Initialize()
        {
            this.EntityWorld.AddEntityTracker(_zombieEntityTracker);
            this.EntityWorld.SubscribeToMessage<PlayerKilledMessage>(this.OnPlayerKilled);
            _passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
        }

        private void OnPlayerKilled(PlayerKilledMessage message)
        {
            if (_passiveStats.ChanceToKillEveryoneOnDeath > 0 && Global.Random.NextFromOdds(_passiveStats.ChanceToKillEveryoneOnDeath))
            {
                foreach (Entity zombie in _zombieEntityTracker)
                {
                    if (zombie.Get<CHealth>().IsAlive)
                    {
                        ZombieHelper.Kill(zombie, Vector2.Zero);
                        this.EntityWorld.BroadcastMessage(this.EntityWorld.FetchMessage<ZombieKilledMessage>().Initialize(zombie));
                        zombie.Delete();
                    }
                }
            }
        }
    }
}
