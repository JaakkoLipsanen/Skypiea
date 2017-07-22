
namespace Flai.Graphics.Particles.Modifiers
{
    public class VelocityClampModifier : ParticleModifier
    {
        private float _maximumSpeed = float.MaxValue;
        public float MaximumSpeed
        {
            get { return _maximumSpeed; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value >= 0);

                _maximumSpeed = value;
            }
        }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            Particle particle = iterator.First;
            float maxSpeedSquared = _maximumSpeed * _maximumSpeed;

            do
            {
                float speedSquared = particle.Velocity.X * particle.Velocity.X + particle.Velocity.Y * particle.Velocity.Y;
                if (speedSquared > maxSpeedSquared)
                {
                    particle.Velocity.Normalize();
                    particle.Velocity *= _maximumSpeed;
                }

            } while (iterator.MoveNext(ref particle));
        }
    }
}
