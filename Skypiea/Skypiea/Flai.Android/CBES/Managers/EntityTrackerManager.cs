using System.Collections.Generic;

namespace Flai.CBES.Managers
{
    internal class EntityTrackerManager
    {
        private readonly EntityWorld _entityWorld;
        private readonly List<EntityTracker> _entityTrackers = new List<EntityTracker>();

        public EntityTrackerManager(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
            _entityWorld.EntityAdded += this.OnEntityAdded; // ..
            _entityWorld.EntityChanged += this.OnEntityChanged; // ..
            _entityWorld.EntityRemoved += this.OnEntityRemoved; // ..
        }

        public void AddTracker(EntityTracker entityTracker)
        {
            _entityTrackers.Add(entityTracker);

            for(int i = 0; i < _entityWorld.AllEntities.Count; i++)
            {
                entityTracker.OnEntityAddedToWorld(_entityWorld.AllEntities[i]);
            }
        }

        public bool RemoveTracker(EntityTracker entityTracker)
        {
            return _entityTrackers.Remove(entityTracker);
        }

        private void OnEntityAdded(Entity entity)
        {
            for(int i = 0; i < _entityTrackers.Count; i++)
            {
                _entityTrackers[i].OnEntityAddedToWorld(entity);
            }
        }

        private void OnEntityChanged(Entity entity)
        {
            for (int i = 0; i < _entityTrackers.Count; i++)
            {
                _entityTrackers[i].OnEntityChangedInWorld(entity);
            }
        }

        private void OnEntityRemoved(Entity entity)
        {
            for (int i = 0; i < _entityTrackers.Count; i++)
            {
                _entityTrackers[i].OnEntityRemovedFromWorld(entity);
            }
        }
    }
}
