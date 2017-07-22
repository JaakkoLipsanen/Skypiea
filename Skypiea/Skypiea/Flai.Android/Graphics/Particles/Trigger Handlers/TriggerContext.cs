using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.TriggerHandlers
{
    public struct TriggerContext
    {
        public int ReleaseAmount;
        public Vector2 Position;
        public float Rotation;

        public bool Canceled;
    }
}
