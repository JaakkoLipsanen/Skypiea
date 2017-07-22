
namespace Flai.General
{
    public struct StructTuple<T1>
    {
        public readonly T1 Item1;

        public StructTuple(T1 item1)
        {
            this.Item1 = item1;
        }

        public override int GetHashCode()
        {
            return this.Item1.GetHashCode();
        }
    }

    public struct StructTuple<T1, T2>
    {
        public readonly T1 Item1;
        public readonly T2 Item2;

        public StructTuple(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public override int GetHashCode()
        {
            return (23 * this.Item1.GetHashCode()) ^ this.Item2.GetHashCode();
        }
    }
}
