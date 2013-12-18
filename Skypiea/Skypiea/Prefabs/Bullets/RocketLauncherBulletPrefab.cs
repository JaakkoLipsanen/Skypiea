using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics.Particles.Controllers;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs.Bullets
{
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

            entity.Transform.Position = transform.Position;
            entity.Transform.Rotation = transform.Rotation;

            entity.AddFromPool<CBullet>().Initialize(RocketLauncherBulletPrefab.BulletSize, parameters.Get<RocketLauncher>(1));
            entity.AddFromPool<CVelocity2D>().Initialize(FlaiMath.GetAngleVector(transform.Rotation), Speed);
            entity.AddFromPool<CParticleEmitter2D>().Initialize(ParticleEffectID.RocketSmoke, new BurstTriggerSettable(0.01f, entity.Transform));
        }
    }
}
