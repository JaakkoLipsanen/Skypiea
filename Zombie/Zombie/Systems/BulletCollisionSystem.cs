using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Zombie.Components;
using Zombie.Misc;

namespace Zombie.Systems
{
    public class BulletCollisionSystem : ComponentProcessingSystem<BulletComponent>
    {
        private readonly EntityTrackerCollection _zombieTracker = EntityTrackerCollection.FromAspect(Aspect.WithTag(EntityTags.Zombie));
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Collision; }
        }

        protected override void Initialize()
        {
            this.EntityWorld.AddEntityTracker(_zombieTracker);
        }

        public override void Process(UpdateContext updateContext, Entity entity, BulletComponent bullet)
        {
            RectangleF bulletArea = bullet.Area;
            foreach (Entity zombie in _zombieTracker)
            {
                AreaComponent area = zombie.Get<AreaComponent>();
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
