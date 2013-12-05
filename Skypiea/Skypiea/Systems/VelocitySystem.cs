using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;

namespace Skypiea.Systems
{
    public class VelocitySystem : ComponentProcessingSystem<CVelocity>
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PreCollision; } // ..? post-update?
        }

        public override void Process(UpdateContext updateContext, Entity entity, CVelocity velocity)
        {
            entity.Transform.Position += velocity.Direction * velocity.Speed * updateContext.DeltaSeconds;
        }
    }
}
