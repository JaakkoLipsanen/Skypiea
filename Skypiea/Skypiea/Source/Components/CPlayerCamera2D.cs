using Flai;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Misc;
using Skypiea.View;

namespace Skypiea.Components
{
    public class CPlayerCamera2D : CCamera2D
    {
        const float CameraSlowdownStartDistance = SkypieaConstants.PixelsPerMeter * 3.5f;
        const float CameraSlowdownEndDistance = SkypieaConstants.PixelsPerMeter * 3;
        private static readonly RectangleF _mapAreaWithNormalCamera = new RectangleF(SkypieaConstants.MapAreaInPixels).Inflate(-CameraSlowdownStartDistance, -CameraSlowdownStartDistance); 

        private Vector2 _realPosition;
        protected internal override void Initialize()
        {
            this.Zoom = SkypieaViewConstants.RenderScale;
            this.Position = this.Entity.Transform.Position;
            _realPosition = this.Position;
        }

        protected internal override void PreUpdate(UpdateContext updateContext)
        {
            _realPosition = Vector2.Lerp(_realPosition, this.Entity.Transform.Position, updateContext.DeltaSeconds * 5f);

            Vector2 newPosition = _realPosition;
            if (!_mapAreaWithNormalCamera.Contains(newPosition))
            {
                // horizontal
                if (newPosition.X < CameraSlowdownStartDistance)
                {
                    newPosition.X = FlaiMath.Scale(FlaiMath.Max(0, newPosition.X), 0, CameraSlowdownStartDistance, CameraSlowdownEndDistance, CameraSlowdownStartDistance);
                }
                else if (newPosition.X > SkypieaConstants.MapWidthInPixels - CameraSlowdownStartDistance)
                {
                    newPosition.X = FlaiMath.Scale(
                        FlaiMath.Min(newPosition.X, SkypieaConstants.MapWidthInPixels), 
                        SkypieaConstants.MapWidthInPixels - CameraSlowdownStartDistance, SkypieaConstants.MapWidthInPixels, 
                        SkypieaConstants.MapWidthInPixels - CameraSlowdownStartDistance, SkypieaConstants.MapWidthInPixels - CameraSlowdownEndDistance);
                }

                // vertical
                if (newPosition.Y < CameraSlowdownStartDistance)
                {
                    newPosition.Y = FlaiMath.Scale(FlaiMath.Max(0, newPosition.Y), 0, CameraSlowdownStartDistance, CameraSlowdownEndDistance, CameraSlowdownStartDistance);
                }
                else if (newPosition.Y > SkypieaConstants.MapHeightInPixels - CameraSlowdownStartDistance)
                {
                    newPosition.Y = FlaiMath.Scale(
                        FlaiMath.Min(newPosition.Y, SkypieaConstants.MapHeightInPixels),
                        SkypieaConstants.MapHeightInPixels - CameraSlowdownStartDistance, SkypieaConstants.MapHeightInPixels,
                        SkypieaConstants.MapHeightInPixels - CameraSlowdownStartDistance, SkypieaConstants.MapHeightInPixels - CameraSlowdownEndDistance);
                }
            }

            this.Position = newPosition;
        }
    }
}
