using Flai;
using Flai.CBES;
using Flai.DataStructures;
using Microsoft.Xna.Framework;
using Skypiea.Components;

// kinda meh name
namespace Skypiea.Services
{
    public interface IZombieSpatialMap
    {
        ReadOnlyBag<Entity> GetZombiesWithinRange(ITransform2D transform, float range);
        ReadOnlyBag<Entity> GetZombiesWithinRange(Vector2 position, float range);

        ReadOnlyBag<Entity> GetZombiesWithCenterInRange(ITransform2D transform, float range);
        ReadOnlyBag<Entity> GetZombiesWithCenterInRange(Vector2 position, float range);

        ReadOnlyBag<Entity> GetZombiesIntersecting(Segment2D segment);
        ReadOnlyBag<Entity> GetZombiesIntersecting(Segment2D segment, float maxBias);

        ReadOnlyBag<Entity> GetZombiesIntersecting(RectangleF rectangleF);
    }

    // make this general instead of just zombies?
    public class ZombieSpatialMap : IZombieSpatialMap
    {
        private readonly EntityWorld _entityWorld;
        private readonly EntityTracker _zombieEntityTracker = EntityTracker.FromAspect(Aspect.All<CZombieInfo>());

        private readonly Bag<Entity> _returnEntities = new Bag<Entity>();
        private readonly ReadOnlyBag<Entity> _readOnlyReturnEntities;

        public ZombieSpatialMap(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
            _readOnlyReturnEntities = new ReadOnlyBag<Entity>(_returnEntities);

            _entityWorld.AddEntityTracker(_zombieEntityTracker);
        }

        // okay, I could make this create less allocations by not using yield return but instead return a ReadOnlyBag.. but since this is just a place holder stuff

        public ReadOnlyBag<Entity> GetZombiesWithinRange(ITransform2D transform, float range)
        {
            return this.GetZombiesWithinRange(transform.Position, range);
        }

        public ReadOnlyBag<Entity> GetZombiesWithinRange(Vector2 position, float range)
        {
            Ensure.IsValid(range);
            Ensure.True(range > 0);

            _returnEntities.Clear();

            // todo: real spatial mapping
            Circle searchCircle = new Circle(position, range);
            for (int i = 0; i < _zombieEntityTracker.Entities.Count; i++)
            {
                Entity zombie = _zombieEntityTracker.Entities[i];
                CZombieInfo zombieInfo = zombie.Get<CZombieInfo>();
                if (searchCircle.Intersects(zombieInfo.AreaCircle))
                {
                    _returnEntities.Add(zombie);
                    searchCircle.Intersects(zombieInfo.AreaCircle);
                }
            }

            return _readOnlyReturnEntities;
        }

        public ReadOnlyBag<Entity> GetZombiesWithCenterInRange(ITransform2D transform, float range)
        {
            return this.GetZombiesWithCenterInRange(transform.Position, range);
        }

        public ReadOnlyBag<Entity> GetZombiesWithCenterInRange(Vector2 position, float range)
        {
            Ensure.IsValid(range);
            Ensure.True(range > 0);

            _returnEntities.Clear();

            float rangeSquared = range * range;
            for (int i = 0; i < _zombieEntityTracker.Entities.Count; i++)
            {
                Entity zombie = _zombieEntityTracker.Entities[i];
                if (Vector2.DistanceSquared(zombie.Transform.Position, position) < rangeSquared)
                {
                    _returnEntities.Add(zombie);
                }
            }

            return _readOnlyReturnEntities;
        }

        public ReadOnlyBag<Entity> GetZombiesIntersecting(Segment2D segment)
        {
            _returnEntities.Clear();
            for (int i = 0; i < _zombieEntityTracker.Entities.Count; i++)
            {
                Entity zombie = _zombieEntityTracker.Entities[i];
                if (Segment2D.Intersects(segment, zombie.Get<CZombieInfo>().AreaCircle))
                {
                    _returnEntities.Add(zombie);
                }

            }

            return _readOnlyReturnEntities;
        }

        public ReadOnlyBag<Entity> GetZombiesIntersecting(Segment2D segment, float maxBias)
        {
            Ensure.IsValid(maxBias);
            Ensure.True(maxBias > 0);

            _returnEntities.Clear();
            for (int i = 0; i < _zombieEntityTracker.Entities.Count; i++)
            {
                Entity zombie = _zombieEntityTracker.Entities[i];
                if (Segment2D.MinimumDistance(segment, zombie.Get<CZombieInfo>().AreaCircle) < maxBias)
                {
                    _returnEntities.Add(zombie);
                }

            }

            return _readOnlyReturnEntities;
        }

        public ReadOnlyBag<Entity> GetZombiesIntersecting(RectangleF area)
        {
            _returnEntities.Clear();
            for (int i = 0; i < _zombieEntityTracker.Entities.Count; i++)
            {
                Entity zombie = _zombieEntityTracker.Entities[i];
                if (zombie.Get<CZombieInfo>().AreaCircle.Intersects(area))
                {
                    _returnEntities.Add(zombie);
                }
            }

            return _readOnlyReturnEntities;
        }
    }
}
