using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class RicochetGun : BulletWeapon
    {
        public override WeaponType Type
        {
            get { return WeaponType.Ricochet; }
        }

        public RicochetGun()
            : base(40, 0.5f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<RicochetBulletPrefab>(playerEntity.Transform, this, -0.125f);
            playerEntity.EntityWorld.CreateEntityFromPrefab<RicochetBulletPrefab>(playerEntity.Transform, this, 0.125f);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            CRicochetBullet ricochetBullet = bullet.Entity.Get<CRicochetBullet>();
            if (ricochetBullet.HasHit(entityHit))
            {
                return false;
            }

            CVelocity2D velocity = bullet.Entity.Get<CVelocity2D>();
            if (ricochetBullet.OnHit(entityHit))
            {
                ZombieHelper.TakeDamageOrDelete(entityHit, 20);
                velocity.Direction = velocity.Direction.Rotate(Global.Random.NextFloat(-1f, 1f));
                return false;
            }

            bullet.Entity.Delete();
            return true;
        }
    }  
}
