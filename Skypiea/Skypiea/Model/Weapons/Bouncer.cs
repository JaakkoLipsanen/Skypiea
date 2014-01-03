using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class Bouncer : BulletWeapon
    {
        private const int BulletCount = 30;
        public override WeaponType Type
        {
            get { return WeaponType.Bouncer; }
        }

        public Bouncer(float ammoMultiplier)
            : base((int)(Bouncer.BulletCount * ammoMultiplier), 0.575f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<BouncerBulletPrefab>(playerEntity.Transform, this, -0.125f);
            playerEntity.EntityWorld.CreateEntityFromPrefab<BouncerBulletPrefab>(playerEntity.Transform, this, 0.125f);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            CBouncerBullet bouncerBullet = bullet.Entity.Get<CBouncerBullet>();
            if (bouncerBullet.HasHit(entityHit))
            {
                return false;
            }

            CVelocity2D velocity = bullet.Entity.Get<CVelocity2D>();
            if (bouncerBullet.OnHit(entityHit))
            {
                ZombieHelper.TakeDamage(entityHit, 20, velocity.Velocity / 3f);
                velocity.Direction = velocity.Direction.Rotate(Global.Random.NextFloat(-1f, 1f)); // change the direction to random
                return false;
            }

            bullet.Entity.Delete();
            return true;
        }
    }  
}
