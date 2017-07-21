using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.Prefabs
{
    public class PlayerPrefab : Prefab
    {
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            entity.Transform.Position = parameters.Get<Vector2>(0);
            entity.Add(new CPlayerInfo(2));
            entity.Add<CCamera2D>(new CPlayerCamera2D());
            entity.Add<CWeapon>().Weapon = WeaponFactory.CreateDefaultWeapon();

            entity.Tag = EntityTags.Player;
        }
    }
}
