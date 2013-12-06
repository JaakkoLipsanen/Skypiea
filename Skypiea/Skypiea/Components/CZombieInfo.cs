using Flai;
using Flai.CBES;
using Skypiea.Model;

namespace Skypiea.Components
{
    public class CZombieInfo : PoolableComponent
    {
        public ZombieType Type { get; private set; }
        public float Size { get; private set; }

        public RectangleF AreaRectangle
        {
            get { return RectangleF.CreateCentered(this.Entity.Transform.Position, this.Size); }
        }

        public Circle AreaCircle
        {
            get { return new Circle(this.Entity.Transform.Position, this.Size * 0.5f); }
        }

        public void Initialize(ZombieType type, float size)
        {
            this.Type = type;
            this.Size = size;
        }

        protected override void Cleanup()
        {
            this.Type = (ZombieType)(-1);
            this.Size = 0;
        }
    }
}
