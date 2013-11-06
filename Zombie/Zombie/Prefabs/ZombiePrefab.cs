using Flai;
using Flai.CBES;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;

namespace Zombie.Prefabs
{
    public class ZombiePrefab : Prefab
    {
        private static readonly SizeF Size = new SizeF(32, 32);
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            Vector2 position = (Vector2) parameters[0];
            entity.Add(new TransformComponent(position));
            entity.Add<BasicZombieAIComponent>();
            entity.Add(new AreaComponent(ZombiePrefab.Size));
            entity.Add(new HealthComponent(10));
            entity.Tag = EntityTags.Zombie;

            return entity;
        }
    }
}
