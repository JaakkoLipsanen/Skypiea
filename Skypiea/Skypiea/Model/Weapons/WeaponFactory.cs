
using System;
using Skypiea.Misc;

namespace Skypiea.Model.Weapons
{
    public static class WeaponFactory
    {
        public static Weapon CreateWeapon(WeaponType weaponType)
        {
            return CreateWeapon(weaponType, 1);
        }

        public static Weapon CreateWeapon(WeaponType weaponType, float ammoMultiplier)
        {
            switch (weaponType)
            {
                case WeaponType.AssaultRifle:
                    return new AssaultRifle();

                case WeaponType.Shotgun:
                    return new Shotgun(ammoMultiplier);

                case WeaponType.RocketLauncher:
                    return new RocketLauncher(ammoMultiplier);

                case WeaponType.Laser:
                    return new Laser(ammoMultiplier);

                case WeaponType.Minigun:
                    return new Minigun(ammoMultiplier);

                case WeaponType.Ricochet:
                    return new RicochetGun(ammoMultiplier);

                case WeaponType.Flamethrower:
                    return new Flamethrower(ammoMultiplier);

                case WeaponType.Waterblaster:
                    return new Waterthrower(ammoMultiplier);

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
