using Flai;
using Flai.CBES;

namespace Skypiea.Components
{
    public class CArea : PoolableComponent
    {
        public SizeF Size { get; private set; }
        public RectangleF Area
        {
            get { return RectangleF.CreateCentered(this.Entity.Transform.Position, this.Size); }
        }

        public void Initialize(SizeF size)
        {
            this.Size = size;
        }

        protected override void Cleanup()
        {
            this.Size = SizeF.Empty;
        }
    }
}
