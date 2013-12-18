using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Model;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs.Bullets
{
    public class RicochetBulletPrefab : Prefab
    {
        private static readonly SizeF BulletSize = new SizeF(6, 6);
        private const float RotationAmount = FlaiMath.TwoPi * 2;

        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            const float Speed = Tile.Size * 20;
            CTransform2D transform = parameters.Get<CTransform2D>(0);

            entity.Transform.Position = transform.Position;
            entity.Transform.Rotation = transform.Rotation;

            entity.AddFromPool<CBullet>().Initialize(RicochetBulletPrefab.BulletSize, parameters.Get<RicochetGun>(1));
            entity.AddFromPool<CVelocity2D>().Initialize(FlaiMath.GetAngleVector(transform.Rotation), Speed);
            entity.AddFromPool<CRotater2D>().Initialize(RicochetBulletPrefab.RotationAmount);
            entity.AddFromPool<CRicochetBullet>().Initialize(6);
        }
    }
}
