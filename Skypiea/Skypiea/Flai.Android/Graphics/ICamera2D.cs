using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public interface ICamera2D
    {
        Vector2 Position { get; }
        float Rotation { get; }
        float Zoom { get; }

        Matrix GetTransform(Size viewportSize);
        RectangleF GetArea(Size screenSize);
        Vector2 ScreenToWorld(Size screenSize, Vector2 v);
    }

    public static class Camera2DExtensions
    {
        public static Matrix GetTransform(this ICamera2D camera)
        {
            return camera.GetTransform(FlaiGame.Current.ScreenSize);
        }
    }
}
