using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;

namespace Skypiea.Systems.Zombie
{
    public class ZombieHealthSystem : ComponentProcessingSystem<CHealth>
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PostFrame; }
        }

        public ZombieHealthSystem()
            : base(Aspect.WithTag(EntityTags.Zombie))
        {
        }

        public override void Process(UpdateContext updateContext, Entity entity, CHealth health)
        {
            if (!health.IsAlive)
            {
                this.EntityWorld.BroadcastMessage(new ZombieKilledMessage(entity));
                entity.Delete();
            }
        }
    }
}
