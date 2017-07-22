using Flai.DataStructures;
using System.Collections.Generic;

namespace Flai.CBES.Managers
{
    internal class TagManager 
    {
        private readonly Dictionary<uint, EntityCollection> _entitiesByTag = new Dictionary<uint, EntityCollection>();
        private readonly Dictionary<Entity, uint?> _tagsByEntities = new Dictionary<Entity, uint?>();

        public ReadOnlyBag<Entity> GetEntitiesByTag(uint tag)
        {
            return this.GetCollectionSafe(tag).ReadOnlyEntities;
        }

        public uint? GetTagOfEntity(Entity entity)
        {
            return _tagsByEntities.TryGetValue(entity); // null if doesn't exist
        }

        public void Register(Entity entity, uint tag)
        {
            this.GetCollectionSafe(tag).Entities.Add(entity);
            _tagsByEntities.Add(entity, tag);
        }

        public void Unregister(Entity entity)
        {
            uint? tag = this.GetTagOfEntity(entity);
            if (tag != null)
            {
                this.GetCollectionSafe(tag.Value).Entities.Remove(entity);
                _tagsByEntities.Remove(entity);
            }
        }

        private EntityCollection GetCollectionSafe(uint tag)
        {
            EntityCollection collection = _entitiesByTag.TryGetValue(tag);
            if (collection == null)
            {
                 collection = new EntityCollection();
                _entitiesByTag.Add(tag, collection);
            }

            return collection;
        }

        private class EntityCollection
        {
            public readonly Bag<Entity> Entities = new Bag<Entity>(); // hmm.. does this work with Bag just like this? i think it does..? but not sure
            public readonly ReadOnlyBag<Entity> ReadOnlyEntities;

            public EntityCollection()
            {
                this.ReadOnlyEntities = new ReadOnlyBag<Entity>(this.Entities);
            }
        }
    }
}
