using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs
{
    public class WeaponDropPrefab : Prefab
    {
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CWeaponDrop>().Initialize(parameters.Get<WeaponType>(1));
            entity.Tag = EntityTags.WeaponDrop;
        }
    }
}
