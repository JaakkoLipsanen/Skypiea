
namespace Flai.Graphics.Particles.Modifiers
{
    public class RotationFromVelocityModifier : ParticleModifier
    {
        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            Particle particle = iterator.First;
            do
            {
                particle.Rotation = FlaiMath.GetAngle(particle.Velocity);
            } while (iterator.MoveNext(ref particle));
        }
    }
}
