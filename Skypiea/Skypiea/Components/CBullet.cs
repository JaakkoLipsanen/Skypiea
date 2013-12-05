using Flai;
using Flai.CBES;
using Skypiea.Model.Weapons;

namespace Skypiea.Components
{
    // returns true if the bullet was destoryed, false otherwise
    public delegate bool BulletHitCallback(CBullet cBullet, Entity entityHit);

    // todo: instead of doing something like this, how about make a callback "OnCollided(Entity entity (or Vector2 position tms))" and then the weapon itself (whos callback it will be calling) decides the damage etc. this.. could be A LOT better!
    public class CBullet : PoolableComponent
    {
        public BulletWeapon Weapon { get; private set; }
        public SizeF Size { get; private set; }

        public RectangleF Area
        {
            get { return RectangleF.CreateCentered(this.Entity.Transform.Position, this.Size); }
        }

        public bool InvokeCallback(Entity entityHit)
        {
            return this.Weapon.OnBulletHitCallback(this, entityHit);
        }

        public void Initialize(SizeF size, BulletWeapon weapon)
        {
            this.Size = size;
            this.Weapon = weapon;
        }

        protected override void Cleanup()
        {
            this.Size = SizeF.Empty;
            this.Weapon = null;
        }
    }
}
