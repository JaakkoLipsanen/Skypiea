using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.EmitterStyles
{
    public class CircleEmitter : EmitterStyle
    {
        public float Radius { get; set; }
        public bool IsShell { get; set; }
        public bool Radiate { get; set; }

        public CircleEmitter(float radius)
            : this(radius, false, false)
        {
        }

        public CircleEmitter(float radius, bool isShell, bool radiate)
        {
            this.Radius = radius;
            this.IsShell = isShell;
            this.Radiate = radiate;
        }

        public override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 initialForce)
        {
            Vector2 unitDirection = FlaiAlgorithms.GenerateRandomUnitVector2();
            offset = unitDirection * this.Radius;
            if (!this.IsShell)
            {
                offset *= Global.Random.NextFloat(0, 1);
            }

            initialForce = this.Radiate ? unitDirection : FlaiAlgorithms.GenerateRandomUnitVector2();
        }
    }
}
