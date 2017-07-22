using Flai.CBES;

namespace Skypiea.Components
{
    public class CHealth : PoolableComponent
    {
        public int MaximumHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public bool IsAlive
        {
            get { return this.CurrentHealth > 0; }
        }

        public bool HasBeenHit      
        {
            get { return this.CurrentHealth < this.MaximumHealth; }
        }

        public bool TakeDamage(float damage)
        {
            this.CurrentHealth -= damage;
            if (this.CurrentHealth <= 0)
            {
                this.CurrentHealth = 0;
                return true;
            }

            return false;
        }

        public void Initialize(int health)
        {
            this.MaximumHealth = health;
            this.CurrentHealth = health;
        }

        protected internal override void Cleanup()
        {
            this.MaximumHealth = 0;
            this.CurrentHealth = 0;
        }
    }
}
