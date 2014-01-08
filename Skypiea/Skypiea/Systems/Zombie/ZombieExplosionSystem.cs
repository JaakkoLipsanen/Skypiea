using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Zombie
{
    public class ZombieExplosionSystem : EntitySystem
    {
        private IPlayerPassiveStats _passiveStats;
        private IZombieSpatialMap _zombieSpatialMap;
        protected override void Initialize()
        {
            _passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
            _zombieSpatialMap = this.EntityWorld.Services.Get<IZombieSpatialMap>();
            this.EntityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            // insta killed zombies can't explode
            if (message.Zombie.Get<CZombieInfo>().KillReason == KillReason.Instakill)
            {
                return;
            }

            if (Global.Random.NextFromOdds(_passiveStats.ChanceForZombieExplodeOnDeath))
            {
                const float ExplosionRange = SkypieaConstants.PixelsPerMeter * 3;
                foreach (Entity zombie in _zombieSpatialMap.GetAllIntersecting(message.Zombie.Transform, ExplosionRange))
                {
                    if (zombie.Get<CHealth>().IsAlive)
                    {
                        ZombieHelper.Kill(zombie, Vector2.Zero);
                    }
                }
            }
        }
    }
}
