using Flai.CBES;
using Microsoft.Xna.Framework;

namespace Zombie.Components
{
    public class VelocityComponent : Component
    {
        private Vector2 _direction = Vector2.Zero;
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = Vector2.Normalize(value); }
        }

        public float Speed { get; set; }

        public VelocityComponent(Vector2 direction, float speed)
        {
            this.Direction = direction;
            this.Speed = speed;
        }
    }
}
