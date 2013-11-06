using Flai;
using Flai.CBES;

namespace Zombie.Components
{
    // returns true if the bullet was destoryed, false otherwise
    public delegate bool BulletHitCallback(BulletComponent bullet, Entity entityHit);

    // todo: instead of doing something like this, how about make a callback "OnCollided(Entity entity (or Vector2 position tms))" and then the weapon itself (whos callback it will be calling) decides the damage etc. this.. could be A LOT better!
    public class BulletComponent : Component
    {
        private readonly SizeF _size;
        private readonly BulletHitCallback _callback;

        public SizeF Size
        {
            get { return _size; }
        }

        public RectangleF Area
        {
            get { return RectangleF.CreateCentered(this.Parent.Get<TransformComponent>().Position, _size); }
        }

        public BulletComponent(SizeF size, BulletHitCallback callback)
        {
            _size = size;
            _callback = callback;
        }

        public bool InvokeCallback(Entity entityHit)
        {
            return _callback(this, entityHit);
        }
    }
}
