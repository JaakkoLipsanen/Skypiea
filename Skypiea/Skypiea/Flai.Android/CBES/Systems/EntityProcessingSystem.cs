
using Flai.DataStructures;

namespace Flai.CBES.Systems
{
    public abstract class EntityProcessingSystem : ProcessingSystem
    {
        protected EntityProcessingSystem(Aspect aspect)
            : base(aspect)
        {
        }

        protected override sealed void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                this.Process(updateContext, entities[i]);
            }
        }

        protected abstract void Process(UpdateContext updateContext, Entity entity);
    }
}
