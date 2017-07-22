
using Flai.DataStructures;
using Flai.IO;
using System;
using System.Reflection;

namespace Flai.CBES.Pools
{
    // okay this could be modified to not use reflection, look at MessagePool.. maube...?? but it forces some things, like fetch<T> must be called before store<T>
    internal class ComponentPool
    {
        private readonly Bag<IComponentPool> _componentPools = new Bag<IComponentPool>();
        private readonly Bag<Action<PoolableComponent>> _storeComponentActions = new Bag<Action<PoolableComponent>>();

        public T Fetch<T>()
            where T : PoolableComponent, new()
        {
            return (T)this.GetPool<T>().Fetch();
        }

        public void Store<T>(T item)
          where T : PoolableComponent, new()
        {
            this.StoreInner<T>(item);
        }

        public void Store(PoolableComponent poolableComponent)
        {
            int typeID = poolableComponent.PoolableComponentTypeID;
            Action<PoolableComponent> storeAction = _storeComponentActions[typeID];
            if (storeAction == null)
            {
                storeAction = this.CreateStoreAction(poolableComponent.GetType());
                _storeComponentActions[typeID] = storeAction;
            }

            storeAction(poolableComponent);
        }

        private void StoreInner<T>(PoolableComponent item)
           where T : PoolableComponent, new()
        {
            this.GetPool<T>().Store(item);
        }

        private ComponentPoolInner<T> GetPool<T>()
            where T : PoolableComponent, new()
        {
            int typeID = PoolableComponent<T>.ID;
            ComponentPoolInner<T> poolInner = (ComponentPoolInner<T>)_componentPools[typeID];
            if (poolInner == null)
            {
                poolInner = new ComponentPoolInner<T>();
                _componentPools[typeID] = poolInner;
            }

            return poolInner;
        }

        #region Create "Store" Action

        private static readonly MethodInfo StoreInnerMethodInfo = typeof(ComponentPool).GetGenericMethod("StoreInner", BindingFlags.Instance | BindingFlags.NonPublic);
        private Action<PoolableComponent> CreateStoreAction(Type type)
        {
            MethodInfo storeMethodInfo = ComponentPool.StoreInnerMethodInfo.MakeGenericMethod(type);

            // on WP7 Delegate.CreateDelegate DOESNT WORK, so I can't use it
#if WINDOWS_PHONE
            if (OperatingSystemHelper.Version == WindowsPhoneVersion.WP7) // is WP7
            {
                object[] array = new object[1];
                return component =>
                {
                    array[0] = component;
                    storeMethodInfo.Invoke(this, array);
                };
            }

#endif
            return ReflectionHelper.CompileMethod<Action<PoolableComponent>>(this, storeMethodInfo);
        }

        #endregion

        #region ComponentPool

        private interface IComponentPool { }
        private class ComponentPoolInner<T> : IComponentPool
            where T : PoolableComponent, new()
        {
            private readonly Bag<PoolableComponent> _components = new Bag<PoolableComponent>();
            public PoolableComponent Fetch()
            {
                PoolableComponent component = _components.RemoveLast();
                if (component == null)
                {
                    component = new T { PoolableComponentTypeID = PoolableComponent<T>.ID };
                    component.Cleanup(); // is this good? without this, basically I sometimes would need to duplicate code in constructor and Cleanup.. lets see
                }

                return component;
            }

            public void Store(PoolableComponent component)
            {
                component.Cleanup();
                _components.Add(component);
            }
        }

        #endregion
    }
}
