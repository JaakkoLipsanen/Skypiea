namespace Flai.CBES.Components
{
    public class CRotater2D : PoolableComponent
    {
        public float RotationAmount { get; private set; }
        public CRotater2D()
        {
        }

        public CRotater2D(float rotationAmount)
        {
            this.Initialize(rotationAmount);
        }

        public void Initialize(float rotationAmount)
        {
            this.RotationAmount = rotationAmount;
        }

        protected internal override void PostUpdate(UpdateContext updateContext)
        {
            this.Transform.Rotation += this.RotationAmount * updateContext.DeltaSeconds;
        }

        protected internal override void Cleanup()
        {
            this.RotationAmount = 0;
        }
    }
}
