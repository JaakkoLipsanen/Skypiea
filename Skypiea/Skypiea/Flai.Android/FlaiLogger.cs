using System.Diagnostics;

namespace Flai
{
    public static class FlaiLogger
    {
        [Conditional("DEBUG")]
        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }

        [Conditional("DEBUG")]
        public static void Log(string format, params object[] parameters)
        {
            Debug.WriteLine(format, parameters);
        }
    }
}
