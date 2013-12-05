using System;
using Flai;
using Flai.CBES;
using Flai.Graphics.Particles;
using Flai.Graphics.Particles.Controllers;

namespace Skypiea.Components
{
    public class CParticleEmitter : PoolableComponent
    {
        private ParticleEffectController _particleController;
        private ParticleEffect _particleEffect;

        public void Initialize<T>(uint particleEffectID, T transformParticleController)
            where T : ParticleEffectController, ITransformParticleController
        {
            Ensure.NotNull(transformParticleController);
            _particleController = transformParticleController;

            IParticleEngine particleEngine = this.Entity.EntityWorld.Services.Get<IParticleEngine>();
            _particleEffect = particleEngine[particleEffectID];
            _particleEffect.Controllers.Add(_particleController);
        }

        protected override void Cleanup()
        {
            if (_particleEffect != null && _particleController != null)
            {
                if (!_particleEffect.Controllers.Remove(_particleController))
                {
                    throw new InvalidOperationException("out of sync");
                }
            }

            _particleEffect = null;
            _particleController = null;
        }
    }
}
