using Flai;
using Flai.CBES;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;

namespace Zombie.Prefabs
{
    public class BasicZombiePrefab : Prefab
    {
        private static readonly SizeF Size = new SizeF(32, 32);
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            Vector2 position = (Vector2) parameters[0];
            entity.Add(new TransformComponent(position));
            entity.Add<BasicZombieAIComponent>();
            entity.Add(new AreaComponent(BasicZombiePrefab.Size));
            entity.Add(new HealthComponent(15));
            entity.Tag = EntityTags.Zombie;

            return entity;
        }
    }
}
