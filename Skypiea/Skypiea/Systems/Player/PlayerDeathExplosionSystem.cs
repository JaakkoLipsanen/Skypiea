using Flai;
using Flai.CBES.Systems;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerDeathExplosionSystem : EntitySystem
    {
        private IPlayerPassiveStats _passiveStats;
        protected override void Initialize()
        {
            this.EntityWorld.SubscribeToMessage<PlayerKilledMessage>(this.OnPlayerKilled);
            _passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
        }

        private void OnPlayerKilled(PlayerKilledMessage message)
        {
            if (_passiveStats.ChanceToKillEveryoneOnDeath > 0 && Global.Random.NextFromOdds(_passiveStats.ChanceToKillEveryoneOnDeath))
            {
                ZombieHelper.KillAllZombies(this.EntityWorld);
            }
        }
    }
}
