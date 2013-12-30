using Flai;
using Flai.CBES;
using Flai.Graphics.Particles;
using Skypiea.Systems;
using Skypiea.Systems.Player;
using Skypiea.Systems.Zombie;

namespace Skypiea.Model
{
    public class World
    {
        private readonly EntityWorld _entityWorld;
        private readonly ParticleEngine _particleEngine;
        private readonly WorldType _worldType;

        public EntityWorld EntityWorld
        {
            get { return _entityWorld; }
        }

        // meh.. this sorta belongs to view but also sorta here... ParticleEngine = Model, ParticleRenderer = View? I guess so
        public ParticleEngine ParticleEngine
        {
            get { return _particleEngine; }
        }

        public WorldType WorldType
        {
            get { return _worldType; }
        }

        public World(WorldType worldType)
        {
            _worldType = worldType;

            _entityWorld = new EntityWorld();
            _particleEngine = new ParticleEngine();

            _entityWorld.Services.Add<IParticleEngine>(_particleEngine);

            this.CreateSystems();
        }

        public void Initialize()
        {
            _entityWorld.Initialize();
        }

        public void Shutdown()
        {
            _entityWorld.Shutdown();
        }

        public void Update(UpdateContext updateContext)
        {
            _particleEngine.Update(updateContext);
            _entityWorld.Update(updateContext);
        }

        private void CreateSystems()
        {
            _entityWorld.AddSystem<ZombieAttackSystem>();
            _entityWorld.AddSystem<ZombieSpawnManagerSystem>();
            _entityWorld.AddSystem<ZombieHealthSystem>();
            _entityWorld.AddSystem<ZombieExplosionManagerSystem>();
            _entityWorld.AddSystem<ZombieSpatialMapSystem>();

            _entityWorld.AddSystem<BasicZombieAISystem>();
            _entityWorld.AddSystem<RusherZombieAISystem>();

            _entityWorld.AddSystem<PlayerControllerSystem>();
            _entityWorld.AddSystem<PlayerManagerSystem>();
            _entityWorld.AddSystem<PlayerDropPickupSystem>();
            _entityWorld.AddSystem<PlayerWeaponManagerSystem>();
            _entityWorld.AddSystem<PlayerDeathExplosionSystem>();

            _entityWorld.AddSystem<BulletCollisionSystem>();
            _entityWorld.AddSystem<BulletOutOfBoundsDestroySystem>();

            _entityWorld.AddSystem<WeaponDropGeneratorSystem>();
            _entityWorld.AddSystem<LifeDropGeneratorSystem>();

            _entityWorld.AddSystem<VelocitySystem>();
            _entityWorld.AddSystem<BoosterManagerSystem>();
            _entityWorld.AddSystem<ZombieStatsSystem>();
            _entityWorld.AddSystem<VirtualThumbstickSystem>();

            _entityWorld.AddSystem<AchievementSystem>();
            _entityWorld.AddSystem<HighscoreSystem>();
            _entityWorld.AddSystem<PlayerPassiveSystem>();
        }
    }
}
