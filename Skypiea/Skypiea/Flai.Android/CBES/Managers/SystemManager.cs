using Flai.CBES.Systems;
using System.Collections.Generic;
using System.Linq;
using Flai.DataStructures;

namespace Flai.CBES.Managers
{
    internal class SystemManager
    {
        private readonly EntityWorld _entityWorld;
        private readonly List<EntitySystem> _systems = new List<EntitySystem>();
        private bool _isInitialized = false;

        public SystemManager(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
        }

        public void Add<T>(T entitySystem)
            where T : EntitySystem
        {
            // preserve the order (!! not sure if works !!)
            _systems.Add(entitySystem);
            _systems.StableSort(EntitySystemHelper.CompareOrder);

            entitySystem.SystemBit = EntitySystem<T>.Bit;
            if (_isInitialized)
            {
                entitySystem.EntityWorld = _entityWorld;
                entitySystem.OnPreInitialize();
                entitySystem.OnInitialize();
            }
        }

        public bool Remove<T>()
            where T : EntitySystem
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                if (_systems[i].GetType() == typeof (T))
                {
                    _systems.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public T GetEntitySystem<T>()
            where T : EntitySystem
        {
            return (T)_systems.First(system => system.SystemBit == EntitySystem<T>.Bit); // First vs FirstOrDefault?
        }

        public void Initialize()
        {
            // Pre-initialize
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].EntityWorld = _entityWorld;
                _systems[i].OnPreInitialize();
            }

            // Initialize
            for(int i = 0; i < _systems.Count; i++)
            {
                _systems[i].OnInitialize();
            }

            _isInitialized = true;
        }

        public void Shutdown()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].OnShutdown();
            }
        }

        public void Update(UpdateContext updateContext)
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].OnUpdate(updateContext);
            }
        }

        internal void OnEntityAdded(Entity entity)
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].OnEntityAddedToWorld(entity);
            }
        }

        internal void OnEntityRemoved(Entity entity)
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].OnEntityRemovedFromWorld(entity);
            }
        }

        internal void OnEntityChanged(Entity entity)
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].OnEntityChangedInWorld(entity);
            }   
        }
    }
}
