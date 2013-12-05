using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Model;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs.Bullets
{
    public class NormalBulletPrefab : Prefab
    {
        // should bullets even have size? they could be just points... or maybe it's wise for them to have size..
        private static readonly SizeF BulletSize = new SizeF(6, 6);
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            const float Speed = Tile.Size * 25f;

            CTransform2D transform = parameters.Get<CTransform2D>(0);
            BulletWeapon weapon = parameters.Get<BulletWeapon>(1);
            float angleOffset = parameters.GetOrDefault<float>(2);

            entity.Transform.Position = transform.Position;
            entity.Transform.Rotation = transform.Rotation + angleOffset;

            entity.AddFromPool<CBullet>().Initialize(NormalBulletPrefab.BulletSize, weapon);
            entity.AddFromPool<CVelocity>().Initialize(FlaiMath.GetAngleVector(entity.Transform.Rotation), Speed);
        }
    }
}
