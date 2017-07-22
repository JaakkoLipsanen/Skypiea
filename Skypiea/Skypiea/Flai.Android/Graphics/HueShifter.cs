using Microsoft.Xna.Framework;

namespace Flai.Graphics
{
    // From Mercury particle engine. pretty damn cool
    // more: http://en.wikipedia.org/wiki/YIQ
    public static class HueShifter
    {
        /// <summary>
        /// Transforms RGB colours in YIQ space.
        /// </summary>
        private static Matrix YiqTransform = new Matrix(
            0.299f, 0.587f, 0.114f, 0.000f,
            0.596f, -.274f, -.321f, 0.000f,
            0.211f, -.523f, 0.311f, 0.000f,
            0.000f, 0.000f, 0.000f, 1.000f);

        /// <summary>
        /// Transforms YIQ colours in RGB space.
        /// </summary>
        private static Matrix RgbTransform = Matrix.Invert(HueShifter.YiqTransform);

        // could be cached
        private static Matrix HueTransformMatrix = new Matrix(
            1f, 0f, 0f, 0f,
            0f, -1, -1, 0f,
            0f, -1, -1, 0f,
            0f, 0f, 0f, 1f);

        public static Matrix RgbToYiqMatrix
        {
            get { return HueShifter.YiqTransform; }
        }

        public static Matrix YiqToRgbMatrix
        {
            get { return HueShifter.RgbTransform; }
        }

        public static Color Shift(Color color, float hueShift)
        {
            Vector3 colorVector = color.ToVector3();
            Matrix hueShiftMatrix = HueShifter.CreateHueTransformationMatrix(hueShift);
            HueShifter.Shift(ref colorVector, ref hueShiftMatrix);

            return new Color(colorVector);
        }

        public static Vector3 Shift(Vector3 color, float hueShift)
        {
            Matrix hueShiftMatrix = HueShifter.CreateHueTransformationMatrix(hueShift);
            HueShifter.Shift(ref color, ref hueShiftMatrix);
            
            return color;
        }

        // high performance. get hueTransformationMatrix by calling HueShifter.CreateHueTransformationMatrix
        public static void Shift(ref Vector3 color, ref Matrix hueTransformationMatrix)
        {
            // convert color to YIQ color space
            Vector3.Transform(ref color, ref HueShifter.YiqTransform, out color);

            // transform the color (hue) in YIQ color space
            Vector3.Transform(ref color, ref hueTransformationMatrix, out color);

            // transform the color back to RGB color space
            Vector3.Transform(ref color, ref HueShifter.RgbTransform, out color);
        }

        public static Matrix CreateHueTransformationMatrix(float hueChange)
        {
            float u = FlaiMath.Cos(hueChange);
            float w = FlaiMath.Sin(hueChange);

            return new Matrix( // this could be cached
                1f, 0f, 0f, 0f,
                0f, u, -w, 0f,
                0f, w, u, 0f,
                0f, 0f, 0f, 1f);
        }
    }
}
