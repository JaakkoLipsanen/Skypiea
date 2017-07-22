using System;
using Flai.CBES.Managers;
using Flai.CBES.Pools;
using Flai.CBES.Systems;
using Flai.DataStructures;
using System.Collections.Generic;

namespace Flai.CBES
{
    // todo: poolable components and entities. Make PoolableComponent class with "CleanUp" method and maybe "Init" method?

    // don't use GenericEvent because then the auto-generated name is just "args" :)
    public delegate void EntityDelegate(Entity entity);

    // lets at first try to make this a "god-class" that handles everything from the outside's POV. if it doesn't work, then lets expose all the inner managers etc
    public class EntityWorld
    {
        #region Fields and Properties

        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;
        private readonly SystemManager _systemManager;
        private readonly MessageManager _messageManager;
        private readonly EntityTrackerManager _entityTrackerManager;
        private readonly TagManager _tagManager;
        private readonly EntityRefreshManager _entityRefreshManager;
        private readonly ParameterCollectionPool _parameterCollectionPool = new ParameterCollectionPool();

        private readonly FlaiServiceContainer _services = new FlaiServiceContainer();
        // tag manager = Dict<string tag, List entities>. dunno if that useful though

        internal TagManager TagManager
        {
            get { return _tagManager; }
        }

        internal EntityManager EntityManager
        {
            get { return _entityManager; }
        }

        internal ComponentManager ComponentManager
        {
            get { return _componentManager; }
        }

        internal SystemManager SystemManager
        {
            get { return _systemManager; }
        }

        internal EntityRefreshManager EntityRefreshManager
        {
            get { return _entityRefreshManager; }
        }

        public ReadOnlyBag<Entity> AllEntities
        {
            get { return _entityManager.AllEntities; }
        }

        public FlaiServiceContainer Services
        {
            get { return _services; }
        }

        public float TotalUpdateTime { get; private set; }

        public event EntityDelegate EntityAdded;
        public event EntityDelegate EntityRemoved;
        public event EntityDelegate EntityChanged;

        #endregion

        public EntityWorld()
        {
            _entityManager = new EntityManager(this);
            _componentManager = new ComponentManager(this);
            _systemManager = new SystemManager(this);
            _messageManager = new MessageManager();
            _entityTrackerManager = new EntityTrackerManager(this);
            _tagManager = new TagManager();
            _entityRefreshManager = new EntityRefreshManager(this);

            // todo: should EntityManager even have EntityAdded/Removed?
            // todo: do these even work?
            _entityManager.EntityAdded += this.EntityAdded;
            _entityManager.EntityRemoved += this.EntityRemoved;
            _entityRefreshManager.EntityRefreshed += this.EntityChanged;
        }

        /* TODO: Meh... should these be here or expose EntityManager and SystemManager... blargh */
        #region EntityManager stuff

        public Entity CreateEntity()
        {
            return this.CreateEntity(null);
        }

        public Entity CreateEntity(string name)
        {
            return _entityManager.Create(name);
        }

        #region Create Entity From Prefab overloads

        // todo: meh.. all of these cause boxing on structs because not using generic arguments... but ParameterCollection itself doesn't use them either, so atm they'd be boxed anyways... 
        // >> I could modify ParameterCollection to not box by using Wrapper<T> : IWrapper or something but meh... I'd have to pool those wrappers too and all... blahh

        // TODO: use "ref" ?!
        public Entity CreateEntityFromPrefab<T>()
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create());
        }

        public Entity CreateEntityFromPrefab<T>(string name)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create());
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1)
           where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2, object parameter3)
           where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2, parameter3));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2, object parameter3)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2, parameter3));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2, object parameter3, object parameter4)
           where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2, object parameter3, object parameter4)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2, object parameter3, object parameter4, object parameter5)
           where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7)
           where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7, object parameter8)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7, object parameter8)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7, object parameter8, object parameter9)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8, parameter9));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7, object parameter8, object parameter9)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8, parameter9));
        }

        public Entity CreateEntityFromPrefab<T>(object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7, object parameter8, object parameter9, object parameter10)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(null, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8, parameter9, parameter10));
        }

        public Entity CreateEntityFromPrefab<T>(string name, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7, object parameter8, object parameter9, object parameter10)
            where T : Prefab, new()
        {
            return _entityManager.CreateFromPrefab<T>(name, _parameterCollectionPool.Create(parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8, parameter9, parameter10));
        }

        #endregion

        public void DeleteEntity(Entity entity)
        {
            _entityManager.DeleteEntity(entity);
        }

        public ReadOnlyBag<Entity> FindEntitiesWithTag(uint tag)
        {
            return _tagManager.GetEntitiesByTag(tag);
        }

        public Entity FindEntityByName(string name)
        {
            return _entityManager.GetEntityByName(name);
        }

        public Entity TryFindEntityByName(string name)
        {
            return _entityManager.TryGetEntityByName(name);
        }

        public IEnumerable<Entity> FindEntitiesByAspect(Aspect aspect)
        {
            return _entityManager.GetEntitiesByAspect(aspect);
        }

        // this is not that good.. if I add that components can be removed then this is useless.. and idk, seems hacky. but lets try at first
        public IEnumerable<Entity> FindEntitiesByPrefab<T>()
            where T : Prefab
        {
            return _entityManager.GetEntitiesByPrefab<T>();
        }

        #endregion

        #region Entity System Manager stuff

        public void AddSystem<T>()
           where T : EntitySystem, new()
        {
            _systemManager.Add(new T()); // event?
        }

        public void AddSystem<T>(T entitySystem)
            where T : EntitySystem
        {
            _systemManager.Add(entitySystem); // event?
        }

        public bool RemoveSystem<T>()
            where T : EntitySystem
        {
            return _systemManager.Remove<T>();
        }

        public T GetSystem<T>()
            where T : EntitySystem
        {
            return _systemManager.GetEntitySystem<T>();
        }

        #endregion

        #region Subscribe/Broadcast Message, Begin/End message-lock

        public T FetchMessage<T>()
            where T : PoolableMessage, new()
        {
            return _messageManager.FetchPoolable<T>();
        }

        public void SubscribeToMessage<T>(MessageAction<T> action)
            where T : Message
        {
            _messageManager.Subscribe<T>(action);
        }

        public bool UnsubscribeToMessage<T>(MessageAction<T> action)
            where T : Message
        {
            return _messageManager.Unsubscribe<T>(action);
        }

        public void BroadcastMessage<T>(T message)
            where T : Message
        {
            _messageManager.Broadcast<T>(message);
        }

        public void BroadcastMessageFromPool<T>()
            where T : PoolableMessage, new()
        {
            this.BroadcastMessageFromPool<T>(null);
        }

        public void BroadcastMessageFromPool<T>(Action<T> initializeAction)
            where T : PoolableMessage, new()
        {
            _messageManager.BroadcastFromPool<T>(initializeAction);
        }

        public bool HasListeners<T>()
            where T : Message
        {
            return _messageManager.HasListeners<T>();
        }

        public void BeginMessageLock()
        {
            _messageManager.BeginLock();
        }

        public void EndMessageLock()
        {
            _messageManager.EndLock();
        }

        #endregion

        #region Add/Remove EntityTracker

        public void AddEntityTracker(EntityTracker entityTracker)
        {
            _entityTrackerManager.AddTracker(entityTracker);
        }

        public bool RemoveEntityTracker(EntityTracker entityTracker)
        {
            return _entityTrackerManager.RemoveTracker(entityTracker);
        }

        #endregion

        public IEnumerable<T> GetAllComponents<T>()
            where T : Component
        {
            return _entityManager.GetAllComponents<T>();
        }

        public void Initialize()
        {
            _systemManager.Initialize();
        }

        public void Shutdown()
        {
            _systemManager.Shutdown();
        }

        // damn.. should I do Update and Draw method..? It's just that it could be a bit hard to sync entities/systems with everything else (gui etc.) ... :(
        // okay not gonna do a draw method, gonna keep the old MVC-architecture
        // maybe draw should be optional..? doable with for example attributes. though with attributes the whole ProcessOrder is dumb. though it is already
        // >> a bit dumb since the names in it are highly game dependant and otherwise sucks too. blargh, just use int?
        public void Update(UpdateContext updateContext)
        {
            this.TotalUpdateTime += updateContext.DeltaSeconds;
            _entityManager.PreUpdate(updateContext);
            _componentManager.PreUpdate(updateContext);

            _systemManager.Update(updateContext);

            _componentManager.PostUpdate(updateContext);
            _entityManager.PostUpdate(updateContext);
        }

        // should EntityManager.Create/Get etc be internal? so that all the        
    }
}
