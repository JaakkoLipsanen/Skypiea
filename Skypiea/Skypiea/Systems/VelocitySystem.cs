using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Skypiea.Components;

namespace Skypiea.Systems
{
    public class VelocitySystem : ComponentProcessingSystem<CVelocity2D>
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PreCollision; } // ..? post-update?
        }

        public override void Process(UpdateContext updateContext, Entity entity, CVelocity2D velocity2D)
        {
            entity.Transform.Position += velocity2D.Direction * velocity2D.Speed * updateContext.DeltaSeconds;
        }
    }
}
