
using Flai;
using Flai.CBES;
using Flai.General;
using Zombie.Components;

namespace Zombie.Model.Weapons
{
    // for stuff like "weapon-drop" graphics etc
    public enum WeaponType
    {
        AssaultRifle,    
        Shotgun,
    }

    public static class WeaponTypeHelper
    {
        private static readonly WeaponType[] _weaponTypes = EnumHelper.GetValues<WeaponType>();
        public static WeaponType GenerateWeaponDropType()
        {
            // this could do some weighting or something.. 
            // exclude assault rifle
            return _weaponTypes[Global.Random.Next(1, _weaponTypes.Length)];
        }

        public static char ToChar(this WeaponType weaponType)
        {
            return EnumHelper.GetName(weaponType)[0]; // hack but whatever
        }
    }

    public abstract class Weapon
    {
        // "bullets" but also fire in the flamethrower etc. when zero
        public abstract int? BulletsRemaining { get; }
        public abstract bool CanShoot { get; }
        public abstract WeaponType Type { get; }

        public virtual bool IsFinished
        {
            get { return this.BulletsRemaining != null && this.BulletsRemaining <= 0; }
        }

        public virtual void Update(UpdateContext updateContext) { }
        public abstract void Shoot(EntityWorld entityWorld, TransformComponent parentTransform);
    }
}
