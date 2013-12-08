using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics.Particles;
using Skypiea.Components;

namespace Skypiea.Misc
{
    public static class ZombieHelper
    {
        public static bool TakeDamageOrDelete(Entity zombie, float damage)
        {
            CHealth health = zombie.TryGet<CHealth>();
            if (health)
            {
                return health.TakeDamage(damage);
            }

            zombie.Delete();
            return true;
        }

        public static bool Kill(Entity zombie)
        {
            const float LotsOfDamage = 1000 * 1000;
            CHealth health = zombie.TryGet<CHealth>();
            if (health)
            {
                health.TakeDamage(LotsOfDamage);
            }
            else
            {
                zombie.Delete();
            }

            return true;
        }

        public static void TriggerBloodSplatter(CTransform2D transform)
        {
            transform.Entity.EntityWorld.Services.Get<IParticleEngine>()[ParticleEffectID.ZombieBloodSplatter].Trigger(transform);
        }
    }
}
