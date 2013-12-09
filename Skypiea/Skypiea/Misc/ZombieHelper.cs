using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics.Particles;
using Skypiea.Components;
using Skypiea.Model.Boosters;

namespace Skypiea.Misc
{
    public static class ZombieHelper
    {
        public static bool TakeDamageOrDelete(Entity zombie, float damage)
        {
            CHealth health = zombie.TryGet<CHealth>();
            if (health)
            {
                return health.TakeDamage(damage * BoosterHelper.GetZombieDamageRedcutionMultiplier(zombie.EntityWorld.Services.Get<IBoosterState>()));
            }

            zombie.Delete();
            return true;
        }

        public static bool Kill(Entity zombie)
        {
            CHealth health = zombie.TryGet<CHealth>();
            if (health)
            {
                const float LotsOfDamage = 1000 * 1000;
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
