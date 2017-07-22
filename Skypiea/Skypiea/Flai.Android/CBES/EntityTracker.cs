using Flai.DataStructures;
using System.Collections;
using System.Collections.Generic;

namespace Flai.CBES
{
    // todo: "ComponentTracker"? Tracks components straight away instead of entities, and gives the users the components straight away. more high-performance solution maybe.
    // todo: should OnEntityAddedInternal be non-internal and should these two classes be inheritable by user?
    public class EntityTracker : IEnumerable<Entity>
    {
        // meh... using hashsets would be faster on "Contains", but on the other hand slower otherwise... hmh..
        private readonly Bag<Entity> _entities = new Bag<Entity>();
        private readonly ReadOnlyBag<Entity> _readOnlyEntities;
        protected readonly Aspect _aspect;

        public ReadOnlyBag<Entity> Entities
        {
            get { return _readOnlyEntities; }
        }

        public int Count
        {
            get { return _entities.Count; }
        }

        public event EntityDelegate EntityAdded;
        public event EntityDelegate EntityChanged; // needed..?
        public event EntityDelegate EntityRemoved;

        internal EntityTracker(Aspect aspect)
        {
            _aspect = aspect;
            _readOnlyEntities = new ReadOnlyBag<Entity>(_entities);
        }

        public bool Contains(Entity entity)
        {
            return _entities.Contains(entity);
        }

        internal void OnEntityAddedToWorld(Entity entity)
        {
            Assert.False(_entities.Contains(entity));
            if (_aspect.Matches(entity))
            {
                this.Add(entity);
            }
        }

        internal void OnEntityChangedInWorld(Entity entity)
        {
            bool contains = this.Contains(entity);
            bool interests = _aspect.Matches(entity);

            if (contains && !interests)
            {
                this.Remove(entity);
            }
            else if (!contains && interests)
            {
                this.Add(entity);
            }
            else
            {
                this.EntityChanged.InvokeIfNotNull(entity);
            }
        }

        internal void OnEntityRemovedFromWorld(Entity entity)
        {
            if (this.Contains(entity))
            {
                this.Remove(entity);
            }
        }

        private void Add(Entity entity)
        {
            _entities.Add(entity);
            this.EntityAdded.InvokeIfNotNull(entity);
        }

        private void Remove(Entity entity)
        {
            _entities.Remove(entity);
            this.EntityRemoved.InvokeIfNotNull(entity);
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        public Entity[] ToArray()
        {
            return _entities.ToArray();
        }

        public static EntityTracker FromAspect(Aspect aspect)
        {
            return new EntityTracker(aspect);
        }
    }
}
