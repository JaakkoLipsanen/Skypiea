using Flai.DataStructures;
using Flai.Graphics;

namespace Flai.CBES.Graphics
{
    public abstract class EntityProcessingRenderer : EntityRenderer
    {
        protected EntityProcessingRenderer(EntityWorld entityWorld, Aspect aspect)
            : base(entityWorld, aspect)
        {
        }

        protected sealed override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                this.Draw(graphicsContext, entity);
            }
        }

        protected abstract void Draw(GraphicsContext graphicsContext, Entity entity);
    }
}
