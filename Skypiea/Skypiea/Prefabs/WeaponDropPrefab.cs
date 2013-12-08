using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs
{
    public class WeaponDropPrefab : Prefab
    {
        private const float LifeTime = 10;
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CWeaponDrop>().Initialize(parameters.Get<WeaponType>(1));
            entity.AddFromPool<CLifeTime>().Initialize(WeaponDropPrefab.LifeTime);
            entity.Tag = EntityTags.WeaponDrop;
        }
    }
}
