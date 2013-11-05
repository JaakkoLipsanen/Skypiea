using Flai.CBES;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model.Weapons;

namespace Zombie.Prefabs
{
    public class PlayerPrefab : Prefab
     {
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            entity.Add<TransformComponent>();
            entity.Add<CameraComponent>();
            entity.Add<WeaponComponent>();
            entity.Get<WeaponComponent>().Weapon = new AssaultRifleWeapon();
            entity.Tag = EntityTags.Player;

            return entity;
        }
     }
}
