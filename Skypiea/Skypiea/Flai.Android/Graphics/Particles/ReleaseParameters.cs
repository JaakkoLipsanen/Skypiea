
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Flai.Graphics.Particles
{
    public class ReleaseParameters
    {
        public RangeInt Quantity = (RangeInt)1;
        public Range Speed = 1;
        public ColorRange Color = XnaColor.White;
        public Range Opacity = 1;
        public Range Scale = 1;
        public Range Rotation = new Range(-FlaiMath.Pi, FlaiMath.Pi); // = "Random rotation"
    }
}
