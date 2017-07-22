using Flai.DataStructures;

namespace Flai.CBES.Systems
{
    public abstract class ProcessingSystem : EntitySystem
    {
        private readonly Bag<Entity> _entities = new Bag<Entity>();
        private readonly ReadOnlyBag<Entity> _readOnlyEntities;
        protected readonly Aspect _aspect;

        protected virtual bool ShouldProcess
        {
            get { return true; }
        }

        protected ProcessingSystem(Aspect aspect)
        {
            _aspect = aspect;
            _readOnlyEntities = new ReadOnlyBag<Entity>(_entities);
        }

        internal override void InternalPreInitialize()
        {
            if (_aspect.IsEmpty)
            {
                return;
            }

            foreach (Entity entity in base.EntityWorld.FindEntitiesByAspect(_aspect))
            {
                if (!entity.SystemMask.HasBits(this.SystemBit)) // if entity has this systems bit, then this entity *should be* in _entities
                {
                    this.Add(entity);
                }
            }
        }

        protected sealed override void Update(UpdateContext updateContext)
        {
            if (this.ShouldProcess)
            {
                this.EntityWorld.EntityRefreshManager.BeginLock();
                this.Process(updateContext, _readOnlyEntities);
                this.EntityWorld.EntityRefreshManager.EndLock();
            }
        }
        protected virtual void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities) { }

        internal override sealed void OnEntityAddedToWorld(Entity entity)
        {
            // pretty much pointless since almost never does a system care about an empty entity.. and even if it does, pretty much always the entity is changed (added components/tag).
            // lets just use Debug.Assert for now..
            if (_aspect.Matches(entity))
            {
                this.Add(entity);
            }
        }

        internal override sealed void OnEntityRemovedFromWorld(Entity entity)
        {
            // pretty much pointless, since all components are removed and tag is removed before entity is actually removed from the world
            // only needed for super-corner cases.. lets use Debug.Assert for now
            if (entity.SystemMask.HasBits(this.SystemBit))
            {
                this.Remove(entity);
            }
        }

        internal override sealed void OnEntityChangedInWorld(Entity entity)
        {
            bool contains = entity.SystemMask.HasBits(this.SystemBit);
            bool interests = _aspect.Matches(entity);
            if (!contains && interests)
            {
                this.Add(entity);
            }
            else if (contains && !interests)
            {
                this.Remove(entity);
            }
            else
            {
                this.OnEntityChanged(entity);
            }
        }

        private void Add(Entity entity)
        {
            Assert.False(_entities.Contains(entity) || entity.SystemMask.HasBits(this.SystemBit));

            _entities.Add(entity); // assume that doesn't contain, shouldn't be possible
            entity.AddSystemBit(this.SystemBit);
            this.OnEntityAdded(entity);
        }

        private void Remove(Entity entity)
        {
            Assert.True(_entities.Contains(entity) && entity.SystemMask.HasBits(this.SystemBit));

            _entities.Remove(entity);
            entity.RemoveSystemBit(this.SystemBit);
            this.OnEntityRemoved(entity); 
        }

        protected virtual void OnEntityAdded(Entity entity) { }
        protected virtual void OnEntityRemoved(Entity entity) { }
        protected virtual void OnEntityChanged(Entity entity) { }
    }
}
 