using Flai;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class AssaultRifle : BulletWeapon
    {
        private const float MinimumTimeBetweenShots = 0.2f;
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
                ZombieHelper.TakeDamageOrDelete(entityHit, 10);
            }

            bullet.Entity.Delete();
            return true;
        }
    }
}
