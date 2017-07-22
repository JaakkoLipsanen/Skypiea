using Flai.General;

namespace Flai.CBES.Systems
{
    internal static class EntitySystemHelper
    {
        public static int CompareOrder(EntitySystem left, EntitySystem right)
        {
            return left.ProcessOrder.CompareTo(right.ProcessOrder);
        }
    }

    internal static class EntitySystem<T>
        where T : EntitySystem
    {
        public static readonly TypeMask<EntitySystem> Bit = TypeMask<EntitySystem>.GetBit<T>(); // dunno if will even use
    }
}
