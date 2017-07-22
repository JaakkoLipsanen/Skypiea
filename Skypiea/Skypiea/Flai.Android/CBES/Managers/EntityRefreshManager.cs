using Flai.General;
using System.Collections.Generic;

namespace Flai.CBES.Managers
{
    internal class EntityRefreshManager
    {
        private readonly EntityWorld _entityWorld;
        private readonly HashSet<Entity> _entitiesToRefresh = new HashSet<Entity>();
        private readonly Lock _lock = new Lock();

        public event EntityDelegate EntityRefreshed;

        public EntityRefreshManager(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
        }

        public void Refresh(Entity entity)
        {
            if (_lock.IsOpen)
            {
                this.InnerRefresh(entity);
            }
            else
            {
                _entitiesToRefresh.Add(entity);
            }
        }

        public void BeginLock()
        {
            _lock.Increase();
        }

        public void EndLock()
        {
            _lock.Decrease();
            if (_lock.IsOpen)
            {
                foreach (Entity entity in _entitiesToRefresh)
                {
                    this.InnerRefresh(entity);
                }

                _entitiesToRefresh.Clear();
            }
        }

        private void InnerRefresh(Entity entity)
        {
            if (entity.IsActive)
            {
                _entityWorld.SystemManager.OnEntityChanged(entity);
                this.EntityRefreshed.InvokeIfNotNull(entity);              
            }
        }
    }
}
