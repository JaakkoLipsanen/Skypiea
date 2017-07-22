#if CBES_3D
using DefaultTransformComponent = Flai.CBES.Components.CTransform3D;
#else
using DefaultTransformComponent = Flai.CBES.Components.CTransform2D;
#endif

using Flai.General;

namespace Flai.CBES
{
    public abstract class Component
    {
        // todo: unique id?
        public Entity Entity { get; internal set; } // todo: not sure about name. Entity vs Parent vs ParentEntity. the problem with Parent(something) is that it's easy to confuse Component.Parent vs Entity.Parent
        public DefaultTransformComponent Transform { get { return this.Entity.Transform; } } // public???
        // todo: IsEnabled { get; set; } ???

        protected EntityWorld EntityWorld
        {
            get { return this.Entity.EntityWorld; }
        }

        protected internal virtual void Initialize() { } // no idea if this is any good... was "OnAttachedToParent" before.. should there even be "On" in the name? rename to "Initialize" and remove initialize from PoolableComponent?
        protected internal virtual void PreUpdate(UpdateContext updateContext) { }
        protected internal virtual void PostUpdate(UpdateContext updateContext) { }

        public override string ToString()
        {
            return this.GetType().Name;
        }

        // if(component) vs if(component != null)
        public static implicit operator bool(Component component)
        {
            return component != null;
        }
    }

    public abstract class PoolableComponent : Component
    {
        // meh... used in the ComponentPool, kinda needed there, otherwise would have to use Dictionary and use GetTypes etc on Store
        internal int PoolableComponentTypeID { get; set; }

        // should there be PoolableComponent specific Initialize?
        protected internal virtual void Cleanup() { }
    }

    // public?
    public static class Component<T>
        where T : Component
    {
        public static readonly TypeMask<Component> Bit = TypeMask<Component>.GetBit<T>();
        public static readonly int ID = (int)TypeID<Component>.GetID<T>();
        public static readonly bool CanBeRemoved = typeof(T) != CBESPreprocessorHelper.DefaultTransformComponentType;
        public static readonly bool IsPoolable = typeof (PoolableComponent).IsAssignableFrom(typeof (T));
    }

    internal static class PoolableComponent<T>
        where T : PoolableComponent
    {
        public static readonly int ID = (int)TypeID<PoolableComponent>.GetID<T>();
    }
}