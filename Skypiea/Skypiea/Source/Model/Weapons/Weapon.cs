
using Flai;
using Flai.CBES;

namespace Skypiea.Model.Weapons
{
    public abstract class Weapon
    {
        // "bullets" but also fire in the flamethrower etc. when zero
        public abstract int? AmmoRemaining { get; }
        public abstract bool CanShoot { get; }
        public abstract WeaponType Type { get; }

        public virtual bool IsFinished
        {
            get { return this.AmmoRemaining != null && this.AmmoRemaining <= 0; }
        }

        public virtual void Update(UpdateContext updateContext, EntityWorld entityWorld) { }
        public abstract void Shoot(UpdateContext updateContext, EntityWorld entityWorld, Entity playerEntity);
        public virtual void OnNewInstancePickedUp() { }
    }
}
