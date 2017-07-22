
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public class Camera2D : ICamera2D
    {
        private bool _dirty = true;

        /// <summary>
        /// Zoom level of the camera
        /// </summary>
        protected float _zoom;

        /// <summary>
        /// DisplayPosition of the camera
        /// </summary>
        protected Vector2 _position;

        /// <summary>
        /// Rotation of the camera
        /// </summary>
        protected float _rotation;

        /// <summary>
        /// Transform matrix of the camera
        /// </summary>
        protected Matrix _transform;
        protected Matrix _spriteBatchInternalMatrix;

        /// <summary>
        /// Visible area of camera
        /// </summary>
        protected RectangleF _area;

        private Size _previousViewportSize = Size.Invalid;

        public Camera2D()
            : this(Vector2.Zero)
        {
        }

        public Camera2D(Vector2 position)
        {
            this.Position = position;
            this.Zoom = 1f;
            this.Rotation = 0f;
        }

        /// <summary>
        /// Zoom level of the camera
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                float clampedValue = MathHelper.Clamp(value, 0.01f, 100f);
                if (_zoom != clampedValue)
                {
                    _zoom = clampedValue;
                    _dirty = true;
                }
            }
        }

        /// <summary>
        /// DisplayPosition of the camera
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    _dirty = true;
                }
            }
        }

        /// <summary>
        /// Rotation of the camera
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    _dirty = true;
                }
            }
        }

        /// <summary>
        /// Returns Transform matrix of the camera
        /// </summary>
        public Matrix GetTransform(GraphicsDevice graphicsDevice)
        {
            return this.GetTransform(graphicsDevice.GetViewportSize());
        }

        public Matrix GetTransform(Size viewportSize)
        {
            if (_dirty || viewportSize != _previousViewportSize)
            {
                this.RecalculateDirtyThings(viewportSize);
            }

            return _transform;
        }

        public Matrix GetTransformForWithoutSpriteBatch(GraphicsDevice graphicsDevice)
        {
            return this.GetTransformForWithoutSpriteBatch(graphicsDevice.GetViewportSize());
        }

        public Matrix GetTransformForWithoutSpriteBatch(Size viewportSize)
        {
            if (_dirty || viewportSize != _previousViewportSize)
            {
                this.RecalculateDirtyThings(viewportSize);
            }

            return _transform * _spriteBatchInternalMatrix;
        }

        protected virtual void RecalculateDirtyThings(Size screenSize)
        {
            // Transform matrix
            _transform = Camera2D.CalculateTransform(screenSize, _position, _zoom, _rotation);

            // SpriteBatch internal matrix
            // todo: meh, should this be calculated here..? it's so rare that I need this.
            if (screenSize != _previousViewportSize)
            {
                float pixelWidth = (screenSize.Width > 0) ? (1f / (float)screenSize.Width) : 0f;
                float pixelHeight = (screenSize.Height > 0) ? (-1f / (float)screenSize.Height) : 0f;
                _spriteBatchInternalMatrix = new Matrix
                {
                    M11 = pixelWidth * 2f,
                    M22 = pixelHeight * 2f,
                    M33 = 1f,
                    M44 = 1f,
                    M41 = -1f - pixelWidth,
                    M42 = 1f - pixelHeight,
                };
            }

            // Area
            _area = Camera2D.CalculateArea(screenSize, _position, _zoom, _rotation);

            _previousViewportSize = screenSize;
            _dirty = false;
        }

        public virtual RectangleF GetArea(GraphicsDevice graphicsDevice)
        {
            return this.GetArea(graphicsDevice.GetViewportSize());
        }

        public virtual RectangleF GetArea(Size screenSize)
        {
            if (_dirty || screenSize != _previousViewportSize)
            {
                this.RecalculateDirtyThings(screenSize);
            }

            return _area;
        }

        // Blaah but I'd say it's worth it
        public Vector2 ScreenToWorld(Vector2 v)
        {
            return this.ScreenToWorld(FlaiGame.Current.GraphicsDevice, v);
        }

        public Vector2 ScreenToWorld(GraphicsDevice graphicsDevice, Vector2 v)
        {
            return _position - new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height) / 2 / _zoom + v / _zoom;
        }

        public Vector2 ScreenToWorld(Size screenSize, Vector2 v)
        {
            return _position - screenSize.ToVector2i() / 2f / _zoom + v / _zoom;
        }

        // not sure if works
        public Vector2 WorldToScreen(Size screenSize, Vector2 v)
        {
            return v * _zoom - _position + screenSize.ToVector2i() / 2f;
        }

        public static Matrix CalculateTransform(Size screenSize, Vector2 position)
        {
            return Camera2D.CalculateTransform(screenSize, position, 1f, 0);
        }

        public static Matrix CalculateTransform(Size screenSize, Vector2 position, float zoom, float rotation)
        {
            return Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                 Matrix.CreateRotationZ(rotation) *
                 Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                 Matrix.CreateTranslation(new Vector3(screenSize.Width * 0.5f, screenSize.Height * 0.5f, 0));
        }

        public static RectangleF CalculateArea(Size screenSize, Vector2 position)
        {
            return Camera2D.CalculateArea(screenSize, position, 1, 0);
        }

        public static RectangleF CalculateArea(Size screenSize, Vector2 position, float zoom, float rotation)
        {
            RectangleF area = new RectangleF(position.X - screenSize.Width / 2f / zoom, position.Y - screenSize.Height / 2f / zoom, screenSize.Width / zoom, screenSize.Height / zoom);
            if (rotation == 0f)
            {
                return area;
            }

            Vector2 center = area.Center;

            float rotationSin = FlaiMath.Sin(rotation);
            float rotationCos = FlaiMath.Cos(rotation);

            Vector2 topLeft = new Vector2(
                (float)(center.X + (area.TopLeft.X - center.X) * rotationCos + (area.TopLeft.Y - center.Y) * rotationSin),
                (float)(center.Y - (area.TopLeft.X - center.X) * rotationSin + (area.TopLeft.Y - center.Y) * rotationCos));
            Vector2 topRight = new Vector2(
                (float)(center.X + (area.TopRight.X - center.X) * rotationCos + (area.TopRight.Y - center.Y) * rotationSin),
                (float)(center.Y - (area.TopRight.X - center.X) * rotationSin + (area.TopRight.Y - center.Y) * rotationCos));

            Vector2 bottomLeft = new Vector2(
                (float)(center.X + (area.BottomLeft.X - center.X) * rotationCos + (area.BottomLeft.Y - center.Y) * rotationSin),
                (float)(center.Y - (area.BottomLeft.X - center.X) * rotationSin + (area.BottomLeft.Y - center.Y) * rotationCos));

            Vector2 bottomRight = new Vector2(
                center.X + (area.BottomRight.X - center.X) * rotationCos + (area.BottomRight.Y - center.Y) * rotationSin,
                center.Y - (area.BottomRight.X - center.X) * rotationSin + (area.BottomRight.Y - center.Y) * rotationCos);

            float minX = FlaiMath.Min(topLeft.X, topRight.X, bottomLeft.X, bottomRight.X);
            float maxX = FlaiMath.Max(topLeft.X, topRight.X, bottomLeft.X, bottomRight.X);
            float minY = FlaiMath.Min(topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y);
            float maxY = FlaiMath.Max(topLeft.Y, topRight.Y, bottomLeft.Y, bottomRight.Y);

            return new RectangleF(minX, minY, (maxX - minX), (maxY - minY));
        }

        public static Vector2 CalculateScreenToWorld(Size screenSize, Vector2 position, float zoom, Vector2 point)
        {
            return position - screenSize.ToVector2i() / 2f / zoom + point / zoom;
        }

        // not sure if works
        public static Vector2 CalculateWorldToScreen(Size screenSize, Vector2 position, float zoom, Vector2 point)
        {
            return point * zoom - position + screenSize.ToVector2i() / 2f;
        }
    }
}
