using Flai;
using Flai.CBES;
using Flai.General;
using Zombie.Systems;

namespace Zombie.Model
{
    public class World
    {
        private readonly ReadOnlyTileMap<TileType> _tileMap;
        private readonly EntityWorld _entityWorld;

        public EntityWorld EntityWorld
        {
            get { return _entityWorld; }
        }

        public ReadOnlyTileMap<TileType> TileMap
        {
            get { return _tileMap; }
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
            _entityWorld.AddService(this);

            this.CreateSystems();
        }

        public void Initialize()
        {
            _entityWorld.Initialize();
        }

        public void Update(UpdateContext updateContext)
        {
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
            _entityWorld.AddSystem<PlayerWeaponDropPickupSystem>();
            _entityWorld.AddSystem<PlayerWeaponManagerSystem>();
            _entityWorld.AddSystem<RusherZombieAISystem>();
        }
    }
}
