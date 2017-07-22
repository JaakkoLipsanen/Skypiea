
namespace Flai.Graphics.Particles.TriggerHandlers
{
    public delegate bool TriggerCullerPredicate(ref TriggerContext triggerContext);
    public class CullerTriggerHandler : TriggerHandler
    {
        private readonly TriggerCullerPredicate _shouldTriggerPredicate;
        public CullerTriggerHandler(TriggerCullerPredicate shouldTriggerPredicate)
        {
            Ensure.NotNull(shouldTriggerPredicate);
            _shouldTriggerPredicate = shouldTriggerPredicate;
        }

        protected internal override void Process(ref TriggerContext triggerContext)
        {
            if (!_shouldTriggerPredicate(ref triggerContext))
            {
                triggerContext.Canceled = true;
            }
        }
    }
}
