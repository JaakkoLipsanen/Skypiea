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
        private const int BulletCount = 25;
        private const int ExplosionRange = 75;

        public override WeaponType Type
        {
            get { return WeaponType.RocketLauncher; }
        }

        public RocketLauncher()
            : base(RocketLauncher.BulletCount, 0.5f)
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
                ZombieHelper.TakeDamageOrDelete(zombie, 10);
            }

            IParticleEngine particleEngine = bullet.Entity.EntityWorld.Services.Get<IParticleEngine>();
            particleEngine[ParticleEffectID.RocketExplosion].Trigger(bullet.Entity.Transform);

            bullet.Entity.Delete();
            return true;
        }
    }
}
