using Flai;
using Flai.CBES;
using Flai.Graphics.Particles;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;
using Skypiea.Systems.Zombie;

namespace Skypiea.Model.Weapons
{
    public class RocketLauncher : BulletWeapon // do this!
    {
        private const int BulletCount = 45;
        private const int ExplosionRange = 60;

        public override WeaponType Type
        {
            get { return WeaponType.RocketLauncher; }
        }

        public RocketLauncher(float ammoMultiplier)
            : base((int)(RocketLauncher.BulletCount * ammoMultiplier), 0.475f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<RocketLauncherBulletPrefab>(playerEntity.Transform, this);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            EntityWorld entityWorld = bullet.Entity.EntityWorld;
            IZombieSpatialMap zombieSpatialMap = entityWorld.Services.Get<IZombieSpatialMap>();

            // if the message has not listeners, then there's no point in doing in broadcasting it.
            RocketExplodedMessage explodedMessage = entityWorld.HasListeners<RocketExplodedMessage>() ? entityWorld.FetchMessage<RocketExplodedMessage>() : null; // AAWFFUUULL!!
            foreach (Entity zombie in zombieSpatialMap.GetAllIntersecting(bullet.Entity.Transform, RocketLauncher.ExplosionRange))
            {
                if (zombie.Get<CHealth>().IsAlive && ZombieHelper.TakeDamage(zombie, 25, Vector2.Zero))
                {
                    // if the message has not listeners, then there's no point in doing in broadcasting it.
                    if (explodedMessage != null)
                    {
                        explodedMessage.Add(zombie);
                    }
                }
            }

            // if the message has not listeners, then there's no point in doing in broadcasting it.
            if (explodedMessage != null)
            {
                entityWorld.BroadcastMessage(explodedMessage);
            }

            IParticleEngine particleEngine = entityWorld.Services.Get<IParticleEngine>();
            particleEngine[ParticleEffectID.RocketExplosion].Trigger(bullet.Entity.Transform);

            bullet.Entity.Delete();
            return true;
        }
    }
}
