namespace Flai
{
    public enum Alignment
    {
        Horizontal = 1,
        Vertical = 2,
    }

    public static class AlignmentExtensions
    {
        public static Vector2i ToUnitVector(this Alignment alignment)
        {
            return (alignment == Alignment.Horizontal) ? Vector2i.UnitX : Vector2i.UnitY;
        }

        public static Alignment Inverse(this Alignment alignment)
        {
            return (alignment == Alignment.Horizontal) ? Alignment.Vertical : Alignment.Horizontal;
        }
    }
}
