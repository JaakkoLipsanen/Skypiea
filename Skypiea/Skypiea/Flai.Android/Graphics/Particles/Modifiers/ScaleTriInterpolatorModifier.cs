
namespace Flai.Graphics.Particles.Modifiers
{
    public class ScaleTriInterpolatorModifier : ParticleModifier
    {
        private float _initialScale = 1f;
        private float _medianScale = 0.5f;
        private float _finalScale = 0f;

        private float _median = 0.5f;

        public float InitialScale
        {
            get { return _initialScale; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value >= 0);

                _initialScale = value;
            }
        }

        public float MedianScale
        {
            get { return _medianScale; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value >= 0);

                _medianScale = value;
            }
        }

        public float FinalScale
        {
            get { return _finalScale; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value >= 0);

                _finalScale = value;
            }
        }

        public float Median
        {
            get { return _median; }
            set
            {
                Ensure.IsValid(value);
                Ensure.WithinRange(value, 0, 1);

                _median = value;
            }
        }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            float scaleDeltaFirst = _medianScale - _initialScale;
            float scaleDeltaLast = _finalScale - _medianScale;
            Particle particle = iterator.First;

            do
            {
                if (particle.Age < _median)
                {
                    particle.Scale = _initialScale + scaleDeltaFirst * (particle.Age / _median);
                }
                else
                {
                    particle.Scale = _medianScale + scaleDeltaLast * (particle.Age - _median) / (1f - _median);
                }

            } while (iterator.MoveNext(ref particle));
        }
    }
}
