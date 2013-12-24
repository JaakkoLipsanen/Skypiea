using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs.Bullets
{
    public class RicochetBulletPrefab : Prefab
    {
        private static readonly SizeF BulletSize = new SizeF(6, 6);
        private const float RotationAmount = FlaiMath.TwoPi * 3;

        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            const float Speed = SkypieaConstants.PixelsPerMeter * 20;
            CTransform2D transform = parameters.Get<CTransform2D>(0);
            float rotationOffset = parameters.GetOrDefault<float>(2);

            entity.Transform.Position = transform.Position;
            entity.Transform.Rotation = transform.Rotation;

            entity.AddFromPool<CBullet>().Initialize(RicochetBulletPrefab.BulletSize, parameters.Get<RicochetGun>(1));
            entity.AddFromPool<CVelocity2D>().Initialize(FlaiMath.GetAngleVector(transform.Rotation + rotationOffset), Speed);
            entity.AddFromPool<CRotater2D>().Initialize(RicochetBulletPrefab.RotationAmount);
            entity.AddFromPool<CRicochetBullet>().Initialize(6);
        }
    }
}
