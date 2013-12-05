using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.DataStructures;
using Skypiea.Components;
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
            : base(Aspect.Combine(Aspect.WithTag(EntityTags.Zombie), Aspect.All<CArea>()))
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

            for (int i = 0; i < entities.Count; i++)
            {
                Entity zombie = entities[i];
                CArea area = zombie.Get<CArea>();
                if (area.Area.Contains(_player.Transform.Position)) // todo: instead of _player.Transform.Position, use a small area and Intersects
                {
                    _playerInfo.OnKilled();
                    break;
                }
            }
        }
    }
}
