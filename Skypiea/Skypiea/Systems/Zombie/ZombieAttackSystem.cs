using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.DataStructures;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;

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

            float range = _playerInfo.IsInvulnerable ? 12 : 6;
            IZombieSpatialMap zombieSpatialMap = this.EntityWorld.Services.Get<IZombieSpatialMap>();
            foreach (Entity zombie in zombieSpatialMap.GetAllIntersecting(_player.Transform, range))
            {
                if (!_playerInfo.IsInvulnerable)
                {
                    _playerInfo.KillPlayer();
                    if (_playerInfo.LivesRemaining == 0)
                    {
                        this.EntityWorld.BroadcastMessage(new GameOverMessage());
                    }

                    return;
                }

                // player is invulnerable
                ZombieHelper.Kill(zombie, Vector2.Zero);
            }
        }
    }
}
