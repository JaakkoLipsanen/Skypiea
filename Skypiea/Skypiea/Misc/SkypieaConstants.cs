using Flai;
using Skypiea.Components;

namespace Skypiea.Misc
{
    public static class SkypieaConstants
    {
        public const int MapWidth = 58; // in tiles
        public const int MapHeight = 30; // in tiles

        public const float ScreenSizeBias = 96;

        public static RectangleF GetAdjustedCameraArea(CCamera2D camera)
        {
            return camera.GetArea(FlaiGame.Current.ScreenSize).AsInflated(ScreenSizeBias, ScreenSizeBias);
        }
    }
}
