using Flai.CBES;
using Microsoft.Xna.Framework;

namespace Skypiea.Components
{
    // todo: CVelocity2D and move to Flai.CBES.Components?
    public class CVelocity : PoolableComponent
    {
        private Vector2 _direction = Vector2.Zero;

        public float Speed { get; set; }
        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = Vector2.Normalize(value); }
        }

        public void Initialize(Vector2 direction, float speed)
        {
            this.Direction = direction;
            this.Speed = speed;
        }

        protected override void Cleanup()
        {
            _direction = Vector2.Zero;
            this.Speed = 0f;
        }
    }
}
