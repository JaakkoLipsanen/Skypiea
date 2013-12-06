using Flai.CBES;
using Skypiea.Components;

namespace Skypiea.Misc
{
    public static class ZombieHelper
    {
        public static void TakeDamageOrDelete(Entity zombie, float damage)
        {
            CHealth health = zombie.TryGet<CHealth>();
            if (health)
            {
                health.TakeDamage(damage);
            }
            else
            {
                zombie.Delete();
            }
        }

        public static void Kill(Entity zombie)
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
        }
    }
}
