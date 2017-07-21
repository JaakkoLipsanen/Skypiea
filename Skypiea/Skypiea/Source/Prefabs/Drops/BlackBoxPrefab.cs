using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Prefabs.Drops
{
    public class BlackBoxPrefab : Prefab
    {
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CDrop>().Initialize(DropType.BlackBox);
            entity.AddFromPool<CLifeTime>().Initialize(100f);
            entity.Tag = EntityTags.Drop;
        }
    }
}
