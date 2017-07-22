#if CBES_3D
using DefaultTransformComponent = Flai.CBES.Components.CTransform3D;
#else
using DefaultTransformComponent = Flai.CBES.Components.CTransform2D;
#endif

using Flai.DataStructures;

namespace Flai.CBES.Pools
{
    internal class EntityPool
    {
        private readonly Bag<Entity> _availableEntities = new Bag<Entity>();
        private int _nextEntityID = 0;
        private uint _nextUniqueID = 0;

        public Entity Fetch(EntityWorld entityWorld, string name)
        {
            Entity entity = _availableEntities.RemoveLast() ?? new Entity(entityWorld, _nextEntityID++);

            entity.Name = name.TrimIfNotNull().NullIfEmpty(); // hmm.. should this trim? since that can cause errors if editor doesnt trim.. hmm..
            entity.UniqueID = _nextUniqueID++;
            entity.Transform = entity.AddFromPool<DefaultTransformComponent>();        

            return entity;
        }

        public void Store(Entity entity)
        {
            entity.Reset();
            _availableEntities.Add(entity);
        }
    }
}
