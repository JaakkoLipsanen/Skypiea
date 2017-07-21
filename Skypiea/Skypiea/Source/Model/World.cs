using Flai;
using Flai.CBES;
using Flai.Graphics.Particles;
using Skypiea.Systems;
using Skypiea.Systems.Drops;
using Skypiea.Systems.Player;
using Skypiea.Systems.Zombie;
using Skypiea.Systems.Zombie.AI;

namespace Skypiea.Model
{
    public class World
    {
        private readonly EntityWorld _entityWorld;
        private readonly ParticleEngine _particleEngine;

        public EntityWorld EntityWorld
        {
            get { return _entityWorld; }
        }

        // meh.. this sorta belongs to view but also sorta here... ParticleEngine = Model, ParticleRenderer = View? I guess so
        public ParticleEngine ParticleEngine
        {
            get { return _particleEngine; }
        }

        public World()
            : this(false)
        {
        }

        private World(bool isSimulation)
        {
            _entityWorld = new EntityWorld();
            _particleEngine = new ParticleEngine();

            _entityWorld.Services.Add<IParticleEngine>(_particleEngine);

            this.CreateSystems(isSimulation);
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

        private void CreateSystems(bool isSimulation)
        {
            _entityWorld.AddSystem<ZombieAttackSystem>();
            _entityWorld.AddSystem<ZombieSpawnSystem>();
            _entityWorld.AddSystem<GoldenGoblinSpawnSystem>();
            _entityWorld.AddSystem<ZombieHealthSystem>();
            _entityWorld.AddSystem<ZombieSpatialMapSystem>();
            _entityWorld.AddSystem<ZombieExplosionSystem>();
            _entityWorld.AddSystem<GoldenGoblinDeathSystem>();

            _entityWorld.AddSystem<BasicZombieAISystem>();
            _entityWorld.AddSystem<RusherZombieAISystem>();
            _entityWorld.AddSystem<GoldenGoblinAISystem>();

            _entityWorld.AddSystem<PlayerControllerSystem>();
            _entityWorld.AddSystem<PlayerManagerSystem>();
            _entityWorld.AddSystem<PlayerDropPickupSystem>();
            _entityWorld.AddSystem<PlayerWeaponManagerSystem>();
            _entityWorld.AddSystem<PlayerDeathExplosionSystem>();

            _entityWorld.AddSystem<BulletCollisionSystem>();
            _entityWorld.AddSystem<BulletOutOfBoundsDestroySystem>();

            _entityWorld.AddSystem<WeaponDropGeneratorSystem>();
            _entityWorld.AddSystem<LifeDropGeneratorSystem>();
            _entityWorld.AddSystem<BlackBoxGeneratorSystem>();

            _entityWorld.AddSystem<VelocitySystem>();
            _entityWorld.AddSystem<BoosterManagerSystem>();
            _entityWorld.AddSystem<ZombieStatsSystem>();
            _entityWorld.AddSystem<VirtualThumbstickSystem>();

            if (!isSimulation)
            {
                _entityWorld.AddSystem<AchievementSystem>();
                _entityWorld.AddSystem<HighscoreSystem>();;
                _entityWorld.AddSystem<StatsTrackerSystem>();
            }

            _entityWorld.AddSystem<PlayerPassiveSystem>();
        }

        public static World CreateSimulationWorld()
        {
            return new World(true);
        }
    }
}
