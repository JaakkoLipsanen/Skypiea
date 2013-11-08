using Flai.CBES;
using Zombie.Model.Weapons;

namespace Zombie.Components
{
    public class WeaponDropComponent : Component
    {
        public WeaponType Type { get; private set; }
        public WeaponDropComponent(WeaponType type)
        {
            this.Type = type;
        }

        public Weapon BuildWeapon()
        {
            return WeaponFactory.CreateWeapon(this.Type);
        }
    }
}
