using Flai;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.General;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Prefabs.Drops;

namespace Skypiea.Systems.Drops
{
    public class LifeDropGeneratorSystem : EntitySystem
    {
        private float _tickTime = Global.Random.NextFloat(75, 95);
        private readonly Timer _lifeDropTimer = new Timer(float.MaxValue);
        private CPlayerInfo _playerInfo;

        protected override void Initialize()
        {
            _playerInfo = this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
            _lifeDropTimer.SetTickTime(_tickTime / this.EntityWorld.Services.Get<IPlayerPassiveStats>().DropIncreaseMultiplier);
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
                _tickTime *= Global.Random.NextFloat(1.25f, 1.75f);
                _lifeDropTimer.SetTickTime(_tickTime);
                _lifeDropTimer.Restart();

                this.CreateLifeDrop();
            }
        }

        private void CreateLifeDrop()
        {
            CTransform2D playerTransform =
                this.EntityWorld.FindEntityByName(EntityNames.Player).Transform;

            const float MinDistanceFromBorder = SkypieaConstants.PixelsPerMeter * 5;
            Vector2 dropPosition = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, SkypieaConstants.MapWidthInPixels - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, SkypieaConstants.MapHeightInPixels - MinDistanceFromBorder),
                playerTransform.Position, SkypieaConstants.PixelsPerMeter * 5);

            this.EntityWorld.CreateEntityFromPrefab<LifeDropPrefab>(dropPosition);
        }
    }
}
