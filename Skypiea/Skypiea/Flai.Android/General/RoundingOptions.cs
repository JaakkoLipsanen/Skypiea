using System;

namespace Flai.General
{
    public enum RoundingOptions
    {
        Default,
        Floor,
        Ceiling,
        Round,
    }

    public static class RoundingOptionsExtensions
    {
        public static float Round(this RoundingOptions roundingOptions, float value)
        {
            switch (roundingOptions)
            {
                case RoundingOptions.Default:
                    return (int)value;

                case RoundingOptions.Floor:
                    return FlaiMath.Floor(value);

                case RoundingOptions.Ceiling:
                    return FlaiMath.Ceiling(value);

                case RoundingOptions.Round:
                    return FlaiMath.Round(value);

                default:
                    throw new ArgumentException("roundingOptions");
            }
        }
    }
}
