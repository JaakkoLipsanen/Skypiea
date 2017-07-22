using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.Controllers
{
    // TODO: MovementTriggerController, ContinousTriggerController etc
    public class BurstTriggerController : ParticleEffectController, ITransformSettable
    {
        private ITransform2D _transform;
        private readonly float _triggerPeriod;
        private float _timeUntilTrigger = 0f;

        public BurstTriggerController(float triggerPeriod)
            : this(triggerPeriod, null)
        {        
        }

        public BurstTriggerController(float triggerPeriod, Vector2 position)
            : this(triggerPeriod, position, 0)
        {
        }

        public BurstTriggerController(float triggerPeriod, Vector2 position, float rotation)
            : this(triggerPeriod, Transform.CreateTransform2D(position, rotation))
        {
        }

        public BurstTriggerController(float triggerPeriod, ITransform2D transform)
        {
            Ensure.True(triggerPeriod > 0);
         // Ensure.NotNull(transform); // allow null transform... so that SetTransform can be called to set it

            _triggerPeriod = triggerPeriod;
            _transform = transform;
        }

        protected internal override void Update(UpdateContext updateContext)
        {
            Ensure.NotNull(_transform); // transform can't be null anymore when updating

            _timeUntilTrigger += updateContext.DeltaSeconds;
            if (_timeUntilTrigger >= _triggerPeriod)
            {
                _timeUntilTrigger -= _triggerPeriod;
                this.ParticleEffect.Trigger(_transform);
            }
        }

        public void SetTransform(ITransform2D transform)
        {
            Ensure.NotNull(transform);
            _transform = transform;
        }
    }  
}
