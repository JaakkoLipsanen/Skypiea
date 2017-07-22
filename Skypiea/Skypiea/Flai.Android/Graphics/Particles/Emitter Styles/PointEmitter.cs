using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.EmitterStyles
{
    public class PointEmitter : EmitterStyle
    {
        public bool InitialForceEnabled { get; set; }

        public PointEmitter()
            : this(true)
        {    
        }

        public PointEmitter(bool initialForceEnabled) 
        {
            this.InitialForceEnabled = initialForceEnabled;
        }

        public override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 initialForce)
        {
            offset = Vector2.Zero;
            initialForce = this.InitialForceEnabled ? FlaiAlgorithms.GenerateRandomUnitVector2() : Vector2.Zero;
        }
    }

    public class RotationalPointEmitter : PointEmitter
    {
        public Range RotationalRange { get; set; }
        public RotationalPointEmitter()
        {          
        }

        public RotationalPointEmitter(bool initialForceEnabled)
            : base(initialForceEnabled)
        {
        }

        public RotationalPointEmitter(bool initialForceEnabled, Range rotationalRange)
            : base(initialForceEnabled)
        {
            Ensure.IsValid(rotationalRange);
            this.RotationalRange = rotationalRange;
        }

        public override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 initialForce)
        {
            offset = Vector2.Zero;
            initialForce = this.InitialForceEnabled ? FlaiMath.GetAngleVector(Global.Random.NextFloat(this.RotationalRange)) : Vector2.Zero;
        }
    }
}
