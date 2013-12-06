using Flai;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.General;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Prefabs.Zombies;

namespace Skypiea.Systems.Zombie
{
    // ZombieSpawnManagerSystem ? -> do it probably
    public class ZombieSpawnManagerSystem : EntitySystem
    {
        private readonly Timer _zombieTimer = new Timer(0.3f);
        private CPlayerInfo _playerInfo;

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PreUpdate; }
        }

        protected override void Initialize()
        {
            _playerInfo = this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
        }

        protected override void Update(UpdateContext updateContext)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            _zombieTimer.Update(updateContext);
            if (_zombieTimer.HasFinished)
            {
                this.SpawnZombie();
                _zombieTimer.Restart();
            }
        }

        private void SpawnZombie()
        {
            const float MinDistanceFromBorder = -Tile.Size * 4;
            const float MinDistanceFromPlayer = 400f; // Tile.Size * 10;

            World world = this.EntityWorld.Services.Get<World>();
            CTransform2D playerTransform = this.EntityWorld.FindEntityByName(EntityNames.Player).Transform;
            Vector2 position = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, world.Width * Tile.Size - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, world.Height * Tile.Size - MinDistanceFromBorder),
                playerTransform.Position, MinDistanceFromPlayer);

            if (Global.Random.NextFromOdds(0.025f))
            {
                this.EntityWorld.CreateEntityFromPrefab<RusherZombiePrefab>(position);
            }
            else if (Global.Random.NextFromOdds(0.025f))
            {
                this.EntityWorld.CreateEntityFromPrefab<FatZombiePrefab>(position);
            }
            else
            {
                this.EntityWorld.CreateEntityFromPrefab<BasicZombiePrefab>(position);           
            }
        }
    }
}
