using System;
namespace Skypiea.Misc
{
    public static class EntityTags
    {
        // kinda hacky to write but very fast and const syntax coloring \o/
        public const uint Player = 0;
        public const uint VirtualThumbStick = 1;
        public const uint MovementThumbStick = 2;
        public const uint RotationThumbStick = 3;
        public const uint Zombie = 4;
        public const uint Drop = 5;
        public const uint ZombieExplosionParticle = 6;
    }

    public static class EntityNames
    {
        public const string Player = "Player";
        public const string MovementThumbStick = "MovementThumbStick";
        public const string RotationThumbStick = "RotationThumbStick";
    }
}
