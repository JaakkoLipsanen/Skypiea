
namespace Zombie.Systems
{
    public static class SystemProcessOrder
    {
        public const int PreFrame = -50;

        public const int PreInput = -40;
        public const int Input = -30;
        public const int PostInput = -20;

        public const int PreUpdate = -10;
        public const int Update = 0;
        public const int PostUpdate = 10;

        public const int PreCollision = 20;
        public const int Collision = 30;
        public const int PostCollision = 40;

        public const int PostFrame = 50;
    }
}
