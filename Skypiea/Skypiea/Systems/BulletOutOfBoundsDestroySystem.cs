using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Systems
{
    public class BulletOutOfBoundsDestroySystem : ComponentProcessingSystem<CBullet>
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PostFrame; }
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
