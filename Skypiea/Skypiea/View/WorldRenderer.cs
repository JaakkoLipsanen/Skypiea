using Flai;
using Flai.Graphics;
using Flai.Ui;
using Skypiea.Model;
using Skypiea.Screens;

namespace Skypiea.View
{
    public class WorldRenderer : FlaiRenderer
    {
        private readonly World _world;

        private readonly MapRenderer _mapRenderer;
        private readonly PlayerRenderer _playerRenderer;
        private readonly ZombieRenderer _zombieRenderer;
        private readonly BulletRenderer _bulletRenderer;
        private readonly DropRenderer _dropRenderer;
        private readonly WeaponRenderer _weaponRenderer;
        private readonly ParticleEffectRenderer _particleEffectRenderer; // hmm... for start at least create the particle effects here.. but should this manage rendering too? dunno..

        private readonly BoosterStateRenderer _boosterStateRenderer;
        private readonly AchievementRenderer _achievementRenderer;
        private readonly VirtualThumbStickRenderer _virtualThumbStickRenderer;
        private readonly DropArrowRenderer _dropArrowRenderer;

        private float _playerUIAlpha = 0f;
        private float _otherUIAlpha = 0f;

        public WorldRenderer(World world)
        {
            _world = world;
            _mapRenderer = new MapRenderer(world);
            _playerRenderer = new PlayerRenderer(_world.EntityWorld);
            _zombieRenderer = new ZombieRenderer(_world.EntityWorld);
            _bulletRenderer = new BulletRenderer(_world.EntityWorld);
            _dropRenderer = new DropRenderer(_world.EntityWorld);
            _weaponRenderer = new WeaponRenderer(_world.EntityWorld);
            _particleEffectRenderer = new ParticleEffectRenderer(_world.EntityWorld, _world.ParticleEngine);

            _boosterStateRenderer = new BoosterStateRenderer(_world.EntityWorld);
            _achievementRenderer = new AchievementRenderer(_world.EntityWorld);
            _virtualThumbStickRenderer = new VirtualThumbStickRenderer(_world.EntityWorld);
            _dropArrowRenderer = new DropArrowRenderer(_world.EntityWorld);
        }

        protected override void LoadContentInner()
        {
            _particleEffectRenderer.LoadContent();
        }

        protected override void UpdateInner(UpdateContext updateContext)
        {
            _boosterStateRenderer.Update(updateContext);
            _achievementRenderer.Update(updateContext);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            _mapRenderer.Draw(graphicsContext);
            _dropRenderer.Draw(graphicsContext);

            _zombieRenderer.Draw(graphicsContext);
            _particleEffectRenderer.Draw(graphicsContext);
            _bulletRenderer.Draw(graphicsContext);
            _weaponRenderer.Draw(graphicsContext);

            _playerRenderer.Draw(graphicsContext);
            _mapRenderer.DrawFades(graphicsContext);
        }

        public void DrawUI(GraphicsContext graphicsContext, BasicUiContainer levelUiContainer)
        {
            IGameContainer gameContainer = _world.EntityWorld.Services.Get<IGameContainer>();
            _playerUIAlpha = FlaiMath.Clamp(_playerUIAlpha + ((gameContainer.IsActive || !gameContainer.IsGameOver) ? 1 : -1) * 4 * graphicsContext.DeltaSeconds, 0, 1); // player ui doesn't fade out when paused
            _otherUIAlpha = FlaiMath.Clamp(_otherUIAlpha + (gameContainer.IsActive ? 1 : -1) * 4 * graphicsContext.DeltaSeconds, 0, 1);

            graphicsContext.SpriteBatch.GlobalAlpha.Push(_playerUIAlpha);
            _playerRenderer.DrawUI(graphicsContext);
            graphicsContext.SpriteBatch.GlobalAlpha.Pop();
            
            graphicsContext.SpriteBatch.GlobalAlpha.Push(_otherUIAlpha);
            levelUiContainer.Draw(graphicsContext, true);
            _virtualThumbStickRenderer.Draw(graphicsContext);

            _dropArrowRenderer.Draw(graphicsContext);
            _boosterStateRenderer.Draw(graphicsContext);
            _achievementRenderer.Draw(graphicsContext);
            graphicsContext.SpriteBatch.GlobalAlpha.Pop();
        }
    }
}
