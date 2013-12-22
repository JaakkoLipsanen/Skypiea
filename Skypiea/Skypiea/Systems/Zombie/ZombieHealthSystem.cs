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
            : base(Aspect.All<CZombieInfo>())
        {
        }

        public override void Process(UpdateContext updateContext, Entity entity, CHealth velocity)
        {
            if (!velocity.IsAlive)
            {
                this.EntityWorld.BroadcastMessage(this.EntityWorld.FetchMessage<ZombieKilledMessage>().Initialize(entity));
                entity.Delete();
            }
        }
    }
}
