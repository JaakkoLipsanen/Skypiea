using Flai.CBES;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model.Weapons;

namespace Zombie.Prefabs
{
    public class WeaponDropPrefab : Prefab
    {
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            Vector2 position = (Vector2)parameters[0];
            WeaponType weaponType = (WeaponType) parameters[1];
            
            entity.Add(new TransformComponent(position));
            entity.Add(new WeaponDropComponent(weaponType));
            entity.Tag = EntityTags.WeaponDrop;
            return entity;
        }
    }
}
