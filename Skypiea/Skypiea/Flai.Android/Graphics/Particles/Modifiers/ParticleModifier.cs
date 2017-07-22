
namespace Flai.Graphics.Particles.Modifiers
{
    public abstract class ParticleModifier
    {
        public abstract void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator);
    }
}
