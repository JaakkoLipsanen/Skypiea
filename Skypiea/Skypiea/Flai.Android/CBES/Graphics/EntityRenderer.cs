using Flai.DataStructures;
using Flai.Graphics;

namespace Flai.CBES.Graphics
{
    public abstract class EntityRenderer : FlaiRenderer
    {
        private readonly EntityTracker _entityTracker;
        protected readonly EntityWorld _entityWorld;

        protected EntityRenderer(EntityWorld entityWorld, Aspect aspect)
        {
            _entityWorld = entityWorld;
            _entityTracker = EntityTracker.FromAspect(aspect);
            _entityTracker.EntityAdded += this.OnEntityAdded;
            _entityTracker.EntityRemoved += this.OnEntityRemoved;

            entityWorld.AddEntityTracker(_entityTracker);
        }

        protected sealed override void DrawInner(GraphicsContext graphicsContext)
        {
            this.PreDraw(graphicsContext);
            this.Draw(graphicsContext, _entityTracker.Entities);
            this.PostDraw(graphicsContext);
        }

        protected virtual void OnEntityAdded(Entity entity) { }
        protected virtual void OnEntityRemoved(Entity entity) { }
         
        protected virtual void PreDraw(GraphicsContext graphicsContext) { } // entities?
        protected abstract void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities);
        protected virtual void PostDraw(GraphicsContext graphicsContext) { } // entities?
    }
}
