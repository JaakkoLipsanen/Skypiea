using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Messages;

namespace Skypiea.Systems.Zombie
{
    public class ZombieHealthSystem : ComponentProcessingSystem<CHealth>
    {
        protected internal override int ProcessOrder
        {
            get { return SystemProcessOrder.PostFrame; }
        }

        public ZombieHealthSystem()
            : base(Aspect.All<CZombieInfo>())
        {
        }

        public override void Process(UpdateContext updateContext, Entity entity, CHealth health)
        {
            if (!health.IsAlive && !entity.IsDeleting)
            {
                this.EntityWorld.BroadcastMessage(this.EntityWorld.FetchMessage<ZombieKilledMessage>().Initialize(entity));
                entity.Delete();
            }
        }
    }
}
