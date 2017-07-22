using Flai.CBES.Pools;
using Flai.DataStructures;
using Flai.General;
using System;
using System.Collections.Generic;

namespace Flai.CBES.Managers
{
    internal class ComponentManager
    {
        private readonly EntityWorld _entityWorld;
        private readonly ComponentPool _componentPool = new ComponentPool();
        private readonly Bag<Bag<Component>> _componentsByType = new Bag<Bag<Component>>();

        internal ComponentManager(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
        }

        #region Update

        // meh, could be a bit slow
        internal void PreUpdate(UpdateContext updateContext)
        {
            for (int i = 0; i < _componentsByType.Count; i++)
            {
                Bag<Component> components = _componentsByType.GetRaw(i);
                if (components == null)
                {
                    continue;
                }

                for (int j = 0; j < components.Count; j++)
                {
                    Component component = components.GetRaw(j);
                    if (component != null)
                    {
                        component.PreUpdate(updateContext);
                    }
                }
            }
        }

        // todo: PROBLEM WITH THESE BOTH: component's parent entity could be "IsActive" == false when these updates are made.. and thats not good!

        // meh, could be a bit slow
        internal void PostUpdate(UpdateContext updateContext)
        {
            for (int i = 0; i < _componentsByType.Count; i++)
            {
                Bag<Component> components = _componentsByType.GetRaw(i);
                if (components == null)
                {
                    continue;
                }

                for (int j = 0; j < components.Count; j++)
                {
                    Component component = components.GetRaw(j);
                    if (component != null)
                    {
                        component.PostUpdate(updateContext);
                    }
                }
            }
        }

        #endregion

        internal T AddComponentFromPool<T>(Entity entity)
            where T : PoolableComponent, new()
        {
            T component = _componentPool.Fetch<T>();
            this.AddComponent(entity, component);

            return component;
        }

        internal void AddComponent<T>(Entity entity, T component)
            where T : Component
        {
            Assert.Null(component.Entity);

            int componentID = Component<T>.ID;
            if (componentID >= _componentsByType.Capacity)
            {
                _componentsByType[componentID] = null;
            }

            Bag<Component> components = _componentsByType[componentID];
            if (components == null)
            {
                components = new Bag<Component>();
                _componentsByType[componentID] = components;
            }

            components[entity.EntityID] = component;
            component.Entity = entity;
            entity.AddComponentBit<T>();
            component.Initialize();

            // okay.. when using prefabs, this causes the entity to *NOT* send ton of refresh messages..
            // however, when not using entities/adding components after entity is added to world, this will still send a lot of refresh messages.
           // if (entity.IsActive) // todo: i added locks to EntityManager.Create(From File/Prefab), so it shouldn't send a lot of refreshs even without this if...
            {
                _entityWorld.EntityRefreshManager.Refresh(entity);
            }
        }

        internal T GetComponent<T>(Entity entity)
            where T : Component
        {
         // Debug.Assert(entity.IsActive); // meh, can't have this. otherwise if Component.Initialize calls Entity.Get<T>, then this will fail the assert
            if (Component<T>.ID >= _componentsByType.Count)
            {
                throw new ArgumentException("Invalid generic-argument");
            }

            Bag<Component> components = _componentsByType[Component<T>.ID];
            if (components != null && entity.EntityID < components.Count)
            {
                T component = (T)components[entity.EntityID];
                if (component != null)
                {
                    return component;
                }
            }

            throw new ArgumentException("Invalid generic-argument");
        }

        public IEnumerable<Component> GetAllComponents(Entity entity)
        {
            TypeMask<Component> componentMask = entity.ComponentMask;
            for (int i = 0; i < _componentsByType.Count; i++)
            {
                // todo: these bit checks aren't necessary, you could just probably check if _componentsByType[i][entity.EntityID] is null or not..?
                ulong componentBit = 1UL << i;
                if (componentMask.HasBits(componentBit))
                {
                    yield return _componentsByType[i][entity.EntityID];
                }
            }
        }

        // same as GetComponent<T> but returns null if not found
        internal T TryGetComponent<T>(Entity entity)
           where T : Component
        {
          // Debug.Assert(entity.IsActive); // meh, can't have this. otherwise if Component.OnAttachedToParent calls ParentEntity.Get<T>, then this will fail the assert
            if (Component<T>.ID >= _componentsByType.Count)
            {
                return null;
            }

            Bag<Component> components = _componentsByType[Component<T>.ID];
            if (components != null && entity.EntityID < components.Count)
            {
                return (T)components[entity.EntityID];
            }

            return null;
        }

        internal IEnumerable<Component> GetComponents(Entity entity)
        {
            for(int i = 0; i < _componentsByType.Count; i++)
            {
                Bag<Component> components = _componentsByType[i];
                if (components != null && entity.EntityID < components.Count)
                {
                    Component component = components[entity.EntityID];
                    if (component != null)
                    {
                        yield return component;
                    }
                }
            }
        }

        internal bool RemoveComponent<T>(Entity entity)
            where T : Component
        {
            if (Component<T>.ID >= _componentsByType.Count)
            {
                return false;
            }

            Bag<Component> components = _componentsByType[(int)Component<T>.ID];
            if (components != null && entity.EntityID < components.Count)
            {
                Component removedComponent = components[entity.EntityID]; // for ComponentRemoved event?? currently only EntityWorld.ComponentChanged
                removedComponent.Entity = null;
                components[entity.EntityID] = null;
                entity.RemoveComponentBit<T>();
                _entityWorld.EntityRefreshManager.Refresh(entity);

                if (Component<T>.IsPoolable)
                {
                    _componentPool.Store((PoolableComponent)removedComponent);
                }

                return true;
            }

            return false;
        }

        internal void RemoveAllComponents(Entity entity)
        {
            for (int i = _componentsByType.Count - 1; i >= 0; i--)
            {
                Bag<Component> components = _componentsByType[i];
                if (components != null && entity.EntityID < components.Count)
                {
                    Component component = components[entity.EntityID];
                    components[entity.EntityID] = null;

                    if (component != null)
                    {
                        component.Entity = null; 

                        PoolableComponent poolableComponent = component as PoolableComponent;
                        if (poolableComponent != null)
                        {
                            _componentPool.Store(poolableComponent);
                        }
                    }  
                }
            }

            entity.ResetComponentMask();

            // should this even be refreshed? EntitySystems could just do the logic in OnEntityRemoved instead of in OnEntityChanged
            _entityWorld.EntityRefreshManager.Refresh(entity);
        }
    }
}
