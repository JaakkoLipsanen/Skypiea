using Flai.CBES.Components;

namespace Skypiea.Misc
{
    public static class DropHelper
    {
        public static bool IsBlinking(CLifeTime lifeTime)
        {
            const float BlinkStartTime = 5;
            const float BlinkPeriod = 0.4f;
            const float BlinkTime = 0.1f;
            return lifeTime.TimeRemaining < BlinkStartTime && lifeTime.TimeRemaining % BlinkPeriod < BlinkTime;
        }
    }
}
