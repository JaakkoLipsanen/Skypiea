
namespace Flai.General
{
    public static class BitHelper
    {
        public static ulong MostSignificantOne(ulong x)
        {
            x |= x >> 32;
            x |= x >> 16;
            x |= x >> 8;
            x |= x >> 4;
            x |= x >> 2;
            x |= x >> 1;

            return (x >> 1) + 1;
        }

        public static ulong LeastSignificantOne(ulong x)
        {
            return x & ~x;
        }

        public static int LogarithmPowerOfTwo(ulong x)
        {
            /* todo can be faster: http://graphics.stanford.edu/~seander/bithacks.html#IntegerLogObvious */

            int r = 0;
            while ((x >>= 1) != 0)
            {
                r++;
            }

            return r;
        }

        private static readonly ulong[] _logarithmMagicNumbers = { 0xAAAAAAAA, 0xCCCCCCCC, 0xF0F0F0F0, 0xFF00FF00, 0xFFFF0000, 0xFFFFFFFF00000000 };
        private static readonly int[] _logTable = new int[256];

        static BitHelper()
        {
            for (int i = 2; i < 256; i++)
            {
                _logTable[i] = 1 + _logTable[i / 2];
            }
        }

        public static int LogarithmPowerOfTwoFast(ulong x)
        {
            ulong v;
            if ((v = x >> 56) != 0)
            {
                return 56 + _logTable[v];
            }
            else if ((v = x >> 48) != 0)
            {
                return 48 + _logTable[v];
            }
            else if ((v = x >> 40) != 0)
            {
                return 40 + _logTable[v];
            }
            else if ((v = x >> 32) != 0)
            {
                return 32 + _logTable[v];
            }
            else if ((v = x >> 24) != 0)
            {
                return 24 + _logTable[v];
            }
            else if ((v = x >> 16) != 0)
            {
                return 16 + _logTable[v];
            }
            else if ((v = x >> 8) != 0)
            {
                return 8 + _logTable[v];
            }
            else
            {
                return _logTable[x];
            }

            /* todo can be faster: http://graphics.stanford.edu/~seander/bithacks.html#IntegerLogObvious */

        /*    int r = ((x & _logarithmMagicNumbers[0]) != 0) ? 1 : 0;
            for (int i = 5; i > 0; i--) // unroll for speed...
            {
                r |= (((x & _logarithmMagicNumbers[i]) != 0) ? 1 : 0) << i;
            }

            return r; */
        }
    }
}
