using System.Diagnostics;
using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Services;

namespace Skypiea.Systems
{
    public class BulletCollisionSystem : ComponentProcessingSystem<CBullet>
    {
        private IZombieSpatialMap _zombieSpatialMap;
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Collision; }
        }

        protected override void Initialize()
        {
            _zombieSpatialMap = this.EntityWorld.Services.Get<IZombieSpatialMap>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, CBullet bullet)
        {
            foreach (Entity zombie in _zombieSpatialMap.GetZombiesWithinRange(entity.Transform.Position, 8))
            {
                if (bullet.InvokeCallback(zombie))
                {
                    break;
                }
            }
        }
    }
}
