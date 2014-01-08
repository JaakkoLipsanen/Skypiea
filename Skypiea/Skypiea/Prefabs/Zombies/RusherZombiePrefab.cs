using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Prefabs.Zombies
{
    public class RusherZombiePrefab : Prefab
    {
        private const float Size = 38;
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CRusherZombieAI>();
            entity.AddFromPool<CZombieInfo>().Initialize(ZombieType.Rusher, RusherZombiePrefab.Size);
            entity.AddFromPool<CHealth>().Initialize(10);
            entity.Tag = EntityTags.Zombie;
        }
    }
}
