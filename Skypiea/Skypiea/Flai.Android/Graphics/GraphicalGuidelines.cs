
using Flai.DataStructures;

namespace Flai.Graphics
{
    // nothing *has to* follow these settings, but from now on all common things will follow these (if I remember this class when implementing said things :P).
    // not the best solution by any means since it's a bit difficult to use for non-debug etc stuff but blahh
    public static class GraphicalGuidelines
    {
        private const int DefaultDecimalPrecision = 3;
        private static readonly ValueStack<int> _decimalPrecisionStack = new ValueStack<int>(GraphicalGuidelines.DefaultDecimalPrecision);

        public static IValueStack<int> DecimalPrecisionInText
        {
            get { return _decimalPrecisionStack; }
        }
    }
}
