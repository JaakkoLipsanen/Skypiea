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
            : base(40, 0.4f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<RicochetBulletPrefab>(playerEntity.Transform, this);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            CRicochetBullet ricochetBullet = bullet.Entity.Get<CRicochetBullet>();
            CVelocity2D velocity2D = bullet.Entity.Get<CVelocity2D>();
            ricochetBullet.OnEnemyHit();

            if (ricochetBullet.HitsRemaining > 0)
            {
                ZombieHelper.TakeDamageOrDelete(entityHit, 20);
                velocity2D.Direction = velocity2D.Direction.Rotate(Global.Random.NextFloat(-1f, 1f));
                return false;
            }

            bullet.Entity.Delete();
            return true;
        }
    }  
}
