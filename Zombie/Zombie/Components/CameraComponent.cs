using Flai;
using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;

namespace Zombie.Components
{
    // TODO: Abstract "CameraComponent" and then inheriting "FollowingCameraComponent" etc?
    public class CameraComponent : Component, ICamera2D
    {
        // if null, should the getter create a new camera and return it?
        public static CameraComponent Active { get; set; }

        public Vector2 Position
        {
            get { return this.Parent.Get<TransformComponent>().Position; } // ..
        }

        public virtual float Rotation { get; set; }
        public float Zoom { get; set; }

        public CameraComponent()
        {
            // cause why not
            if (CameraComponent.Active == null)
            {
                CameraComponent.Active = this;
            }

            this.Zoom = 1f;
        }

        public Matrix GetTransform(Size screenSize)
        {
            return Camera2D.CalculateTransform(screenSize, this.Position, this.Zoom, this.Rotation);
        }

        public RectangleF GetArea(Size screenSize)
        {
            return Camera2D.CalculateArea(screenSize, this.Position, this.Zoom, this.Rotation);
        }

        public Vector2 ScreenToWorld(Size screenSize, Vector2 v)
        {
            return Camera2D.CalculateScreenToWorld(screenSize, this.Position, this.Zoom, v);
        }
    }
}
