using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.Modifiers
{
    public class ColorTriInterpolatorModifier : ParticleModifier
    {
        private Vector3 _initialColor;
        private Vector3 _medianColor;
        private Vector3 _finalColor;

        private float _median = 0.5f;

        public Color InitialColor
        {
            get { return new Color(_initialColor); }
            set { _initialColor = value.ToVector3(); }
        }

        public Color MedianColor
        {
            get { return new Color(_medianColor); }
            set { _medianColor = value.ToVector3(); }
        }

        public Color FinalColor
        {
            get { return new Color(_finalColor); }
            set { _finalColor = value.ToVector3(); }
        }

        public float Median
        {
            get { return _median; }
            set
            {
                Ensure.IsValid(_median);
                Ensure.WithinRange(_median, 0, 1);

                _median = value;
            }
        }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            Vector3 deltaColorFirst = _medianColor - _initialColor;
            Vector3 deltaColorLast = _finalColor - _medianColor;
            Particle particle = iterator.First;

            do
            {
                if (particle.Age < _median)
                {
                    float ageMultiplier = particle.Age / _median;
                    particle.Color.X = _initialColor.X + deltaColorFirst.X * ageMultiplier;
                    particle.Color.Y = _initialColor.Y + deltaColorFirst.Y * ageMultiplier;
                    particle.Color.Z = _initialColor.Z + deltaColorFirst.Z * ageMultiplier;
                }
                else
                {
                    float ageMultiplier = (particle.Age - _median) / (1f - _median);
                    particle.Color.X = _medianColor.X + deltaColorLast.X * particle.Age;
                    particle.Color.Y = _medianColor.Y + deltaColorLast.Y * particle.Age;
                    particle.Color.Z = _medianColor.Z + deltaColorLast.Z * particle.Age;
                }

            } while (iterator.MoveNext(ref particle));
        }
    }
}
