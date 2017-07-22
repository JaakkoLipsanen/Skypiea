using System.Text;

namespace Flai
{
    public static class Global
    {
        private static readonly StringBuilder _stringBuilder = new StringBuilder(32);
        private static readonly FlaiRandom _random = new FlaiRandom();

        public static FlaiRandom Random
        {
            get { return _random; }
        }

        public static StringBuilder StringBuilder
        {
            get
            {
                _stringBuilder.Clear();
                return _stringBuilder;
            }
        }
    }
}
