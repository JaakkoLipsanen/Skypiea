using Flai;
using Skypiea.Components;

namespace Skypiea.Misc
{
    public static class SkypieaConstants
    {
        public const float ScreenSizeBias = 96;

        public static RectangleF GetAdjustedCameraArea(CCamera2D camera)
        {
            return camera.GetArea(FlaiGame.Current.ScreenSize).AsInflated(ScreenSizeBias, ScreenSizeBias);
        }
    }
}
