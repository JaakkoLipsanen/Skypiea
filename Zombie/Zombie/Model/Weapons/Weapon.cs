
using Flai;
using Flai.CBES;
using Zombie.Components;

namespace Zombie.Model.Weapons
{
    public abstract class Weapon
    {
        public abstract bool CanShoot { get; }
        public abstract bool IsFinished { get; } // "IsEmpty"?

        public virtual void Update(UpdateContext updateContext) { }
        public abstract void Shoot(EntityWorld entityWorld, TransformComponent parentTransform);
    }
}
