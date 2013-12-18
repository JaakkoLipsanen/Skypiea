using Flai;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.General;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Prefabs;

namespace Skypiea.Systems
{
    public class LifeDropGeneratorSystem : EntitySystem
    {
        private float _tickTime = Global.Random.NextFloat(45, 75);
        private readonly Timer _lifeDropTimer = new Timer(float.MaxValue);
        private CPlayerInfo _playerInfo;
        protected override void Initialize()
        {
            _playerInfo = this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
            _lifeDropTimer.SetTickTime(_tickTime);
        }

        protected override void Update(UpdateContext updateContext)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            _lifeDropTimer.Update(updateContext);
            if (_lifeDropTimer.HasFinished)
            {
                _tickTime *= Global.Random.NextFloat(1f, 1.5f);
                _lifeDropTimer.SetTickTime(_tickTime);
                _lifeDropTimer.Restart();

                this.CreateLifeDrop();
            }
        }

        private void CreateLifeDrop()
        {
            World world = this.EntityWorld.Services.Get<World>();
            CTransform2D playerTransform =
                this.EntityWorld.FindEntityByName(EntityNames.Player).Transform;

            const float MinDistanceFromBorder = Tile.Size * 5;
            Vector2 dropPosition = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, world.Width * Tile.Size - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, world.Height * Tile.Size - MinDistanceFromBorder),
                playerTransform.Position, Tile.Size * 5);

            this.EntityWorld.CreateEntityFromPrefab<LifeDropPrefab>(dropPosition);
        }
    }
}
