using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class AssaultRifle : BulletWeapon
    {
        private const float MinimumTimeBetweenShots = 0.15f;
        public override WeaponType Type
        {
            get { return WeaponType.AssaultRifle; }
        }

        public AssaultRifle()
            : base(null, AssaultRifle.MinimumTimeBetweenShots)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<NormalBulletPrefab>(playerEntity.Transform, this);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(CBullet bullet, Entity entityHit)
        {
            if (entityHit != null)
            {
                CHealth cHealth = entityHit.TryGet<CHealth>();
                if (cHealth)
                {
                    cHealth.TakeDamage(10);
                }
                else
                {
                    entityHit.Delete();
                }
            }

            bullet.Entity.Delete();
            return true;
        }
    }
}
