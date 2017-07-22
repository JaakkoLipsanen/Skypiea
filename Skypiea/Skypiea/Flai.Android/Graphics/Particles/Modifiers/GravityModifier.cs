using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.Modifiers
{
    public class GravityModifier : ParticleModifier
    {
        private Vector2 _direction;
        // unit vector
        public Vector2 Direction
        {
            get { return _direction; }
            set
            {
                Ensure.True(value != Vector2.Zero);
                Ensure.IsValid(value.X);
                Ensure.IsValid(value.Y);

                _direction = value;
                _direction.Normalize();
            }
        }

        public float Strength { get; set; }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            float deltaStrength = this.Strength * updateContext.DeltaSeconds;
            if (deltaStrength == 0f || _direction == Vector2.Zero)
            {
                return;
            }

            Vector2 deltaGravity = _direction * deltaStrength;
            Particle particle = iterator.First;
            do
            {
                particle.Velocity += deltaGravity;
            } while (iterator.MoveNext(ref particle));
        }
    }
}
