
namespace Flai.Graphics.Particles.Modifiers
{
    public class OpacityFadeModifier : ParticleModifier
    {
        private float _initialOpacity = 1f;
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

        // value > 1. value = 1 -> noise doesnt affect, value => 2 -> noise affects from 1x to 2x, value => 4 -> noise affects from 1x to 4x
        private float _noiseSpeedMultiplier = 1;
        public float NoiseSpeedMultiplier
        {
            get { return _noiseSpeedMultiplier; }
            set
            {
                Ensure.IsValid(value);
                Ensure.True(value >= 1);

                _noiseSpeedMultiplier = value;
            }
        }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            Particle particle = iterator.First;
            if (_noiseSpeedMultiplier == 1)
            {
                do
                {
                    particle.Opacity = _initialOpacity - _initialOpacity * particle.Age;
                } while (iterator.MoveNext(ref particle));
            }
            else
            {
                float multiplier = _initialOpacity * _noiseSpeedMultiplier + _initialOpacity;
                do
                {
                    particle.Opacity = _initialOpacity - _initialOpacity * (1 + (multiplier - 1) * particle.Noise) * particle.Age; // with this opacity can go below zero
                } while (iterator.MoveNext(ref particle));
            }
        }
    }
}
