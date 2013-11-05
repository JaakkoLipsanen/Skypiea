using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Zombie.Components;

namespace Zombie.Systems
{
    public class VelocitySystem : ComponentProcessingSystem<TransformComponent, VelocityComponent>
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PreCollision; } // ..? post-update?
        }

        public override void Process(UpdateContext updateContext, Entity entity, TransformComponent transform, VelocityComponent velocity)
        {
            transform.Position += velocity.Direction * velocity.Speed * updateContext.DeltaSeconds;
        }
    }
}
