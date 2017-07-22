
using Flai.DataStructures;

namespace Flai.CBES.Systems
{
    public abstract class ComponentProcessingSystem<T1> : ProcessingSystem
        where T1 : Component
    {
        protected ComponentProcessingSystem()
            : this(Aspect.AcceptAll)
        {
        }

        protected ComponentProcessingSystem(Aspect aspect)
            : base(Aspect.Combine(aspect, Aspect.All<T1>()))
        {
        }

        protected sealed override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                this.Process(updateContext, entity, entity.Get<T1>());
            }
        }

        public abstract void Process(UpdateContext updateContext, Entity entity, T1 component1);
    }

    public abstract class ComponentProcessingSystem<T1, T2> : ProcessingSystem
        where T1 : Component
        where T2 : Component
    {
        protected ComponentProcessingSystem()
            : this(Aspect.AcceptAll)
        {
        }

        protected ComponentProcessingSystem(Aspect aspect)
            : base(Aspect.Combine(aspect, Aspect.All<T1, T2>()))
        {
        }

        protected sealed override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                this.Process(updateContext, entity, entity.Get<T1>(), entity.Get<T2>());
            }
        }

        public abstract void Process(UpdateContext updateContext, Entity entity, T1 component1, T2 component2);
    }

    public abstract class ComponentProcessingSystem<T1, T2, T3> : ProcessingSystem
        where T1 : Component
        where T2 : Component
        where T3 : Component
    {
        protected ComponentProcessingSystem()
            : this(Aspect.AcceptAll)
        {
        }

        protected ComponentProcessingSystem(Aspect aspect)
            : base(Aspect.Combine(aspect, Aspect.All<T1, T2, T3>()))
        {
        }

        protected sealed override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                this.Process(updateContext, entity, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>());
            }
        }

        public abstract void Process(UpdateContext updateContext, Entity entity, T1 component1, T2 component2, T3 component3);
    }

    public abstract class ComponentProcessingSystem<T1, T2, T3, T4> : ProcessingSystem
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
    {
        protected ComponentProcessingSystem()
            : this(Aspect.AcceptAll)
        {
        }

        protected ComponentProcessingSystem(Aspect aspect)
            : base(Aspect.Combine(aspect, Aspect.All<T1, T2, T3, T4>()))
        {
        }

        protected sealed override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                this.Process(updateContext, entity, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>(), entity.Get<T4>());
            }
        }

        public abstract void Process(UpdateContext updateContext, Entity entity, T1 component1, T2 component2, T3 component3, T4 component4);
    }

    public abstract class ComponentProcessingSystem<T1, T2, T3, T4, T5> : ProcessingSystem
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
    {
        protected ComponentProcessingSystem()
            : this(Aspect.AcceptAll)
        {
        }

        protected ComponentProcessingSystem(Aspect aspect)
            : base(Aspect.Combine(aspect, Aspect.All<T1, T2, T3, T4, T5>()))
        {
        }

        protected sealed override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                this.Process(updateContext, entity, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>(), entity.Get<T4>(), entity.Get<T5>());
            }
        }

        public abstract void Process(UpdateContext updateContext, Entity entity, T1 component1, T2 component2, T3 component3, T4 component4, T5 component5);
    }

    public abstract class ComponentProcessingSystem<T1, T2, T3, T4, T5, T6> : ProcessingSystem
        where T1 : Component
        where T2 : Component
        where T3 : Component
        where T4 : Component
        where T5 : Component
        where T6 : Component
    {
        protected ComponentProcessingSystem()
            : this(Aspect.AcceptAll)
        {
        }

        protected ComponentProcessingSystem(Aspect aspect)
            : base(Aspect.Combine(aspect, Aspect.All<T1, T2, T3, T4, T5, T6>()))
        {
        }

        protected sealed override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                this.Process(updateContext, entity, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>(), entity.Get<T4>(), entity.Get<T5>(), entity.Get<T6>());
            }
        }

        public abstract void Process(UpdateContext updateContext, Entity entity, T1 component1, T2 component2, T3 component3, T4 component4, T5 component5, T6 component6);
    }
}