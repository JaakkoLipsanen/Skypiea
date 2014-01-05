using System;
using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.DataStructures;
using Flai.Diagnostics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Systems.Zombie
{
    public interface IZombieSpatialMap
    {
        ReadOnlyBag<Entity> GetAllIntersecting(ITransform2D transform, float range);
        ReadOnlyBag<Entity> GetAllIntersecting(Vector2 position, float range);

        ReadOnlyBag<Entity> GetZombiesWithCenterInRange(ITransform2D transform, float range);
        ReadOnlyBag<Entity> GetZombiesWithCenterInRange(Vector2 position, float range);

        ReadOnlyBag<Entity> GetAllIntersecting(Segment2D segment);
        ReadOnlyBag<Entity> GetAllIntersecting(Segment2D segment, float maxBias);

        ReadOnlyBag<Entity> GetAllIntersecting(RectangleF rectangleF);
    }

    // make this general instead of just zombies?
    public class ZombieSpatialMapSystem : EntitySystem, IZombieSpatialMap
    {
        private readonly EntityTracker _zombieEntityTracker = EntityTracker.FromAspect(Aspect.All<CZombieInfo>());
        private readonly ZombieGrid _zombieGrid = new ZombieGrid();

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PostUpdate; }
        }

        protected override void PreInitialize()
        {
            this.EntityWorld.Services.Add<IZombieSpatialMap>(this);
        }

        protected override void Initialize()
        {
            this.EntityWorld.AddEntityTracker(_zombieEntityTracker);
            _zombieEntityTracker.EntityRemoved += entity => _zombieGrid.OnZombieRemoved(entity);
        }

        protected override void Update(UpdateContext updateContext)
        {
            _zombieGrid.Update(updateContext, _zombieEntityTracker.Entities);
        }

        #region Implementation of IZombieSpatialMap

        // okay, I could make this create less allocations by not using yield return but instead return a ReadOnlyBag.. but since this is just a place holder stuff

        public ReadOnlyBag<Entity> GetAllIntersecting(ITransform2D transform, float range)
        {
            return this.GetAllIntersecting(transform.Position, range);
        }

        public ReadOnlyBag<Entity> GetAllIntersecting(Vector2 position, float range)
        {
            Ensure.IsValid(range);
            Ensure.True(range > 0);

            Circle searchCircle = new Circle(position, range);
            return _zombieGrid.GetAllIntersecting(searchCircle);
        }

        public ReadOnlyBag<Entity> GetAllIntersecting(Segment2D segment, float maxBias)
        {
            Ensure.IsValid(maxBias);
            Ensure.True(maxBias > 0);

            return _zombieGrid.GetAllIntersecting(segment, maxBias);
        }

        public ReadOnlyBag<Entity> GetZombiesWithCenterInRange(ITransform2D transform, float range)
        {
            return this.GetZombiesWithCenterInRange(transform.Position, range);
        }

        public ReadOnlyBag<Entity> GetZombiesWithCenterInRange(Vector2 position, float range)
        {
            Assert.IsValid(range);
            Assert.True(range > 0);

            return _zombieGrid.GetAllWithinRange(position, range);
        }

        public ReadOnlyBag<Entity> GetAllIntersecting(Segment2D segment)
        {
            throw new NotImplementedException();

            //_returnEntities.Clear();
            //for (int i = 0; i < _zombieEntityTracker.Entities.Count; i++)
            //{
            //    Entity zombie = _zombieEntityTracker.Entities[i];
            //    if (Segment2D.Intersects(segment, zombie.Get<CZombieInfo>().AreaCircle))
            //    {
            //        _returnEntities.Add(zombie);
            //    }

            //}

            //return _readOnlyReturnEntities;
        }

        public ReadOnlyBag<Entity> GetAllIntersecting(RectangleF area)
        {
            Ensure.IsValid(area);
            return _zombieGrid.GetAllIntersecting(area);
        }

        #endregion

        #region ZombieGrid

        private class ZombieGrid
        {
            private static readonly Size GridSize = new Size(SkypieaConstants.MapWidth / 4 + 2, SkypieaConstants.MapHeight / 3 + 2);
            private static readonly SizeF CellSize = new SizeF(SkypieaConstants.MapWidthInPixels / (SkypieaConstants.MapWidth / 4), SkypieaConstants.MapHeightInPixels / (SkypieaConstants.MapHeight / 3));
            private readonly Bag<Entity>[] _cells;

            private readonly Bag<Entity> _returnEntities = new Bag<Entity>();
            private readonly ReadOnlyBag<Entity> _readOnlyReturnEntities;

            public ZombieGrid()
            {
                _cells = new Bag<Entity>[ZombieGrid.GridSize.Width * ZombieGrid.GridSize.Height];
                for (int i = 0; i < _cells.Length; i++)
                {
                    _cells[i] = new Bag<Entity>();
                }

                _readOnlyReturnEntities = new ReadOnlyBag<Entity>(_returnEntities);
            }

            public void Update(UpdateContext updateContext, ReadOnlyBag<Entity> zombies)
            {
                for (int i = 0; i < _cells.Length; i++)
                {
                    _cells[i].Clear();
                }

                for (int i = 0; i < zombies.Count; i++)
                {
                    Vector2i index = this.GetIndex(zombies[i].Transform.Position);
                    if (index.X >= 0 && index.Y >= 0 && index.X < ZombieGrid.GridSize.Width && index.Y < ZombieGrid.GridSize.Height)
                    {
                        this.GetCell(index.X, index.Y).Add(zombies[i]);
                    }
                }
            }

            public ReadOnlyBag<Entity> GetAllIntersecting(Circle searchCircle)
            {
                _returnEntities.Clear();

                // okay these -1 and +1 are just hacks to make sure that the zombie AREA intersect with searchCircle instead of just zombie center points
                int left = (int)FlaiMath.Max(0, this.GetIndexX(searchCircle.Left) - 1);
                int right = (int)FlaiMath.Min(ZombieGrid.GridSize.Width - 1, FlaiMath.Ceiling(this.GetIndexX(searchCircle.Right)) + 1);
                int top = (int)FlaiMath.Max(0, this.GetIndexY(searchCircle.Top) - 1);
                int bottom = (int)FlaiMath.Min(ZombieGrid.GridSize.Height - 1, FlaiMath.Ceiling(this.GetIndexY(searchCircle.Bottom)) + 1);

                for (int y = top; y <= bottom; y++)
                {
                    for (int x = left; x <= right; x++)
                    {
                        foreach (Entity zombie in this.GetCell(x, y))
                        {
                            CZombieInfo zombieInfo = zombie.Get<CZombieInfo>();
                            if (zombieInfo.AreaCircle.Intersects(searchCircle))
                            {
                                _returnEntities.Add(zombie);
                            }
                        }
                    }
                }

                return _readOnlyReturnEntities;
            }

            public ReadOnlyBag<Entity> GetAllIntersecting(RectangleF searchArea)
            {
                _returnEntities.Clear();

                int left = (int)FlaiMath.Max(0, this.GetIndexX(searchArea.Left) - 1);
                int right = (int)FlaiMath.Min(ZombieGrid.GridSize.Width - 1, FlaiMath.Ceiling(this.GetIndexX(searchArea.Right)) + 1);
                int top = (int)FlaiMath.Max(0, this.GetIndexY(searchArea.Top) - 1);
                int bottom = (int)FlaiMath.Min(ZombieGrid.GridSize.Height - 1, FlaiMath.Ceiling(this.GetIndexY(searchArea.Bottom)) + 1);

                for (int y = top; y <= bottom; y++)
                {
                    for (int x = left; x <= right; x++)
                    {
                        // "zombie is far enough from the search border, lets assume that it intersects with the searchArea"
                        if (x > left + 1 && x < right - 2 && y > top + 1 && y < bottom - 2)
                        {
                            _returnEntities.AddAll(this.GetCell(x, y));
                        }
                        else
                        {
                            // otherwise check if the zombie intersects
                            foreach (Entity zombie in this.GetCell(x, y))
                            {
                                CZombieInfo zombieInfo = zombie.Get<CZombieInfo>();
                                if (zombieInfo.AreaRectangle.Intersects(searchArea))
                                {
                                    _returnEntities.Add(zombie);
                                }
                            }
                        }
                    }
                }

                return _readOnlyReturnEntities;
            }

            public ReadOnlyBag<Entity> GetAllIntersecting(Segment2D segment, float maxBias)
            {
                _returnEntities.Clear();

                Vector2 min = Vector2.Min(segment.Start, segment.End) - Vector2.One * maxBias;
                Vector2 max = Vector2.Max(segment.Start, segment.End) + Vector2.One * maxBias;

                int left = (int)FlaiMath.Max(0, this.GetIndexX(min.X));
                int right = (int)FlaiMath.Min(ZombieGrid.GridSize.Width - 1, FlaiMath.Ceiling(this.GetIndexX(max.X)));
                int top = (int)FlaiMath.Max(0, this.GetIndexY(min.Y));
                int bottom = (int)FlaiMath.Min(ZombieGrid.GridSize.Height - 1, this.GetIndexY(max.Y));

                for (int y = top; y <= bottom; y++)
                {
                    for (int x = left; x <= right; x++)
                    {
                        // -1 because the first grid is BEFORE the map!
                        RectangleF rectangle = new RectangleF((x - 1) * ZombieGrid.CellSize.Width, (y - 1) * ZombieGrid.CellSize.Height, ZombieGrid.CellSize.Width, ZombieGrid.CellSize.Height);
                        if (Segment2D.MinimumDistance(segment, rectangle) > maxBias)
                        {
                            continue;
                        }

                        foreach (Entity zombie in this.GetCell(x, y))
                        {
                            CZombieInfo zombieInfo = zombie.Get<CZombieInfo>();
                            if (Segment2D.MinimumDistance(segment, zombieInfo.AreaCircle) < maxBias)
                            {
                                _returnEntities.Add(zombie);
                            }
                        }
                    }
                }

                return _readOnlyReturnEntities;
            }

            // okay this is be very "(micro :P -)optimized" since this is the biggest bottleneck (zombie separation uses this)
            public ReadOnlyBag<Entity> GetAllWithinRange(Vector2 position, float range)
            {
                _returnEntities.Clear();

                /* "bounds" checking inlined */
                int left = (int)this.GetIndexX(position.X - range);
                if (left < 0)
                {
                    left = 0;
                }

                int right = (int)Math.Ceiling(this.GetIndexX(position.X + range));
                if (right >= ZombieGrid.GridSize.Width)
                {
                    right = ZombieGrid.GridSize.Width - 1;
                }

                int top = (int)this.GetIndexY(position.Y - range);
                if (top < 0)
                {
                    top = 0;
                }

                int bottom = (int)Math.Ceiling(this.GetIndexY(position.Y + range));
                if (bottom >= ZombieGrid.GridSize.Height)
                {
                    bottom = ZombieGrid.GridSize.Height - 1;
                }

                float rangeSquared = range * range;
                for (int y = top; y <= bottom; y++)
                {
                    for (int x = left; x <= right; x++)
                    {
                        Bag<Entity> cell = this.GetCell(x, y);
                        for (int i = 0; i < cell.Count; i++)
                        {
                            Entity zombie = cell.GetRaw(i); // GetRaw == no unnecessary checks

                            // Vector2.DistanceSquared inlined
                            // Transform.LocalPosition is faster than Transform.Position
                            float xDelta = zombie.Transform.LocalPosition.X - position.X;
                            float yDelta = zombie.Transform.LocalPosition.Y - position.Y;

                            // Circle.Contains inlined
                            if (xDelta * xDelta + yDelta * yDelta < rangeSquared)
                            {
                                _returnEntities.Add(zombie);
                            }
                        }
                    }
                }

                return _readOnlyReturnEntities;
            }

            public void OnZombieRemoved(Entity entity)
            {
                // local position, because Position doesn't work (it is already detached from the parent entity, so it would throw an exception)
                Vector2i index = this.GetIndex(entity.Transform.LocalPosition);

                // check usual cases
                if (this.TryRemove(entity, index.X, index.Y) ||
                    this.TryRemove(entity, index.X - 1, index.Y) || this.TryRemove(entity, index.X, index.Y - 1) ||
                    this.TryRemove(entity, index.X + 1, index.Y) || this.TryRemove(entity, index.X, index.Y + 1) ||
                    this.TryRemove(entity, index.X - 1, index.Y - 1) || this.TryRemove(entity, index.X - 1, index.Y + 1) ||
                    this.TryRemove(entity, index.X + 1, index.Y - 1) || this.TryRemove(entity, index.X + 1, index.Y + 1))
                {
                    return;
                }

                // check other cases (probably never happens, but just to be sure)
                for (int i = 0; i < _cells.Length; i++)
                {
                    if (_cells[i].Remove(entity))
                    {
                        return;
                    }
                }

                // otherwise it just isn't in here
            }

            private bool TryRemove(Entity entity, int x, int y)
            {
                if (Check.WithinRange(x, 0, ZombieGrid.GridSize.Width - 1) && Check.WithinRange(y, 0, ZombieGrid.GridSize.Height - 1))
                {
                    return this.GetCell(x, y).Remove(entity);
                }

                return false;
            }

            private Bag<Entity> GetCell(int x, int y)
            {
                return _cells[x + y * ZombieGrid.GridSize.Width];
            }

            // meh since returns float but whatever
            private float GetIndexX(float position)
            {
                position += ZombieGrid.CellSize.Width;
                return position / ZombieGrid.CellSize.Width;
            }

            private float GetIndexY(float position)
            {
                position += ZombieGrid.CellSize.Height;
                return position / ZombieGrid.CellSize.Height;
            }

            private Vector2i GetIndex(Vector2 position)
            {
                position += ZombieGrid.CellSize;
                return new Vector2i(position / ZombieGrid.CellSize);
            }
        }

        #endregion
    }
}
