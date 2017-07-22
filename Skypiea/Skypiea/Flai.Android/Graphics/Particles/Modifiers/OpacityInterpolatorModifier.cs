
namespace Flai.Graphics.Particles.Modifiers
{
    public class OpacityInterpolatorModifier : ParticleModifier
    {
        private float _initialOpacity = 1f;
        private float _finalOpacity = 0f;

        public float InitialOpacity
        {
            get { return _initialOpacity; }
            set
            {
                Ensure.IsValid(value);
                Ensure.WithinRange(value, 0, 1);

                _initialOpacity = value;
            }
        }

        public float FinalOpacity
        {
            get { return _finalOpacity; }
            set
            {
                Ensure.IsValid(value);
                Ensure.WithinRange(value, 0, 1);

                _finalOpacity = value;
            }
        }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            float opacityDelta = _finalOpacity - _initialOpacity;
            Particle particle = iterator.First;

            do
            {
                particle.Opacity = _initialOpacity + opacityDelta * particle.Age;
            } while (iterator.MoveNext(ref particle));
        }
    }
}
