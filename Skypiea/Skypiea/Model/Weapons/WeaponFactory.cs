
using System;
using Skypiea.Misc;

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

                case WeaponType.Minigun:
                    return new Minigun();

                case WeaponType.Ricochet:
                    return new RicochetGun();

                case WeaponType.Flamethrower:
                    return new Flamethrower();

                default:
                    throw new ArgumentOutOfRangeException("weaponType");
            }
        }

        public static Weapon CreateDefaultWeapon()
        {
            return WeaponFactory.CreateWeapon(TestingGlobals.DefaultWeaponType);
        }
    }
}
