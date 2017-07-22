using Flai;
using Flai.CBES;
using Skypiea.Prefabs;

namespace Skypiea.Components
{
    public enum KillReason
    {
        Normal,
        Instakill, // killed with ZombieHelper.Kill (invunerability kill, passive "chance for zombie to explode on death" kill etc)
    }

    public class CZombieInfo : PoolableComponent
    {
        public ZombieType Type { get; private set; }
        public float Size { get; private set; }
        public KillReason? KillReason { get; private set; }

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
            this.KillReason = null;
        }

        protected internal override void Cleanup()
        {
            this.Type = (ZombieType)(-1);
            this.Size = 0;
            this.KillReason = null;
        }

        public void SetKillReason(KillReason killReason)
        {
            this.KillReason = killReason;
        }
    }
}
