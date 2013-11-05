using Flai.CBES;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;

namespace Zombie.Prefabs
{
    public class ZombiePrefab : Prefab
    {
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            Vector2 position = (Vector2) parameters[0];
            entity.Add(new TransformComponent(position));
            entity.Add<BasicZombieAIComponent>();
            entity.Tag = EntityTags.Zombie;

            return entity;
        }
    }
}
