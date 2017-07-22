using Flai;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.General;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Zombies;

namespace Skypiea.Systems.Zombie
{
    public class GoldenGoblinSpawnSystem : EntitySystem
    {
        private const float FirstMinTime = 55;
        private const float FirstMaxTime = 85;
        private const float TestMultiplier = 1.35f;
        private const float ChanceToSpawn = 0.5f;

        private readonly Timer _zombieTimer = new Timer(Global.Random.NextFloat(GoldenGoblinSpawnSystem.FirstMinTime, GoldenGoblinSpawnSystem.FirstMaxTime)); // first gg has a change to
        private CPlayerInfo _playerInfo;

        protected internal override int ProcessOrder
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
                if (Global.Random.NextFromOdds(GoldenGoblinSpawnSystem.ChanceToSpawn))
                {
                    this.SpawnZombie();
                }

                _zombieTimer.SetTickTime(_zombieTimer.TickTime * GoldenGoblinSpawnSystem.TestMultiplier);
                _zombieTimer.Restart();
            }
        }

        private void SpawnZombie()
        {
            const float MinDistanceFromBorder = -SkypieaConstants.PixelsPerMeter * 4;
            const float MinDistanceFromPlayer = 480; // sqrt(400^2 + 240^2) == 466

            CTransform2D playerTransform = this.EntityWorld.FindEntityByName(EntityNames.Player).Transform;
            Vector2 position = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, SkypieaConstants.MapWidthInPixels - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, SkypieaConstants.MapHeightInPixels - MinDistanceFromBorder),
                playerTransform.Position, MinDistanceFromPlayer);

            this.EntityWorld.CreateEntityFromPrefab<GoldenGoblinPrefab>(position);
        }
    }
}
