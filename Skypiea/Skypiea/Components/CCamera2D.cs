using Flai;
using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;

namespace Skypiea.Components
{
    // TODO: Abstract "CameraComponent" and then inheriting "FollowingCameraComponent" etc?
    public class CCamera2D : Component, ICamera2D // todo: to Poolable? Also abstract this and move to Flai.CBES.Components?
    {
        // if null, should the getter create a new camera and return it?
        public static CCamera2D Active { get; set; }

        // Field? Also, was before Parent.Transform.Position. But it's not wise to hardcode that.. One way to 
        // to do it "clean", would be if Camera would be it's own entity which was attached as "child" to the Player Entity.
        // so todo: implement entity child/parent relationship toi Flai.CBES?
        public Vector2 Position { get; set; } 
        public float Rotation { get; set; }
        public float Zoom { get; set; }

        public CCamera2D()
        {
            // cause why not
            if (CCamera2D.Active == null)
            {
                CCamera2D.Active = this;
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

    public class CPlayerCamera2D : CCamera2D
    {
        protected override void Initialize()
        {
            this.Position = this.Entity.Transform.Position;
        }

        protected override void PreUpdate(UpdateContext updateContext)
        {
            this.Position = Vector2.Lerp(this.Position, this.Entity.Transform.Position, updateContext.DeltaSeconds * 5f);
        }
    }
}
