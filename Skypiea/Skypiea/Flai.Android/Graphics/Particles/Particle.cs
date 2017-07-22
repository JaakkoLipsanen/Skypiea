using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles
{
    public struct Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector3 Color; // use Vector3 for precision/ease of use
        public float Opacity;
        public float Rotation;
        public float Scale;
        public float Age; // [0, 1]
        public float Noise; // [0, 1]: DO-NOT-CHANGE except from ParticleEmitter.Trigger!!

        internal float Inception; // Time of Spawn. Time == Emitters own time
    }
}
