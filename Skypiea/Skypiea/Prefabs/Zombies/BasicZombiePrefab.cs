using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Prefabs.Zombies
{
    public class BasicZombiePrefab : Prefab
    {
        private const float Size = 32;
        private const float Speed = Tile.Size * 2;
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CBasicZombieAI>().Initialize(BasicZombiePrefab.Speed);
            entity.AddFromPool<CZombieInfo>().Initialize(ZombieType.Normal, BasicZombiePrefab.Size);
            entity.AddFromPool<CHealth>().Initialize(10);

            entity.Tag = EntityTags.Zombie;
        }
    }
}
