
namespace Flai
{
    public enum Sign
    {
        Positive = 1,
        Negative = -1,
        None = 0,
    }

    public static class SignExtensions
    {
        public static Sign Opposite(this Sign sign)
        {
            if (sign == Sign.Positive)
            {
                return Sign.Negative;
            }
            else if (sign == Sign.Negative)
            {
                return Sign.Positive;
            }

            return Sign.None;
        }

        public static int ToInt(this Sign sign)
        {
            return (int)sign;
        }
    }
}
