using Flai;
using Flai.CBES.Components;

namespace Skypiea.Misc
{
    public static class SkypieaConstants
    {
        public const int PixelsPerMeter = 32;
        public const int MapWidth = 56; // in tiles // was 58, change *possibly* broke something
        public const int MapHeight = 30; // in tiles
        public const int MapWidthInPixels = SkypieaConstants.MapWidth * SkypieaConstants.PixelsPerMeter;
        public const int MapHeightInPixels = SkypieaConstants.MapHeight * SkypieaConstants.PixelsPerMeter;
        public static readonly Vector2i MapSizeInPixels = new Vector2i(SkypieaConstants.MapWidthInPixels, SkypieaConstants.MapHeightInPixels);
        public static readonly RectangleF MapAreaInPixels = new RectangleF(0, 0, SkypieaConstants.MapWidthInPixels, SkypieaConstants.MapHeightInPixels);

        public const float ScreenSizeBias = 96;

        public static RectangleF GetAdjustedCameraArea(CCamera2D camera)
        {
            return camera.GetArea(FlaiGame.Current.ScreenSize).AsInflated(ScreenSizeBias, ScreenSizeBias);
        }
    }
}
