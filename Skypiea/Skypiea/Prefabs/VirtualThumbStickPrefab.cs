using Flai.CBES;
using Flai.CBES.Components;
using Flai.Misc;
using Skypiea.Misc;

namespace Skypiea.Prefabs
{
    public class VirtualThumbstickPrefab : Prefab
    {
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Add(new CVirtualThumbstick(parameters.Get<VirtualThumbstick>(0)));
            entity.Tag = EntityTags.VirtualThumbStick;
        }
    }
}
