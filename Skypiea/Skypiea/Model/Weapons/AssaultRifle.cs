using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
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

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<NormalBulletPrefab>(playerEntity.Transform, this);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            if (entityHit != null)
            {
                if (!ZombieHelper.TakeDamageOrDelete(entityHit, 10, bullet.Entity.Get<CVelocity2D>().Velocity / 4f))
                {
                    ZombieHelper.TriggerBloodSplatter(bullet.Entity.Transform, Vector2.Zero);
                }
            }

            bullet.Entity.Delete();
            return true;
        }
    }
}
