
using Microsoft.Xna.Framework;

namespace Flai
{
    public static class Screen
    {
        public static Size Size
        {
            get { return FlaiGame.Current.ScreenSize; }
        }

        public static int Width
        {
            get { return Screen.Size.Width; }
        }

        public static int Height
        {
            get { return Screen.Size.Height; }
        }

        public static float AspectRation
        {
            get { return Screen.Size.AspectRatio; }
        }

        public static Rectangle Area
        {
            get { return FlaiGame.Current.ScreenArea; }
        }

        public static Size ViewportSize
        {
            get { return FlaiGame.Current.ViewportSize; }
        }

        public static Rectangle ViewportArea
        {
            get { return FlaiGame.Current.ViewportArea; }
        }

        public static void ChangeResolution(Size resolution)
        {
            FlaiGame.Current.ChangeResolution(resolution);
        }
    }
}
