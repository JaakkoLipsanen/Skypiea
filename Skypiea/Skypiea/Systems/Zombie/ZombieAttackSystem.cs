using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.DataStructures;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Services;

namespace Skypiea.Systems.Zombie
{
    public class ZombieAttackSystem : ProcessingSystem
    {
        private Entity _player;
        private CPlayerInfo _playerInfo;

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Collision + 1; } // after "BulletCollisionSystem", because why not.
        }

        public ZombieAttackSystem()
            : base(Aspect.All<CZombieInfo>())
        {
        }

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            _playerInfo = _player.Get<CPlayerInfo>();
        }

        protected override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            IZombieSpatialMap zombieSpatialMap = this.EntityWorld.Services.Get<IZombieSpatialMap>();
            foreach (Entity zombie in zombieSpatialMap.GetZombiesWithinRange(_player.Transform, 2))
            {
                if (true) //!_playerInfo.IsInvulnerable)
                {
                   // _playerInfo.KillPlayer();
                    return;
                }
                else // if is invulnerable
                {
                    ZombieHelper.Kill(zombie);
                }
            }
        }
    }
}
