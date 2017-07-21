using Flai.CBES;
using Skypiea.Model.Weapons;

namespace Skypiea.Messages
{
    public class WeaponChangedMessage : PoolableMessage
    {
        public Weapon OldWeapon { get; private set; }
        public Weapon NewWeapon { get; private set; }

        public void Initialize(Weapon oldWeapon, Weapon newWeapon)
        {
            this.OldWeapon = oldWeapon;
            this.NewWeapon = newWeapon;    
        }

        protected override void Cleanup()
        {
            this.OldWeapon = null;
            this.NewWeapon = null;  
        }
    }
}
