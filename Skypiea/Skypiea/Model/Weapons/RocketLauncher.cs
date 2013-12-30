using Flai;
using Flai.CBES;
using Flai.Graphics.Particles;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;
using Skypiea.Systems.Zombie;

namespace Skypiea.Model.Weapons
{
    public class RocketLauncher : BulletWeapon // do this!
    {
        private const int BulletCount = 55;
        private const int ExplosionRange = 60;

        public override WeaponType Type
        {
            get { return WeaponType.RocketLauncher; }
        }

        public RocketLauncher(float ammoMultiplier)
            : base((int)(RocketLauncher.BulletCount * ammoMultiplier), 0.3f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<RocketLauncherBulletPrefab>(playerEntity.Transform, this);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            IZombieSpatialMap zombieSpatialMap = entityHit.EntityWorld.Services.Get<IZombieSpatialMap>();
            foreach (Entity zombie in zombieSpatialMap.GetAllIntersecting(bullet.Entity.Transform, RocketLauncher.ExplosionRange))
            {
                ZombieHelper.TakeDamage(zombie, 25, Vector2.Zero); // Vector2.Normalize(zombie.Transform.Position - bullet.Transform.Position) * SkypieaConstants.PixelsPerMeter * 6f);
            }

            IParticleEngine particleEngine = bullet.Entity.EntityWorld.Services.Get<IParticleEngine>();
            particleEngine[ParticleEffectID.RocketExplosion].Trigger(bullet.Entity.Transform);

            bullet.Entity.Delete();
            return true;
        }
    }
}
