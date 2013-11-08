
using System;

namespace Zombie.Model.Weapons
{
    public static class WeaponFactory
    {
        public static Weapon CreateWeapon(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.AssaultRifle:
                    return new AssaultRifleWeapon();

                case WeaponType.Shotgun:
                    return new ShotgunWeapon();

                default:
                    throw new ArgumentOutOfRangeException("weaponType");
            }
        }

        public static Weapon CreateDefaultWeapon()
        {
            return new AssaultRifleWeapon();
        }
    }
}
