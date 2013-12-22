using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Model;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs.Bullets
{
    public class FlamethrowerBulletPrefab : Prefab
    {
        private static readonly SizeF BulletSize = new SizeF(12, 12);
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            const float Speed = Tile.Size * 14.25f;
            CTransform2D transform = parameters.Get<CTransform2D>(0);
            BulletWeapon weapon = parameters.Get<BulletWeapon>(1);
            float angleOffset = Global.Random.NextFloat(-0.3f, 0.3f);

            entity.Transform.Position = transform.Position;
            entity.Transform.Rotation = transform.Rotation + angleOffset;

            entity.AddFromPool<CBullet>().Initialize(FlamethrowerBulletPrefab.BulletSize, weapon);
            entity.AddFromPool<CVelocity2D>().Initialize(FlaiMath.GetAngleVector(entity.Transform.Rotation), Speed);
            entity.AddFromPool<CLifeTime>().Initialize(0.5f);
        }
    }
}
