using System;
namespace Zombie.Misc
{
    public static class EntityTags
    {
        // todo: there is no reason why Entity.Tag and these couldn't be int's. a lot faster with absolutely no difference
        public const string Player = "Player";
        public const string VirtualThumbStick = "VirtualThumbStick";
        public const string MovementThumbStick = "MovementThumbStick";
        public const string RotationThumbStick = "RotationThumbStick";
        public const string Zombie = "Zombie";
    }
}
