using System.Linq;
using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics.Particles;
using Flai.Graphics.Particles.Controllers;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

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
            foreach (Entity zombie in bullet.Entity.EntityWorld.FindEntitiesWithTag(EntityTags.Zombie).Where(entity => Vector2.Distance(entity.Transform.Position, bullet.Entity.Transform.Position) < RocketLauncher.ExplosionRange))
            {
                CHealth health = zombie.TryGet<CHealth>();
                if (health)
                {
                    health.TakeDamage(10);
                }
                else
                {
                    zombie.Delete();
                }
            }

            IParticleEngine particleEngine = bullet.Entity.EntityWorld.Services.Get<IParticleEngine>();
            particleEngine[ParticleEffectID.RocketExplosion].Trigger(bullet.Entity.Transform);

            bullet.Entity.Delete();
            return true;
        }
    }

    // todo: should Weapon bullets be at Prefab/ folder or in the same class as the weapon?
    // BulletPrefab..?
    public class RocketLauncherBulletPrefab : Prefab
    {
        // should bullets even have size? they could be just points... or maybe it's wise for them to have size..
        private static readonly SizeF BulletSize = new SizeF(6, 6);

        // todo: instead of doing something like this, how about make a callback "OnCollided(Entity entity (or Vector2 position tms))" and then the weapon itself (whos callback it will be calling) decides the damage etc. this.. could be A LOT better!
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            const float Speed = Tile.Size * 25;
            CTransform2D transform = parameters.Get<CTransform2D>(0);
            RocketLauncher rocketLauncher = parameters.Get<RocketLauncher>(1);

            entity.Transform.Position = transform.Position;
            entity.Transform.Rotation = transform.Rotation;

            entity.AddFromPool<CBullet>().Initialize(RocketLauncherBulletPrefab.BulletSize, rocketLauncher);
            entity.AddFromPool<CVelocity>().Initialize(FlaiMath.GetAngleVector(transform.Rotation), Speed);
            entity.AddFromPool<CParticleEmitter>().Initialize(ParticleEffectID.RocketSmoke, new BurstTriggerParticleController(0.01f, entity.Transform));
        }
    }
}
