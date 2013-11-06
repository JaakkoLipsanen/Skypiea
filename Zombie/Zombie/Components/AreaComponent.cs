using Flai;
using Flai.CBES;

namespace Zombie.Components
{
    // okay awful name...
    public class AreaComponent : Component
    {
        private readonly SizeF _size;
        public RectangleF Area
        {
            get
            {
                TransformComponent transform = this.Parent.Get<TransformComponent>();
                return new RectangleF(transform.Position.X - _size.Width / 2f, transform.Position.Y - _size.Height / 2f, _size.Width, _size.Height);
            }
        }

        public AreaComponent(SizeF size)
        {
            _size = size;
        }
    }
}
