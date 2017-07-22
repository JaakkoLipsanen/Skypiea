using System;
using System.Collections.Generic;

namespace Flai.Graphics.Particles.Modifiers
{
    public sealed class ParticleModifierCollection : List<ParticleModifier>
    {
        public T Get<T>()
            where T : ParticleModifier
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].GetType() == typeof (T))
                {
                    return (T)this[i];
                }
            }

            throw new ArgumentException("No modifier of type " + typeof(T).Name + " was found");
        }

        internal void ProcessParticles(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Process(updateContext, ref iterator);
                iterator.Reset();
            }
        }
    }
}
