using Flai.Graphics.Particles;
using Flai.Graphics.Particles.Controllers;
using System;

namespace Flai.CBES.Components
{
    public class CParticleEmitter2D : PoolableComponent
    {
        private ParticleEffectController _particleController;
        private ParticleEffect _particleEffect;

        public void Initialize<T>(uint particleEffectID, T transformParticleController)
            where T : ParticleEffectController, ITransformSettable
        {
            Ensure.NotNull(transformParticleController);
            _particleController = transformParticleController;
            transformParticleController.SetTransform(this.Transform);

            IParticleEngine particleEngine = this.Entity.EntityWorld.Services.Get<IParticleEngine>();

            if (particleEngine.ContainsID(particleEffectID))
            {
                _particleEffect = particleEngine[particleEffectID];
                _particleEffect.Controllers.Add(_particleController);
            }
            else
            {
                FlaiLogger.Log("CParticleEmitter2D: No particle effect with ID of {0} found!", particleEffectID);
            }
        }

        protected internal override void Cleanup()
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
