using Microsoft.Xna.Framework;

namespace Flai.CBES.Components
{
    // todo: CVelocity2D and move to Flai.CBES.Components?
    public class CVelocity2D : PoolableComponent
    {
        private Vector2 _direction = Vector2.Zero;

        public float Speed { get; set; }
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = Vector2.Normalize(value); }
        }

        public Vector2 Velocity
        {
            get { return _direction*this.Speed; }
        }

        public CVelocity2D()
        {
        }

        public CVelocity2D(Vector2 direction, float speed)
        {
            this.Initialize(direction, speed);
        }

        public void Initialize(Vector2 direction, float speed)
        {
            this.Direction = direction;
            this.Speed = speed;
        }

        protected internal override void Cleanup()
        {
            _direction = Vector2.Zero;
            this.Speed = 0f;
        }
    }
}
