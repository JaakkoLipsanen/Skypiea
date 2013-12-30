using Flai;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.General;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;
using Skypiea.Prefabs;

namespace Skypiea.Systems
{
    // todo: "WeaponDropLifeTimeSystem"? destroyes them after a certain amount of time
    public class WeaponDropGeneratorSystem : EntitySystem
    {
        private const float WeaponDropTestInterval = 8;
        private readonly Timer _weaponDropTimer = new Timer(float.MaxValue);
        private CPlayerInfo _playerInfo;
        private IPlayerPassiveStats _passiveStats;

        protected override void Initialize()
        {
            _playerInfo = this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
            _passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
            _weaponDropTimer.SetTickTime(WeaponDropGeneratorSystem.WeaponDropTestInterval / _passiveStats.DropIncreaseMultiplier);
        }

        protected override void Update(UpdateContext updateContext)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            _weaponDropTimer.Update(updateContext);
            if (_weaponDropTimer.HasFinished)
            {
                _weaponDropTimer.Restart();

                const float DropOdds = 0.75f;
                if (Global.Random.NextFromOdds(DropOdds))
                {
                    this.CreateWeaponDrop();
                }
            }
        }

        public void CreateWeaponDrop()
        {
            CTransform2D playerTransform =
                this.EntityWorld.FindEntityByName(EntityNames.Player).Transform;

            const float MinDistanceFromBorder = SkypieaConstants.PixelsPerMeter * 6;
            Vector2 dropPosition = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, SkypieaConstants.MapWidthInPixels - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, SkypieaConstants.MapHeightInPixels - MinDistanceFromBorder),
                playerTransform.Position, SkypieaConstants.PixelsPerMeter * 5);

            WeaponType dropType = WeaponTypeHelper.GenerateWeaponDropType();
            if (dropType == WeaponType.Flamethrower && _passiveStats.ChanceForWaterBlaster)
            {
                // 50% chance... should it be lower?
                if (Global.Random.NextFromOdds(0.5f))
                {
                    dropType = WeaponType.Waterblaster;
                }
            }

            this.EntityWorld.CreateEntityFromPrefab<WeaponDropPrefab>(dropPosition, dropType);
        }
    }
}
