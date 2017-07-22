using Flai;
using Flai.CBES;
using Skypiea.Model.Weapons;

namespace Skypiea.Components
{
    public class CWeapon : Component // poolable?
    {
        public Weapon Weapon { get; set; }
        protected internal override void PreUpdate(UpdateContext updateContext)
        {
            this.Weapon.Update(updateContext, this.EntityWorld);
        }
    }
}
