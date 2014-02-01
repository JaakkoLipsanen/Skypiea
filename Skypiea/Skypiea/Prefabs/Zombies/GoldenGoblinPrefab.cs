using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics.Particles.Controllers;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Prefabs.Zombies
{
    public class GoldenGoblinPrefab : Prefab
    {
        private const float Size = SkypieaConstants.PixelsPerMeter * 0.825f;
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.AddFromPool<CGoldenGoblinAI>().Initialize(entity.Transform.Position);
            entity.AddFromPool<CZombieInfo>().Initialize(ZombieType.GoldenGoblin, GoldenGoblinPrefab.Size);
            entity.AddFromPool<CHealth>().Initialize(145);
            entity.AddFromPool<CParticleEmitter2D>().Initialize(ParticleEffectID.GoldenGoblinPath, new BurstTriggerController(0.025f, entity.Transform));

            // okay.. awful... entities can have only one component of a single type so I must create a child entity...
            // another option would be "CMultiParticleEmitter2D" tms
            Entity childEntity = entityWorld.CreateEntity();
            childEntity.AddFromPool<CParticleEmitter2D>().Initialize(ParticleEffectID.GoldenGoblinSpray, new BurstTriggerController(0.1f, entity.Transform));
            entity.AttachChild(childEntity);

            entity.Tag = EntityTags.Zombie;
        }
    }
}
