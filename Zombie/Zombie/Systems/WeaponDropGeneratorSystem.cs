using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model;
using Zombie.Model.Weapons;
using Zombie.Prefabs;

namespace Zombie.Systems
{
    // todo: "WeaponDropLifeTimeSystem"? destroyes them after a certain amount of time
    public class WeaponDropGeneratorSystem : EntitySystem
    {
        private const float WeaponDropTestInterval = 3f;
        private readonly Timer _weaponDropTimer = new Timer(WeaponDropGeneratorSystem.WeaponDropTestInterval);

        protected override void Update(UpdateContext updateContext)
        {
            _weaponDropTimer.Update(updateContext);
            if (_weaponDropTimer.HasFinished)
            {
                _weaponDropTimer.Restart();
                const float DropOdds = 0.25f;
                if (Global.Random.NextFromOdds(DropOdds))
                {
                    this.CreateWeaponDrop();
                }
            }
        }

        private void CreateWeaponDrop()
        {
            World world = this.EntityWorld.GetService<World>();
            TransformComponent playerTransform =
                this.EntityWorld.FindEntityByName(EntityTags.Player).Get<TransformComponent>();

            const float MinDistanceFromBorder = Tile.Size * 5;
            Vector2 dropPosition = FlaiAlgorithms.GenerateRandomVector2(
                new Range(MinDistanceFromBorder, world.Width * Tile.Size - MinDistanceFromBorder),
                new Range(MinDistanceFromBorder, world.Height * Tile.Size - MinDistanceFromBorder),
                playerTransform.Position, Tile.Size * 5);

            this.EntityWorld.AddEntity(Prefab.CreateInstance<WeaponDropPrefab>(dropPosition, WeaponTypeHelper.GenerateWeaponDropType(/* some arguments maybe... */)));
        }
    }
}
