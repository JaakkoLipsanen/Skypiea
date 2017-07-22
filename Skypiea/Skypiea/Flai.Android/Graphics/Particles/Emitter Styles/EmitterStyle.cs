using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.EmitterStyles
{
    public abstract class EmitterStyle
    {
        public abstract void GenerateOffsetAndForce(out Vector2 offset, out Vector2 initialForce);
    }
}
