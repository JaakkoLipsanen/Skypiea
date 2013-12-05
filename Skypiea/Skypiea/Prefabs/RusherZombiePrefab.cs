using Flai;
using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Prefabs
{
    public class RusherZombiePrefab : Prefab
    {
        private static readonly SizeF Size = new SizeF(42, 42);
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CRusherZombieAI>();
            entity.AddFromPool<CArea>().Initialize(RusherZombiePrefab.Size);
            entity.AddFromPool<CHealth>().Initialize(5);

            entity.Tag = EntityTags.Zombie;
        }
    }
}
