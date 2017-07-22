using Flai.CBES.Pools;
using Flai.DataStructures;
using Flai.General;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Flai.CBES.Managers
{
    // todo: for Create and Delete??? blaahh!!! also for Entity.Refresh? and also for BroadcastMessage?
    public enum ActionType
    {
        Instant,
        EndOfFrame,
    }

    internal class EntityManager : IEnumerable<Entity>
    {
        private readonly EntityWorld _entityWorld;
        private readonly EntityPool _entityPool = new EntityPool();

        private readonly Bag<Entity> _entities = new Bag<Entity>();
        private readonly Bag<Entity> _allEntitiesReturnBag = new Bag<Entity>(); 
        private readonly ReadOnlyBag<Entity> _readOnlyAllEntitiesReturnBag; 
        private readonly Dictionary<string, Entity> _entitiesByName = new Dictionary<string, Entity>();

        private readonly List<Entity> _entitiesToAdd = new List<Entity>();
        private readonly List<Entity> _entitiesToRemove = new List<Entity>();

        private bool _isLocked = false;

        // todo: should these be here...? or in the EntityWorld...?
        public event EntityDelegate EntityAdded;
        public event EntityDelegate EntityRemoved;

        public ReadOnlyBag<Entity> AllEntities
        {
            get
            {
                _allEntitiesReturnBag.Clear();
                for (int i = 0; i < _entities.Count; i++)
                {
                    if (_entities[i] != null)
                    {
                        _allEntitiesReturnBag.Add(_entities[i]);
                    }
                }

                return _readOnlyAllEntitiesReturnBag;
            }
        }

        public EntityManager(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
            _readOnlyAllEntitiesReturnBag = new ReadOnlyBag<Entity>(_allEntitiesReturnBag);
        }

        #region Update

        internal void PreUpdate(UpdateContext updateContext)
        {
            _isLocked = true;
        }

        internal void PostUpdate(UpdateContext updateContext)
        {
            _isLocked = false;

            this.AddNewEntities();
            this.RemoveDeletedEntities();
        }

        #endregion

        #region Create Entity

        // todo: i probably have to implement that component are allowed to be added even after the entity has been added to world
        internal Entity Create(string name)
        {
            _entityWorld.EntityRefreshManager.BeginLock();
            Entity entity = _entityPool.Fetch(_entityWorld, name);
            this.AddEntity(entity);
            _entityWorld.EntityRefreshManager.EndLock();

            return entity;
        }

        // todo: create "PrefabParamaterArray" etc and pool them!!
        internal Entity CreateFromPrefab<T>(string name, ParameterCollection parameterCollection)
            where T : Prefab, new()
        {
            _entityWorld.EntityRefreshManager.BeginLock();
            Entity entity = _entityPool.Fetch(_entityWorld, name);
            this.AddEntity(Prefab.BuildEntity<T>(_entityWorld, entity, parameterCollection));
            _entityWorld.EntityRefreshManager.EndLock();

            return entity;
        }

        internal Entity CreateFromFile<T>(BinaryReader reader, string name)
            where T : LoadablePrefab, new()
        {
            _entityWorld.EntityRefreshManager.BeginLock();
            Entity entity = _entityPool.Fetch(_entityWorld, name);
            this.AddEntity(Prefab.LoadEntityFromFile<T>(_entityWorld, entity, reader));
            _entityWorld.EntityRefreshManager.EndLock();

            return entity;
        }

        #endregion

        #region Delete Entity

        public void DeleteEntity(Entity entity)
        {
            this.RemoveEntity(entity);
        }

        #endregion

        #region Get

        public IEnumerable<T> GetAllComponents<T>()
            where T : Component
        {
            TypeMask<Component> bit = Component<T>.Bit;
            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].ComponentMask.HasBits(bit))
                {
                    yield return _entities[i].Get<T>();
                }
            }
        }

        public Entity GetEntityByName(string name)
        {
            return _entitiesByName[name];
        }

        public Entity TryGetEntityByName(string name)
        {
            return _entitiesByName.ContainsKey(name) ? _entitiesByName[name] : null;
        }

        public IEnumerable<Entity> GetEntitiesByAspect(Aspect aspect)
        {
            // meh
            for (int i = 0; i < _entities.Count; i++)
            {
                Entity entity = _entities[i];
                if (aspect.Matches(entity))
                {
                    yield return entity;
                }
            }
        }

        public IEnumerable<Entity> GetEntitiesByPrefab<T>()
           where T : Prefab
        {
            // this could be turned into manager which has Dictionary<PrefabID, Bag<Entity>>
            return _entities.Where(entity => (entity.PrefabID == Prefab<T>.ID));
        }

        #endregion

        public bool IsActive(int id)
        {
            return id < _entities.Count && _entities[id] != null;
        }

        #region Private Methods

        // todo: these are not necessarily in sync. if called delete(entity1), add(entity1), then entity is added and then deleted. but lets assume that wont happen
        // todo: >> this could be fixed by having just one list of for example Action's or struct { Entity, enum { Add, DeleteEntity } }'s
        private void AddNewEntities()
        {
            for(int i = 0; i < _entitiesToAdd.Count; i++)
            {
                this.AddInner(_entitiesToAdd[i]);
            }

            _entitiesToAdd.Clear();
        }

        private void RemoveDeletedEntities()
        {
            for (int i = 0; i < _entitiesToRemove.Count; i++)
            {
                this.RemoveInner(_entitiesToRemove[i]);
            }

            _entitiesToRemove.Clear();
        }

        private void AddEntity(Entity entity)
        {
            if (_isLocked)
            {
                Assert.False(_entitiesToAdd.Contains(entity));
                _entitiesToAdd.Add(entity);
            }
            else
            {
                this.AddInner(entity);
            }
        }

        private void RemoveEntity(Entity entity)
        {
            if (_isLocked)
            {
                Assert.False(_entitiesToRemove.Contains(entity));
                _entitiesToRemove.Add(entity);
            }
            else
            {
                this.RemoveInner(entity);
            }
        }

        private void AddInner(Entity entity)
        {
            Assert.Null(_entities[entity.EntityID]);
            _entities[entity.EntityID] = entity;
            if (entity.Name != null)
            {
                _entitiesByName.Add(entity.Name, entity);
            }

            _entityWorld.SystemManager.OnEntityAdded(entity);
            this.EntityAdded.InvokeIfNotNull(entity);
        }

        private void RemoveInner(Entity entity)
        {
            Assert.NotNull(_entities[entity.EntityID]);
            _entities[entity.EntityID] = null;
            _entityWorld.ComponentManager.RemoveAllComponents(entity);
            _entityWorld.TagManager.Unregister(entity);

            // todo: should be removed after the frame..
            if (entity.Name != null && !_entitiesByName.Remove(entity.Name))
            {
                throw new ArgumentException("entity");
            }

            // should this even be refreshed? EntitySystems could just do the logic in OnEntityRemoved instead of in OnEntityChanged
            _entityWorld.EntityRefreshManager.Refresh(entity);

            _entityWorld.SystemManager.OnEntityRemoved(entity);
            this.EntityRemoved.InvokeIfNotNull(entity);

            _entityPool.Store(entity);
        }

        #endregion

        #region Implementation of IEnumerable<T>

        public IEnumerator<Entity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
