
using System;

namespace Skypiea.Model.Weapons
{
    public static class WeaponFactory
    {
        public static Weapon CreateWeapon(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.AssaultRifle:
                    return new AssaultRifle();

                case WeaponType.Shotgun:
                    return new Shotgun();

                case WeaponType.RocketLauncher:
                    return new RocketLauncher();

                case WeaponType.Laser:
                    return new Laser();

                default:
                    throw new ArgumentOutOfRangeException("weaponType");
            }
        }

        public static Weapon CreateDefaultWeapon()
        {
            return new AssaultRifle();
        }
    }
}
