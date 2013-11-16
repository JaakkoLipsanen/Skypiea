using Flai.CBES;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model.Weapons;

namespace Zombie.Prefabs
{
    public class PlayerPrefab : Prefab
     {
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            Vector2 position = (Vector2) parameters[0];

            entity.Add(new PlayerInfoComponent(2));
            entity.Add(new TransformComponent(position));
            entity.Add<CameraComponent>(new PlayerCameraComponent());
            entity.Add<WeaponComponent>();
            entity.Get<WeaponComponent>().Weapon = new AssaultRifleWeapon();
            entity.Tag = EntityTags.Player;

            return entity;
        }
     }
}
