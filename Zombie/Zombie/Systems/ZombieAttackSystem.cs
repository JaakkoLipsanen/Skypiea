using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Zombie.Components;
using Zombie.Misc;

namespace Zombie.Systems
{
    public class ZombieAttackSystem : ProcessingSystem
    {
        private Entity _player;
        private PlayerInfoComponent _playerInfo;
        private TransformComponent _playerTransform;

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Collision + 1; } // after "BulletCollisionSystem", because why not.
        }

        public ZombieAttackSystem()
            : base(Aspect.Combine(Aspect.WithTag(EntityTags.Zombie), Aspect.All<AreaComponent>()))
        {           
        }

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityTags.Player);
            _playerInfo = _player.Get<PlayerInfoComponent>();
            _playerTransform = _player.Get<TransformComponent>();
        }

        protected override void Process(UpdateContext updateContext, EntityCollection entities)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            foreach (Entity zombie in entities)
            {
                AreaComponent area = zombie.Get<AreaComponent>();
                if (area.Area.Contains(_playerTransform.Position))
                {
                    _playerInfo.OnKilled();
                    break;
                }
            }
        }
    }
}
