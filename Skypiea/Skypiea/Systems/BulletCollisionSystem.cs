using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Systems
{
    public class BulletCollisionSystem : ComponentProcessingSystem<CBullet>
    {
        private readonly EntityTracker _zombieTracker = EntityTracker.FromAspect(Aspect.WithTag(EntityTags.Zombie));
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Collision; }
        }

        protected override void Initialize()
        {
            this.EntityWorld.AddEntityTracker(_zombieTracker);
        }

        public override void Process(UpdateContext updateContext, Entity entity, CBullet bullet)
        {
            RectangleF bulletArea = bullet.Area;
            foreach (Entity zombie in _zombieTracker)
            {
                CArea area = zombie.Get<CArea>();
                if (bulletArea.Intersects(area.Area)) // todo: "sweep" from previous bullet position
                {
                    if (bullet.InvokeCallback(zombie))
                    {
                        break;
                    }
                }
            }
        }
    }
}
