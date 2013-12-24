using Flai.CBES.Components;

namespace Skypiea.Misc
{
    public static class DropHelper
    {
        public static bool IsBlinking(CLifeTime lifeTime)
        {
            return lifeTime.TimeRemaining < 5 && lifeTime.TimeRemaining % 0.4f < 0.1f;
        }
    }
}
