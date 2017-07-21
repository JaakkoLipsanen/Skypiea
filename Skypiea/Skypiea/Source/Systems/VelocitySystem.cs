using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Systems;

namespace Skypiea.Systems
{
    public class VelocitySystem : ComponentProcessingSystem<CVelocity2D>
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PreCollision; } // ..? post-update?
        }

        public override void Process(UpdateContext updateContext, Entity entity, CVelocity2D velocity)
        {
            entity.Transform.Position += velocity.Direction * velocity.Speed * updateContext.DeltaSeconds;
        }
    }
}
