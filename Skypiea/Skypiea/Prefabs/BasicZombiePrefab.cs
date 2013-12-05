using Flai;
using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Prefabs
{
    public class BasicZombiePrefab : Prefab
    {
        private static readonly SizeF Size = new SizeF(38, 38);
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CBasicZombieAI>();
            entity.AddFromPool<CArea>().Initialize(BasicZombiePrefab.Size);
            entity.AddFromPool<CHealth>().Initialize(5);

            entity.Tag = EntityTags.Zombie;
        }
    }
}
