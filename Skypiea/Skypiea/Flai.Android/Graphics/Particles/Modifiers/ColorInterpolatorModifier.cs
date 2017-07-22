using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.Modifiers
{
    public class ColorInterpolatorModifier : ParticleModifier
    {
        private Vector3 _initialColor;
        private Vector3 _finalColor;

        public Color InitialColor
        {
            get { return new Color(_initialColor); }
            set { _initialColor = value.ToVector3(); }
        }

        public Color FinalColor
        {
            get { return new Color(_finalColor); }
            set { _finalColor = value.ToVector3(); }
        }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            Vector3 deltaColor = _finalColor - _initialColor;
            Particle particle = iterator.First;
            do
            {
                particle.Color.X = _initialColor.X + deltaColor.X * particle.Age;
                particle.Color.Y = _initialColor.Y + deltaColor.Y * particle.Age;
                particle.Color.Z = _initialColor.Z + deltaColor.Z * particle.Age;

            } while (iterator.MoveNext(ref particle));
        }
    }
}
