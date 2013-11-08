using Flai;
using Flai.CBES;
using Microsoft.Xna.Framework;

namespace Zombie.Components
{
    public class TransformComponent : Component
    {
        public Vector2 Position;
        public float Rotation;

        public Vector2 RotationAsVector
        {
            get { return FlaiMath.GetAngleVector(this.Rotation); }
            set { this.Rotation = FlaiMath.GetAngle(Vector2.Normalize(value)); }
        }

        public TransformComponent()
        {
        }

        public TransformComponent(Vector2 position)
        {
            this.Position = position;
        }

        public TransformComponent(Vector2 position, float rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }

        public TransformComponent(TransformComponent copy)
        {
            this.Position = copy.Position;
            this.Rotation = copy.Rotation;
        }

        public void LookAt(TransformComponent transform)
        {
            this.LookAt(transform.Position);
        }

        public void LookAt(Vector2 position)
        {
            this.RotationAsVector = position - this.Position;
        }
    }
}
