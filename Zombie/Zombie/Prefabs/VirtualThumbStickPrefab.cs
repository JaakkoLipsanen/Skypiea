using Flai.CBES;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;

namespace Zombie.Prefabs
{
    public class VirtualThumbStickPrefab : Prefab
    {
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            Vector2 position = (Vector2) parameters[0];
            entity.Add(new VirtualThumbStickComponent(position));
            entity.Tag = EntityTags.VirtualThumbStick;

            return entity;
        }
    }
}
