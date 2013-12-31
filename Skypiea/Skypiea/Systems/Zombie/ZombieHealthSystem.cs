using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Messages;

namespace Skypiea.Systems.Zombie
{
    public class ZombieHealthSystem : ComponentProcessingSystem<CHealth>
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PostFrame; }
        }

        public ZombieHealthSystem()
            : base(Aspect.All<CZombieInfo>())
        {
        }

        public override void Process(UpdateContext updateContext, Entity entity, CHealth health)
        {
            if (!health.IsAlive)
            {
                this.EntityWorld.BroadcastMessage(this.EntityWorld.FetchMessage<ZombieKilledMessage>().Initialize(entity));
                entity.Delete();
            }
        }
    }
}
