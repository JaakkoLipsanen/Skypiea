using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Prefabs.Zombies
{
    public class FatZombiePrefab : Prefab
    {
        private const float Size = 46;
        private const float Speed = Tile.Size * 1.75f;
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CBasicZombieAI>().Initialize(FatZombiePrefab.Speed);
            entity.AddFromPool<CZombieInfo>().Initialize(ZombieType.Fat, FatZombiePrefab.Size);
            entity.AddFromPool<CHealth>().Initialize(75);

            entity.Tag = EntityTags.Zombie;
        }
    }
}
