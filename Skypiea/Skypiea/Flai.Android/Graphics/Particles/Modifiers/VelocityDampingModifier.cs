
namespace Flai.Graphics.Particles.Modifiers
{
    public class VelocityDampingModifier : ParticleModifier
    {
        // coefficent something something
        public float DampingPower { get; set; }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            if (this.DampingPower <= float.Epsilon)
            {
                return;
            }

            float inverseDampingDelta = -this.DampingPower * updateContext.DeltaSeconds; 
            Particle particle = iterator.First;
            do
            {
                particle.Velocity.X += particle.Velocity.X * inverseDampingDelta; // inverse velocity 
                particle.Velocity.Y += particle.Velocity.Y * inverseDampingDelta; // inverse velocity 
            } while (iterator.MoveNext(ref particle));
        }
    }
}
