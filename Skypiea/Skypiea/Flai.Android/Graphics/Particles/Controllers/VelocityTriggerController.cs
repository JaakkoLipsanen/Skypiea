using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.Controllers
{
    // meh name. basically, triggers when Transform has moved a given distance.
    public class VelocityTriggerSettable : ParticleEffectController, ITransformSettable
    {
        private readonly float _triggerDistance;
        private ITransform2D _transform;

        private float _movement = 0f;
        private Vector2 _previousPosition;

        public VelocityTriggerSettable(float triggerDistance)
            : this(triggerDistance, null)
        {
        }

        public VelocityTriggerSettable(float triggerDistance, ITransform2D transform)
        {
            Ensure.IsValid(triggerDistance);
            Ensure.True(triggerDistance > 0);

            _triggerDistance = triggerDistance;
            _transform = transform;
            _previousPosition = _transform.Position;
        }

        protected internal override void Update(UpdateContext updateContext)
        {
            _movement += Vector2.Distance(_previousPosition, _transform.Position);
            if (_movement >= _triggerDistance)
            {
                _movement -= _triggerDistance;
                this.ParticleEffect.Trigger(_transform);
            }

            _previousPosition = _transform.Position;
        }

        public void SetTransform(ITransform2D transform)
        {
            _transform = transform;
            _previousPosition = _transform.Position;
        }
    }
}
