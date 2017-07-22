using Microsoft.Xna.Framework;
using System;

namespace Flai
{
    public interface ITransform2D
    {
        Vector2 Position { get; }
        float Rotation { get; }
    }

    // not sure about this..
    public interface IModifiableTransform2D : ITransform2D
    {
        // not sure about these "new".. both ITransform2D and IModifiableTransform2D have get; now... should IModifiableTransform2D have only set; ?
        new Vector2 Position { get; set; }
        new float Rotation { get; set; }
    }

    public static class Transform
    {
        public static ITransform2D CreateTransform2D(Vector2 position)
        {
            return new ITransform2DDirect(position, 0);
        }

        public static ITransform2D CreateTransform2D(Vector2 position, float rotation)
        {
            return new ITransform2DDirect(position, rotation);
        }

        public static ITransform2D CreateTransform2D(Func<Vector2> positionFunc) 
        {
            return new ITransform2DFunc(positionFunc, () => 0f);
        }

        public static ITransform2D CreateTransform2D(Func<Vector2> positionFunc, Func<float> rotationFunc)
        {
            return new ITransform2DFunc(positionFunc, rotationFunc);
        }

        #region ITransform2D Implemenatations

        private class ITransform2DDirect : ITransform2D
        {
            public Vector2 Position { get; private set; }
            public float Rotation { get; private set; }

            public ITransform2DDirect(Vector2 position, float rotation)
            {
                Ensure.IsValid(position);
                Ensure.IsValid(rotation);

                this.Position = position;
                this.Rotation = rotation;
            }
        }

        private class ITransform2DFunc : ITransform2D
        {
            private readonly Func<Vector2> _positionFunc;
            private readonly Func<float> _rotationFunc;

            public Vector2 Position
            {
                get { return _positionFunc(); }
            }

            public float Rotation
            {
                get { return _rotationFunc(); }
            }

            public ITransform2DFunc(Func<Vector2> positionFunc, Func<float> rotationFunc)
            {
                Ensure.NotNull(positionFunc);
                Ensure.NotNull(rotationFunc);

                _positionFunc = positionFunc;
                _rotationFunc = rotationFunc;
            }
        }

        #endregion
    }

    public static class TransformExtensions
    {
        public static Vector2 GetRotationVector(this ITransform2D transform)
        {
            return FlaiMath.GetAngleVector(transform.Rotation);
        }
    }
}
