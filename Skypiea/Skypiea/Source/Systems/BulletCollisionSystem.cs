using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Systems.Zombie;

namespace Skypiea.Systems
{
    public class BulletCollisionSystem : ComponentProcessingSystem<CBullet>
    {
        private IZombieSpatialMap _zombieSpatialMap;
        protected internal override int ProcessOrder
        {
            get { return SystemProcessOrder.Collision; }
        }

        protected override void Initialize()
        {
            _zombieSpatialMap = this.EntityWorld.Services.Get<IZombieSpatialMap>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, CBullet velocity)
        {
            foreach (Entity zombie in _zombieSpatialMap.GetAllIntersecting(entity.Transform.Position, 8))
            {
                if (velocity.InvokeCallback(updateContext, zombie))
                {
                    break;
                }
            }
        }
    }
}
