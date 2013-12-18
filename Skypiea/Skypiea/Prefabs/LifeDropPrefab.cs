using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Prefabs
{
    public class LifeDropPrefab : Prefab
    {
        private const float LifeTime = 20;
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CDrop>().Initialize(DropType.Life);
            entity.AddFromPool<CLifeDrop>();
            entity.AddFromPool<CLifeTime>().Initialize(LifeDropPrefab.LifeTime);
            entity.Tag = EntityTags.Drop;
        }
    }
}
