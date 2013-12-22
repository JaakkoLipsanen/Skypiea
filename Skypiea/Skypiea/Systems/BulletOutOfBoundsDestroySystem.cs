using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems
{
    public class BulletOutOfBoundsDestroySystem : ComponentProcessingSystem<CBullet>
    {
        private World _world;
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PostFrame; }
        }

        protected override void Initialize()
        {
            _world = this.EntityWorld.Services.Get<World>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, CBullet velocity)
        {
            RectangleF cameraArea = SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active);
            RectangleF bulletArea = velocity.Area;

            if (!bulletArea.Intersects(cameraArea))
            {
                entity.Delete();
            }
        }
    }
}
