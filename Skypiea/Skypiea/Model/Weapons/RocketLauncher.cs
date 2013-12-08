using Flai;
using Flai.CBES;
using Flai.Graphics.Particles;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;
using Skypiea.Services;

namespace Skypiea.Model.Weapons
{
    public class RocketLauncher : BulletWeapon // do this!
    {
        private const int BulletCount = 50;
        private const int ExplosionRange = 60;

        public override WeaponType Type
        {
            get { return WeaponType.RocketLauncher; }
        }

        public RocketLauncher()
            : base(RocketLauncher.BulletCount, 0.3f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<RocketLauncherBulletPrefab>(playerEntity.Transform, this);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(CBullet bullet, Entity entityHit)
        {
            IZombieSpatialMap zombieSpatialMap = entityHit.EntityWorld.Services.Get<IZombieSpatialMap>();
            foreach (Entity zombie in zombieSpatialMap.GetZombiesWithinRange(bullet.Entity.Transform, RocketLauncher.ExplosionRange))
            {
                if (!ZombieHelper.TakeDamageOrDelete(zombie, 20))
                {
                    ZombieHelper.TriggerBloodSplatter(bullet.Entity.Transform);
                }
            }

            IParticleEngine particleEngine = bullet.Entity.EntityWorld.Services.Get<IParticleEngine>();
            particleEngine[ParticleEffectID.RocketExplosion].Trigger(bullet.Entity.Transform);

            bullet.Entity.Delete();
            return true;
        }
    }
}
