using Flai.CBES;

namespace Zombie.Components
{
    // todo: instead of doing something like this, how about make a callback "OnCollided(Entity entity (or Vector2 position tms))" and then the weapon itself (whos callback it will be calling) decides the damage etc. this.. could be A LOT better!
    public class BulletComponent : Component
    {
        public int Damage { get; private set; }
        public BulletComponent(int damage)
        {
            this.Damage = damage;
        }
    }
}
