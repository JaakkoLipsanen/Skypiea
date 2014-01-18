using Flai;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.General;
using Microsoft.Xna.Framework;
using Skypiea.Misc;
using Skypiea.Prefabs.Drops;

namespace Skypiea.Systems.Drops
{
    public class BlackBoxGeneratorSystem : EntitySystem
    {
        private readonly Timer _spawnTimer = new Timer(5);
        protected override void Update(UpdateContext updateContext)
        {
            // for now, black boxes can drop only from goblins
            return;

            _spawnTimer.Update(updateContext);
            if (_spawnTimer.HasFinished)
            {
                _spawnTimer.Restart();
                this.SpawnBlackBox();
            }
        }

        private void SpawnBlackBox()
        {
            CTransform2D playerTransform =
                this.EntityWorld.FindEntityByName(EntityNames.Player).Transform;

            const float MinDistanceFromBorder = SkypieaConstants.PixelsPerMeter * 5;
            Vector2 dropPosition = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, SkypieaConstants.MapWidthInPixels - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, SkypieaConstants.MapHeightInPixels - MinDistanceFromBorder),
                playerTransform.Position, SkypieaConstants.PixelsPerMeter * 5);

            this.EntityWorld.CreateEntityFromPrefab<BlackBoxPrefab>(dropPosition);
        }
    }
}
