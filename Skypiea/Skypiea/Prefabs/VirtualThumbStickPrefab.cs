using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Prefabs
{
    public class VirtualThumbStickPrefab : Prefab
    {
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Add(new CVirtualThumbstick(parameters.Get<Vector2>(0)));
            entity.Tag = EntityTags.VirtualThumbStick;
        }
    }
}
