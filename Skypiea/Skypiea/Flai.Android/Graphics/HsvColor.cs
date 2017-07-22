using System;
using Microsoft.Xna.Framework;

namespace Flai.Graphics
{
    public struct HsvColor
    {
        public float H;
        public float S;
        public float V;

        public HsvColor(float h, float s, float v)
        {
            if (h < 0 || s < 0 || v < 0 || s > 1 || v > 1)
            {
                throw new ArgumentOutOfRangeException("");
            }

            if (h > 360)
            {
                h = h % 360;
            }

            this.H = h;
            this.S = s;
            this.V = v;
        }

        public static HsvColor FromRgb(Color color)
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
            float V = max;

            float S = (C == 0) ? 0 : (C / V);

            return new HsvColor(H, S, V);
        }

        public Color ToRgb()
        {
            float tempH = this.H / 60f;
            float C = this.V * this.S;
            float X = C * (1 - Math.Abs(FlaiMath.RealModulus(tempH, 2) - 1));
            float m = V - C;
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
                    return Color.Transparent;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.H, this.S, this.V);
        }
    }
}
