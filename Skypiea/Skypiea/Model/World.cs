using Flai;
using Flai.CBES;
using Flai.General;
using Flai.Graphics.Particles;
using Skypiea.Systems;
using Skypiea.Systems.Player;
using Skypiea.Systems.Zombie;

namespace Skypiea.Model
{
    public class World
    {
        private readonly ReadOnlyTileMap<TileType> _tileMap;
        private readonly EntityWorld _entityWorld;
        private readonly ParticleEngine _particleEngine;

        public EntityWorld EntityWorld
        {
            get { return _entityWorld; }
        }

        public ReadOnlyTileMap<TileType> TileMap
        {
            get { return _tileMap; }
        }

        // meh.. this sorta belongs to view but also sorta here... ParticleEngine = Model, ParticleRenderer = View? I guess so
        public ParticleEngine ParticleEngine
        {
            get { return _particleEngine; }
        }

        public int Width
        {
            get { return _tileMap.Width; }
        }

        public int Height
        {
            get { return _tileMap.Height; }
        }

        public World(ITileMap<TileType> tiles)
        {
            _tileMap = new ReadOnlyTileMap<TileType>(tiles);
            _entityWorld = new EntityWorld();
            _particleEngine = new ParticleEngine();

            _entityWorld.Services.Add(this);
            _entityWorld.Services.Add<IParticleEngine>(_particleEngine);

            this.CreateSystems();
        }

        public void Initialize()
        {
            _entityWorld.Initialize();
        }

        public void Update(UpdateContext updateContext)
        {
            _particleEngine.Update(updateContext);
            _entityWorld.Update(updateContext);
        }

        private void CreateSystems()
        { 
            _entityWorld.AddSystem<ZombieAttackSystem>();
            _entityWorld.AddSystem<PlayerControllerSystem>();
            _entityWorld.AddSystem<VelocitySystem>();
            _entityWorld.AddSystem<ZombieSpawnManagerSystem>();
            _entityWorld.AddSystem<BasicZombieAISystem>();
            _entityWorld.AddSystem<BulletCollisionSystem>();
            _entityWorld.AddSystem<BulletOutOfBoundsDestroySystem>();
            _entityWorld.AddSystem<PlayerManagerSystem>();
            _entityWorld.AddSystem<ZombieHealthSystem>();
            _entityWorld.AddSystem<WeaponDropGeneratorSystem>();
            _entityWorld.AddSystem<PlayerWeaponPickupSystem>();
            _entityWorld.AddSystem<PlayerWeaponManagerSystem>();
            _entityWorld.AddSystem<RusherZombieAISystem>();
            _entityWorld.AddSystem<ZombieExplosionManagerSystem>();
        }
    }
}
