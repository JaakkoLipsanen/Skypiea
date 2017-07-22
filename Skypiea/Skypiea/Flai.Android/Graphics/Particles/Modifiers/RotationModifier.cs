
namespace Flai.Graphics.Particles.Modifiers
{
    public class RotationModifier : ParticleModifier
    {
        public float RotationRate { get; set; }
        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            if (this.RotationRate <= float.Epsilon)
            {
                return;
            }

            float deltaRotation = this.RotationRate * updateContext.DeltaSeconds;
            Particle particle = iterator.First;
            do
            {
                particle.Rotation += deltaRotation;
            } while (iterator.MoveNext(ref particle));
        }
    }
}
