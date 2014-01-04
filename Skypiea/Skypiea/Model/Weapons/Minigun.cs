using Flai;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class Minigun : BulletWeapon
    {
        private const int BulletCount = 160;
        private const float BulletSpeed = SkypieaConstants.PixelsPerMeter * 40f;
        public override WeaponType Type
        {
            get { return WeaponType.Minigun; }
        }

        public Minigun(float ammoMultiplier)
            : base((int)(Minigun.BulletCount * ammoMultiplier), 0.1f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<NormalBulletPrefab>(playerEntity.Transform, this, 0f, Minigun.BulletSpeed);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            ZombieHelper.TakeDamage(entityHit, 10, bullet.Transform.RotationVector * Minigun.BulletSpeed / 3.5f);
            bullet.Entity.Delete();
            return true;
        }
    }
}
