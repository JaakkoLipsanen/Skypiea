using Flai.DataStructures;
using Flai.Graphics;

namespace Flai.CBES.Graphics
{
    public abstract class ComponentProcessingRenderer<T1> : EntityRenderer
        where T1 : Component
    {
        protected ComponentProcessingRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<T1>())
        {
        }

        protected ComponentProcessingRenderer(EntityWorld entityWorld, Aspect aspect)
            : base(entityWorld, Aspect.Combine(Aspect.All<T1>(), aspect))
        {
        }

        protected sealed override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                this.Draw(graphicsContext, entity, entity.Get<T1>());
            }
        }

        protected abstract void Draw(GraphicsContext graphicsContext, Entity entity, T1 drop);
    }

    public abstract class ComponentProcessingRenderer<T1, T2> : EntityRenderer
        where T1 : Component
        where T2 : Component
    {
        protected ComponentProcessingRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<T1, T2>())
        {
        }

        protected ComponentProcessingRenderer(EntityWorld entityWorld, Aspect aspect)
            : base(entityWorld, Aspect.Combine(Aspect.All<T1, T2>(), aspect))
        {
        }

        protected sealed override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                this.Draw(graphicsContext, entity, entity.Get<T1>(), entity.Get<T2>());
            }
        }

        protected abstract void Draw(GraphicsContext graphicsContext, Entity entity, T1 component1, T2 component2);
    }

    public abstract class ComponentProcessingRenderer<T1, T2, T3> : EntityRenderer
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        protected ComponentProcessingRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<T1, T2, T3>())
        {
        }

        protected ComponentProcessingRenderer(EntityWorld entityWorld, Aspect aspect)
            : base(entityWorld, Aspect.Combine(Aspect.All<T1, T2, T3>(), aspect))
        {
        }

        protected sealed override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                this.Draw(graphicsContext, entity, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>());
            }
        }

        protected abstract void Draw(GraphicsContext graphicsContext, Entity entity, T1 component1, T2 component2, T3 component4);
    }

    public abstract class ComponentProcessingRenderer<T1, T2, T3, T4> : EntityRenderer
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        protected ComponentProcessingRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<T1, T2, T3, T4>())
        {
        }

        protected ComponentProcessingRenderer(EntityWorld entityWorld, Aspect aspect)
            : base(entityWorld, Aspect.Combine(Aspect.All<T1, T2, T3, T4>(), aspect))
        {
        }

        protected sealed override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                this.Draw(graphicsContext, entity, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>(), entity.Get<T4>());
            }
        }

        protected abstract void Draw(GraphicsContext graphicsContext, Entity entity, T1 component1, T2 component2, T3 component3, T4 component4);
    }

    public abstract class ComponentProcessingRenderer<T1, T2, T3, T4, T5> : EntityRenderer
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        protected ComponentProcessingRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<T1, T2, T3, T4, T5>())
        {
        }

        protected ComponentProcessingRenderer(EntityWorld entityWorld, Aspect aspect)
            : base(entityWorld, Aspect.Combine(Aspect.All<T1, T2, T3, T4, T5>(), aspect))
        {
        }

        protected sealed override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                this.Draw(graphicsContext, entity, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>(), entity.Get<T4>(), entity.Get<T5>());
            }
        }

        protected abstract void Draw(GraphicsContext graphicsContext, Entity entity, T1 component1, T2 component2, T3 component3, T4 component4, T5 component5);
    }

    public abstract class ComponentProcessingRenderer<T1, T2, T3, T4,T5, T6> : EntityRenderer
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
        where T6 : Component
    {
        protected ComponentProcessingRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<T1, T2, T3, T4, T5, T6>())
        {
        }

        protected ComponentProcessingRenderer(EntityWorld entityWorld, Aspect aspect)
            : base(entityWorld, Aspect.Combine(Aspect.All<T1, T2, T3, T4, T5, T6>(), aspect))
        {
        }

        protected sealed override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                this.Draw(graphicsContext, entity, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>(), entity.Get<T4>(), entity.Get<T5>(), entity.Get<T6>());
            }
        }

        protected abstract void Draw(GraphicsContext graphicsContext, Entity entity, T1 component1, T2 component2, T3 component3, T4 component4, T5 component5, T6 component6);
    }
}
