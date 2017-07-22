#if CBES_3D
using DefaultTransformComponent = Flai.CBES.Components.CTransform3D;
#else
using DefaultTransformComponent = Flai.CBES.Components.CTransform2D;
#endif

using System;

namespace Flai.CBES
{
    internal static class CBESPreprocessorHelper
    {
        public static readonly Type DefaultTransformComponentType = typeof(DefaultTransformComponent);
    }
}
