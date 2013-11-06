using Flai.CBES;

namespace Zombie.Components
{
    public class HealthComponent : Component
    {
        public int MaximumHealth { get; private set; }
        public int CurrentHealth { get; private set; }

        public bool IsAlive
        {
            get { return this.CurrentHealth > 0; }
        }

        public HealthComponent(int health)
        {
            this.MaximumHealth = health;
            this.CurrentHealth = health;
        }

        public bool TakeDamage(int damage)
        {
            this.CurrentHealth -= damage;
            if (this.CurrentHealth <= 0)
            {
                this.CurrentHealth = 0;
                return true;
            }

            return false;
        }   
    }
}
