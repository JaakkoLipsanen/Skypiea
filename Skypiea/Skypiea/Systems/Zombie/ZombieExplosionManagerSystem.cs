using Flai.CBES.Systems;
using Flai.Graphics.Particles;
using Skypiea.Messages;
using Skypiea.Misc;

namespace Skypiea.Systems.Zombie
{
    public class ZombieExplosionManagerSystem : EntitySystem
    {
        private IParticleEngine _particleEngine;
        protected override void Initialize()
        {
            this.EntityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            _particleEngine = this.EntityWorld.Services.Get<IParticleEngine>();
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            ParticleEffect explosionParticleEffect = _particleEngine[ParticleEffectID.ZombieExplosion];
            explosionParticleEffect.Trigger(message.Zombie.Transform);
        }
    }
}
