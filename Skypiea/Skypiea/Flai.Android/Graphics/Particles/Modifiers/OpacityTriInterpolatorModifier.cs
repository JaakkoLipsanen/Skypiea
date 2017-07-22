using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flai.Graphics.Particles.Modifiers
{
    public class OpacityTriInterpolatorModifier : ParticleModifier
    {
        private float _initialOpacity = 1f;
        private float _medianOpacity = 0.5f;
        private float _finalOpacity = 0f;

        private float _median = 0.5f;

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

        public float MedianOpacity
        {
            get { return _medianOpacity; }
            set
            {
                Ensure.IsValid(value);
                Ensure.WithinRange(value, 0, 1);

                _medianOpacity = value;
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
            float opacityDeltaFirst = _medianOpacity - _initialOpacity;
            float opacityDeltaLast = _finalOpacity - _medianOpacity;
            Particle particle = iterator.First;

            do
            {
                if (particle.Age < _median)
                {
                    particle.Opacity = _initialOpacity + opacityDeltaFirst * (particle.Age / _median);
                }
                else
                {
                    particle.Opacity = _medianOpacity + opacityDeltaLast * (particle.Age - _median) / (1f - _median);                   
                }

            } while (iterator.MoveNext(ref particle));
        }
    }
}
