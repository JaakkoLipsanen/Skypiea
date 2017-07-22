using System.Collections.Generic;
using Flai.DataStructures;

namespace Flai.Graphics.Particles
{
    public interface IParticleEngine
    {
        void Add(uint key, ParticleEffect particleEffect);
        bool ContainsID(uint particleEffectID);
        ParticleEffect this[uint key] { get; }

        void TerminateAll();
    }

    public class ParticleEngine : IParticleEngine
    {
        private readonly Dictionary<uint, ParticleEffect> _particleEffectsByKey = new Dictionary<uint, ParticleEffect>();
        private readonly List<ParticleEffect> _particleEffects = new List<ParticleEffect>();
        private readonly ReadOnlyList<ParticleEffect> _readOnlyParticleEffects;

        public ReadOnlyList<ParticleEffect> ParticleEffects
        {
            get { return _readOnlyParticleEffects; }
        }

        public ParticleEngine()
        {
            _readOnlyParticleEffects = new ReadOnlyList<ParticleEffect>(_particleEffects);
        }

        public void Update(UpdateContext updateContext)
        {
            foreach (ParticleEffect particleEffect in _particleEffectsByKey.Values)
            {
                particleEffect.Update(updateContext);
            }
        }

        public void Add(uint key, ParticleEffect particleEffect)
        {
            _particleEffectsByKey.Add(key, particleEffect);
            _particleEffects.Add(particleEffect);
        }

        public bool ContainsID(uint particleEffectID)
        {
            return _particleEffectsByKey.ContainsKey(particleEffectID);
        }

        public ParticleEffect this[uint key]
        {
            get { return _particleEffectsByKey[key]; }
        }

        public void TerminateAll()
        {
            foreach (ParticleEffect particleEffect in _particleEffects)
            {
                particleEffect.Terminate();
            }
        }
    }
}
