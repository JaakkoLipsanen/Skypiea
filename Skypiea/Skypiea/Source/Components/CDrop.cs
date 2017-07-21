using Flai.CBES;
using Skypiea.Model;

namespace Skypiea.Components
{
    public class CDrop : PoolableComponent
    {
        public DropType DropType { get; private set; }
        public void Initialize(DropType dropType)
        {
            this.DropType = dropType;
        }

        protected override void Cleanup()
        {
            this.DropType = (DropType)(-1);
        }
    }
}
