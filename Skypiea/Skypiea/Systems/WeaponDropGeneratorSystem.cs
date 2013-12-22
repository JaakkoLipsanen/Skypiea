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
        private const float WeaponDropTestInterval = 6;
        private readonly Timer _weaponDropTimer = new Timer(WeaponDropGeneratorSystem.WeaponDropTestInterval);
        private CPlayerInfo _playerInfo;

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

        private void CreateWeaponDrop()
        {
            World world = this.EntityWorld.Services.Get<World>();
            CTransform2D playerTransform =
                this.EntityWorld.FindEntityByName(EntityNames.Player).Transform;

            const float MinDistanceFromBorder = Tile.Size * 6;
            Vector2 dropPosition = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, world.Width * Tile.Size - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, world.Height * Tile.Size - MinDistanceFromBorder),
                playerTransform.Position, Tile.Size * 5);

            this.EntityWorld.CreateEntityFromPrefab<WeaponDropPrefab>(dropPosition, WeaponTypeHelper.GenerateWeaponDropType(/* some arguments maybe... */));
        }
    }
}
