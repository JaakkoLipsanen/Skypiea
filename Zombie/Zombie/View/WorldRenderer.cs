using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Model;
using Zombie.Prefabs;

namespace Zombie.View
{
    public class WorldRenderer : FlaiRenderer
    {
        private readonly World _world;
        private readonly TileMapRenderer _tileMapRenderer;
        private readonly PlayerRenderer _playerRenderer;
        private readonly ZombieRenderer _zombieRenderer;
        private readonly VirtualThumbStickRenderer _virtualThumbStickRenderer;
        private readonly BulletRenderer _bulletRenderer;

        public WorldRenderer(World world)
        {
            _world = world;
            _tileMapRenderer = new TileMapRenderer(world.TileMap);
            _playerRenderer = new PlayerRenderer(_world.EntityWorld);
            _virtualThumbStickRenderer = new VirtualThumbStickRenderer(_world.EntityWorld);
            _bulletRenderer = new BulletRenderer(_world.EntityWorld);
            _zombieRenderer = new ZombieRenderer(_world.EntityWorld);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            _tileMapRenderer.Draw(graphicsContext);

            _bulletRenderer.Draw(graphicsContext);
            _playerRenderer.Draw(graphicsContext);
            _zombieRenderer.Draw(graphicsContext);
        }

        public void DrawUI(GraphicsContext graphicsContext)
        {
            _virtualThumbStickRenderer.Draw(graphicsContext);
        }
    }
}
