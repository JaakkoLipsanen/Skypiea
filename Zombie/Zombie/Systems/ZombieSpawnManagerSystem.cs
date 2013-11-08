using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model;
using Zombie.Prefabs;

namespace Zombie.Systems
{
    // ZombieSpawnManagerSystem ? -> do it probably
    public class ZombieSpawnManagerSystem : EntitySystem
    {
        private readonly Timer _zombieTimer = new Timer(0.75f);
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PreUpdate; }
        }

        protected override void Update(UpdateContext updateContext)
        {
            _zombieTimer.Update(updateContext);
            if (_zombieTimer.HasFinished)
            {
                this.SpawnZombie(updateContext);
                _zombieTimer.Restart();
            }
        }

        private void SpawnZombie(UpdateContext updateContext)
        {
            const float MinDistanceFromBorder = Tile.Size * 2;
            const float MinDistanceFromPlayer = 400f; // Tile.Size * 10;

            World world = this.EntityWorld.GetService<World>();
            TransformComponent playerTransform = this.EntityWorld.FindEntityByName(EntityTags.Player).Get<TransformComponent>();
            Vector2 position = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, world.Width * Tile.Size - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, world.Height * Tile.Size - MinDistanceFromBorder),
                playerTransform.Position, MinDistanceFromPlayer);

            if (Global.Random.NextFromOdds(0.25f))
            {
                this.EntityWorld.AddEntity(Prefab.CreateInstance<RusherZombiePrefab>(position));
            }
            else
            {
                this.EntityWorld.AddEntity(Prefab.CreateInstance<BasicZombiePrefab>(position));              
            }
        }
    }
}
