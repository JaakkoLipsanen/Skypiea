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
            return WeaponFactory.CreateWeapon(this.Type);
        }

        protected override void Cleanup()
        {
            this.Type = (WeaponType)(-1);
        }
    }

    public class CLifeDrop : PoolableComponent
    {        
    }
}
