using Flai.Graphics;
using Skypiea.Model;

namespace Skypiea.View
{
    public class WorldRenderer : FlaiRenderer
    {
        private readonly World _world;
        private readonly MapRenderer _mapRenderer;
        private readonly PlayerRenderer _playerRenderer;
        private readonly ZombieRenderer _zombieRenderer;
        private readonly VirtualThumbStickRenderer _virtualThumbStickRenderer;
        private readonly BulletRenderer _bulletRenderer;
        private readonly WeaponDropRenderer _weaponDropRenderer;
        private readonly WeaponRenderer _weaponRenderer;
        private readonly ParticleEffectRenderer _particleEffectRenderer; // hmm... for start at least create the particle effects here.. but should this manage rendering too? dunno..

        public WorldRenderer(World world)
        {
            _world = world;
            _mapRenderer = new MapRenderer(world);
            _playerRenderer = new PlayerRenderer(_world.EntityWorld);
            _virtualThumbStickRenderer = new VirtualThumbStickRenderer(_world.EntityWorld);
            _bulletRenderer = new BulletRenderer(_world.EntityWorld);
            _zombieRenderer = new ZombieRenderer(_world.EntityWorld);
            _weaponDropRenderer = new WeaponDropRenderer(_world.EntityWorld);
            _weaponRenderer = new WeaponRenderer(_world.EntityWorld);
            _particleEffectRenderer = new ParticleEffectRenderer(_world.ParticleEngine);
        }

        protected override void LoadContentInner()
        {
            _particleEffectRenderer.LoadContent();
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            _mapRenderer.Draw(graphicsContext);

            _weaponDropRenderer.Draw(graphicsContext);
            _bulletRenderer.Draw(graphicsContext);

            _weaponRenderer.Draw(graphicsContext);
            _playerRenderer.Draw(graphicsContext);
            _zombieRenderer.Draw(graphicsContext);
            _particleEffectRenderer.Draw(graphicsContext);
            _mapRenderer.DrawFades(graphicsContext);
        }

        public void DrawUI(GraphicsContext graphicsContext)
        {
            _playerRenderer.DrawUI(graphicsContext);
            _virtualThumbStickRenderer.Draw(graphicsContext);
        }
    }
}
