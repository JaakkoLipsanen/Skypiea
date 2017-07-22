
namespace Flai.Graphics.Particles.Modifiers
{
    public class ScaleInterpolatorModifier : ParticleModifier
    {
        private float _initialScale = 1f;
        private float _finalScale = 0f;

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

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            float scaleDelta = _finalScale - _initialScale;
            Particle particle = iterator.First;

            do
            {
                particle.Scale = _initialScale + scaleDelta * particle.Age;
            } while (iterator.MoveNext(ref particle));
        }
    }
}
