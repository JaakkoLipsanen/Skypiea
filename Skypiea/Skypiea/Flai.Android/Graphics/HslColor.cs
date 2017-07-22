using System;
using Microsoft.Xna.Framework;

namespace Flai.Graphics
{
    public struct HslColor
    {
        public float H;
        public float S;
        public float L;

        public HslColor(float h, float s, float l)
        {
            if (h < 0 || s < 0 || l < 0 || s > 1 || l > 1)
            {
                throw new ArgumentOutOfRangeException("");
            }

            if (h > 360)
            {
                h = h % 360;
            }

            this.H = h;
            this.S = s;
            this.L = l;
        }

        public static HslColor FromRgb(Color color)
        {
            float R = color.R / 255f;
            float G = color.G / 255f;
            float B = color.B / 255f;

            float max = FlaiMath.Max(R, G, B);
            float min = FlaiMath.Min(R, G, B);

            float C = max - min;
            float tempH;
            if (C == 0)
            {
                tempH = 0;
            }
            else if (max == R)
            {
                tempH = FlaiMath.RealModulus((G - B) / C, 6);
            }
            else if (max == G)
            {
                tempH = ((B - R) / C) + 2;
            }
            else
            {
                tempH = ((R - G) / C) + 4;
            }

            float H = 60 * tempH;
            float L = (max + min) / 2;

            float S = (C == 0) ? 0 : (1 - Math.Abs(2 * L - 1));

            return new HslColor(H, S, L);
        }

        public Color ToRgb()
        {
            float tempH = this.H / 60f;
            float C = (1 - Math.Abs(2 * L - 1)) * this.S;

            float X = C * (1 - Math.Abs(FlaiMath.RealModulus(tempH, 2) - 1));
            float m = this.L - C / 2;
            switch ((int)tempH)
            {
                case 0:
                    return new Color(C + m, X + m, m);
                case 1:
                    return new Color(X + m, C + m, m);
                case 2:
                    return new Color(m, C + m, X + m);
                case 3:
                    return new Color(m, X + m, C + m);
                case 4:
                    return new Color(X + m, m, C + m);
                case 5:
                    return new Color(C + m, m, X + m);

                default:
                    throw new ArgumentOutOfRangeException("Hue is out of range");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.H, this.S, this.L);
        }
    }
}
