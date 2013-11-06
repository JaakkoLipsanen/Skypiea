using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Zombie.Components;
using Zombie.Messages;
using Zombie.Misc;

namespace Zombie.Systems
{
    public class ZombieHealthSystem : ComponentProcessingSystem<HealthComponent>
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PostFrame; }
        }

        public ZombieHealthSystem()
            : base(Aspect.WithTag(EntityTags.Zombie))
        {
        }

        public override void Process(UpdateContext updateContext, Entity entity, HealthComponent health)
        {
            if (!health.IsAlive)
            {
                this.EntityWorld.BroadcastMessage(new ZombieKilledMessage(entity));
                entity.Delete();
            }
        }
    }
}
