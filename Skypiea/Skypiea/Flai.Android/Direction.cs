
using System;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Flai
{
    public enum HorizontalDirection
    {
        Left = -1,
        Right = 1,
    }

    public enum VerticalDirection 
    {
        Up = -1, // Up == -1 is a bit questionable, but in XNA (2D) the coordinates go so that the upper you go, lower the Y-value is.. so yup
        Down = 1,
    }

    public enum Direction2D // : byte // maybe I shouldn't use byte's after all.. int's are a bit more efficient and the memory difference
    {                                 // in level files or in runtime is so small
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    public enum Direction3D
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
        Forward = 4,
        Backward = 5,
    }

    public static class DirectionExtensions
    {
        #region Direction2D Extensions

        public static Vector2i ToUnitVector(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.Right:
                    return Vector2i.UnitX;

                case Direction2D.Left:
                    return -Vector2i.UnitX;

                case Direction2D.Down:
                    return Vector2i.UnitY;

                case Direction2D.Up:
                    return -Vector2i.UnitY;

                default:
                    throw new ArgumentException("Value \"" + direction + "\" not recognized");
            }
        }

        public static Direction2D Inverse(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.Right:
                    return Direction2D.Left;
                case Direction2D.Left:
                    return Direction2D.Right;
                case Direction2D.Up:
                    return Direction2D.Down;
                case Direction2D.Down:
                    return Direction2D.Up;

                default:
                    throw new ArgumentException("Value \"" + direction + "\" not recognized");
            }
        }

        public static Direction2D RotateRight(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.Right:
                    return Direction2D.Down;
                case Direction2D.Down:
                    return Direction2D.Left;
                case Direction2D.Left:
                    return Direction2D.Up;
                case Direction2D.Up:
                    return Direction2D.Right;

                default:
                    throw new ArgumentException("Value \"" + direction + "\" not recognized");
            }
        }

        public static Direction2D RotateLeft(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.Right:
                    return Direction2D.Up;
                case Direction2D.Up:
                    return Direction2D.Left;
                case Direction2D.Left:
                    return Direction2D.Down;
                case Direction2D.Down:
                    return Direction2D.Right;

                default:
                    throw new ArgumentException("Value \"" + direction + "\" not recognized");
            }
        }

        public static int ToDegrees(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.Up:
                    return 270;

                case Direction2D.Right:
                    return 0;

                case Direction2D.Down:
                    return 90;

                case Direction2D.Left:
                    return 180;

                default:
                    throw new ArgumentException("Direction is invalid");
            }
        }

        public static float ToRadians(this Direction2D direction)
        {
            return MathHelper.ToRadians(direction.ToDegrees());
        }

        public static Alignment ToAlignment(this Direction2D direction)
        {
            if (direction == Direction2D.Left || direction == Direction2D.Right)
            {
                return Alignment.Horizontal;
            }

            return Alignment.Vertical;
        }

        public static int Sign(this Direction2D direction)
        {
            if (direction == Direction2D.Left || direction == Direction2D.Up)
            {
                return -1;
            }

            return 1;
        }

        public static Direction2D Opposite(this Direction2D direction)
        {
            switch (direction)
            {
                case Direction2D.Left:
                    return Direction2D.Right;

                case Direction2D.Right:
                    return Direction2D.Left;

                case Direction2D.Up:
                    return Direction2D.Down;

                case Direction2D.Down:
                    return Direction2D.Up;

                default:
                    throw new ArgumentException("Invalid direction!");
            }
        }

        #endregion

        #region Direction3D Extensions

        public static Vector3i ToVector3i(this Direction3D direction)
        {
            switch (direction)
            {
                case Direction3D.Right:
                    return Vector3i.UnitX;

                case Direction3D.Left:
                    return -Vector3i.UnitX;

                case Direction3D.Up:
                    return Vector3i.UnitY;

                case Direction3D.Down:
                    return -Vector3i.UnitY;

                case Direction3D.Forward:
                    return Vector3i.UnitZ;

                case Direction3D.Backward:
                    return -Vector3i.UnitZ;

                default:
                    throw new ArgumentException("Value \"" + direction + "\" not recognized");
            }
        }

        public static Direction3D Inverse(this Direction3D direction)
        {
            switch (direction)
            {
                case Direction3D.Right:
                    return Direction3D.Left;
                case Direction3D.Left:
                    return Direction3D.Right;

                case Direction3D.Up:
                    return Direction3D.Down;
                case Direction3D.Down:
                    return Direction3D.Up;

                case Direction3D.Forward:
                    return Direction3D.Backward;
                case Direction3D.Backward:
                    return Direction3D.Forward;

                default:
                    throw new ArgumentException("Value \"" + direction + "\" not recognized");
            }
        }

        #endregion
    }
}