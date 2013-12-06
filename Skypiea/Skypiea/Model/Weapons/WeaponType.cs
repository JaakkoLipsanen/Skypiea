using Flai;
using Flai.General;
using System.Linq;

namespace Skypiea.Model.Weapons
{
    // for stuff like "weapon-drop" graphics etc
    public enum WeaponType
    {
        AssaultRifle = 0,
        Shotgun = 1,
        RocketLauncher = 2,
        Laser = 3,
        Minigun = 4,
    }

    public static class WeaponTypeHelper
    {
        private static readonly WeaponType[] _weaponTypes = EnumHelper.GetValues<WeaponType>().ToArray();
        private static readonly string[] _weaponDisplayNames = EnumHelper.GetNames<WeaponType>().Select(name => Common.AddSpaceBeforeCaps(name)).ToArray();
        public static WeaponType GenerateWeaponDropType()
        {
            // this could do some weighting or something.. 
            // exclude assault rifle
            return _weaponTypes[Global.Random.Next((int)WeaponType.Shotgun, _weaponTypes.Length)];
        }

        public static string GetDisplayName(this WeaponType weaponType)
        {
            return _weaponDisplayNames[(int)weaponType]; // no idea if this works
        }

        public static char ToChar(this WeaponType weaponType)
        {
            return EnumHelper.GetName(weaponType)[0]; // hack but whatever
        }
    }
}