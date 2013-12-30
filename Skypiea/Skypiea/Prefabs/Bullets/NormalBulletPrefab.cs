using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs.Bullets
{
    public class NormalBulletPrefab : Prefab
    {
        public const float DefaultSpeed = SkypieaConstants.PixelsPerMeter*25f;

        // should bullets even have size? they could be just points... or maybe it's wise for them to have size..
        private static readonly SizeF BulletSize = new SizeF(6, 6);
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            CTransform2D transform = parameters.Get<CTransform2D>(0);
            BulletWeapon weapon = parameters.Get<BulletWeapon>(1);
            float angleOffset = parameters.GetOrDefault<float>(2);
            float speed = parameters.GetOrDefault<float>(3, DefaultSpeed);

            entity.Transform.Position = transform.Position;
            entity.Transform.Rotation = transform.Rotation + angleOffset;

            entity.AddFromPool<CBullet>().Initialize(NormalBulletPrefab.BulletSize, weapon);
            entity.AddFromPool<CVelocity2D>().Initialize(FlaiMath.GetAngleVector(entity.Transform.Rotation), speed);
        }
    }
}
