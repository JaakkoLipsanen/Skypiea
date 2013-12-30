using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class Shotgun : BulletWeapon
    {
        private const int BulletCount = 55;
        public override WeaponType Type
        {
            get { return WeaponType.Shotgun; }
        }

        public Shotgun(float ammoMultiplier)
            : base((int)(Shotgun.BulletCount * ammoMultiplier), 0.4f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            const int Bullets = 6;
            const float SpreadAngle = 0.3f;
            for (int i = 0; i < Bullets; i++)
            {
                playerEntity.EntityWorld.CreateEntityFromPrefab<NormalBulletPrefab>(playerEntity.Transform, this, -SpreadAngle / 2f + i / (float)Bullets * SpreadAngle);
            }

            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            ZombieHelper.TakeDamage(entityHit, 10, bullet.Entity.Get<CVelocity2D>().Velocity / 3f);
            bullet.Entity.Delete();
            return false; // okay this shouldn't return always false. but rather like "allow 2 first hits to go through, and the stop"
        }
    }
}
