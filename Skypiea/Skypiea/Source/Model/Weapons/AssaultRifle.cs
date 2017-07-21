using Flai;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class AssaultRifle : BulletWeapon
    {
        private const float MinimumTimeBetweenShots = 0.1875f;
        public override WeaponType Type
        {
            get { return WeaponType.AssaultRifle; }
        }

        public AssaultRifle()
            : base(null, AssaultRifle.MinimumTimeBetweenShots)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, EntityWorld entityWorld, Entity playerEntity)
        {
            entityWorld.CreateEntityFromPrefab<NormalBulletPrefab>(playerEntity.Transform, this);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            ZombieHelper.TakeDamage(entityHit, 10, bullet.Transform.RotationVector * NormalBulletPrefab.DefaultSpeed / 3.5f);
            bullet.Entity.Delete();
            return true;
        }
    }
}
