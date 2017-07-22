
namespace Flai.General
{
    public static class TypeID<T>
    {
        private static uint NextID = 0;
        public static uint GetID<Y>()
            where Y : T
        {
            return TypeIDHelperInner<Y>.ID;
        }

        private static class TypeIDHelperInner<Y>
            where Y : T
        {
            public static readonly uint ID = TypeID<T>.NextID++;
        }
    }
}
