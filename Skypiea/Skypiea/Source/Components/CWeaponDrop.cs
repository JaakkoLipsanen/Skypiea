using Flai.CBES;
using Skypiea.Model.Weapons;

namespace Skypiea.Components
{
    public class CWeaponDrop : PoolableComponent
    {
        public WeaponType Type { get; private set; }
        public void Initialize(WeaponType type)
        {
            this.Type = type;
        }

        public Weapon BuildWeapon()
        {
            return this.BuildWeapon(1);
        }

        public Weapon BuildWeapon(float ammoMultiplier)
        {
            return WeaponFactory.CreateWeapon(this.Type, ammoMultiplier);
        }

        protected internal override void Cleanup()
        {
            this.Type = (WeaponType)(-1);
        }
    }

    public class CLifeDrop : PoolableComponent
    {
    }
}
