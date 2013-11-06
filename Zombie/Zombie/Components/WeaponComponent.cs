using Flai;
using Flai.CBES;
using Zombie.Model.Weapons;

namespace Zombie.Components
{
    public class WeaponComponent : Component
    {
        public Weapon Weapon { get; set; }
        protected override void PreUpdate(UpdateContext updateContext)
        {
            this.Weapon.Update(updateContext);
        }
    }
}
