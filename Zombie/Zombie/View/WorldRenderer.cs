using Flai;
using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.DataStructures;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Model;
using Zombie.Model.Weapons;
using Zombie.Systems;

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
        private readonly WeaponDropRenderer _weaponDropRenderer;

        // todo: create generic "particle renderer" class?
        private readonly ZombieExplosionParticleRenderer _zombieExplosionParticleRenderer;

        public WorldRenderer(World world)
        {
            _world = world;
            _tileMapRenderer = new TileMapRenderer(world.TileMap);
            _playerRenderer = new PlayerRenderer(_world.EntityWorld);
            _virtualThumbStickRenderer = new VirtualThumbStickRenderer(_world.EntityWorld);
            _bulletRenderer = new BulletRenderer(_world.EntityWorld);
            _zombieRenderer = new ZombieRenderer(_world.EntityWorld);
            _weaponDropRenderer = new WeaponDropRenderer(_world.EntityWorld);

            _zombieExplosionParticleRenderer = new ZombieExplosionParticleRenderer(_world.EntityWorld);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            _tileMapRenderer.Draw(graphicsContext);

            _weaponDropRenderer.Draw(graphicsContext);
            _bulletRenderer.Draw(graphicsContext);
            _playerRenderer.Draw(graphicsContext);
            _zombieRenderer.Draw(graphicsContext);
            _zombieExplosionParticleRenderer.Draw(graphicsContext);
        }

        public void DrawUI(GraphicsContext graphicsContext)
        {
            _playerRenderer.DrawUI(graphicsContext);
            _virtualThumbStickRenderer.Draw(graphicsContext);
        }
    }

    internal class ZombieExplosionParticleRenderer : ComponentProcessingRenderer<TransformComponent, ZombieExplosionParticleComponent>
    {
        public ZombieExplosionParticleRenderer(EntityWorld entityWorld) 
            : base(entityWorld)
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity, TransformComponent transform, ZombieExplosionParticleComponent zombieExplosionParticle)
        {
            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, transform.Position, Color.DarkRed * zombieExplosionParticle.Alpha, 0, zombieExplosionParticle.Size);
        }
    }
}
