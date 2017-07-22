using System;
using System.Globalization;
using System.IO;
using Flai.Diagnostics;
using Flai.Graphics;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection;
using System.Collections.Generic;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace Flai //.Extensions // use just base "Flai" namespace, it's easier for clients
{
    public static class XnaExtensions
    {
        #region GameServiceContainer Extensions

        /// <summary>
        /// Adds service to the service container
        /// </summary>
        public static void AddService<T>(this GameServiceContainer services, T provider)
        {
            services.AddService(typeof(T), provider);
        }

        /// <summary>
        /// Gets service from the service container
        /// </summary>
        public static T GetService<T>(this GameServiceContainer services)
        {
            return (T)services.GetService(typeof(T));
        }

        public static T Get<T>(this GameComponentCollection components)
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].GetType() == typeof (T))
                {
                    return (T)components[i];
                }
            }

            throw new ArgumentException("T");
        }

        #endregion

        #region Vector2 Extensions

        public static Vector2i ToVector2i(this Vector2 vector)
        {
            return new Vector2i((int)vector.X, (int)vector.Y);
        }

        public static Vector2 Rotate(this Vector2 point, float radians)
        {
            float cosRadians = FlaiMath.Cos(radians);
            float sinRadians = FlaiMath.Sin(radians);
            return new Vector2(
                point.X * cosRadians - point.Y * sinRadians,
                point.X * sinRadians + point.Y * cosRadians);
        }

        public static Vector2 Rotate(this Vector2 point, float radians, Vector2 origin)
        {
            float cosRadians = (float)Math.Cos(radians);
            float sinRadians = (float)Math.Sin(radians);

            Vector2 translatedPoint = new Vector2
            {
                X = point.X - origin.X,
                Y = point.Y - origin.Y,
            };

            return new Vector2
            {
                X = translatedPoint.X * cosRadians - translatedPoint.Y * sinRadians + origin.X,
                Y = translatedPoint.X * sinRadians + translatedPoint.Y * cosRadians + origin.Y,
            };
        }

        public static float GetAxis(this Vector2 point, Alignment axis)
        {
            return axis == Alignment.Horizontal ? point.X : point.Y;
        }

        public static Vector2 NormalizeOrZero(this Vector2 value)
        {
            // FlaiMath.NormalizeOrZero
            value.Normalize();
            if (!Check.IsValid(value))
            {
                return Vector2.Zero;
            }

            return value;
        }

        #endregion

        #region Point Extensions

        /// <summary>
        /// Converts Point to Vector
        /// </summary>
        public static Vector2 ToVector(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        #endregion

        #region Rectangle Extensions

        // Meh
        public static Rectangle Multiply(this Rectangle rectangle, float amount)
        {
            return rectangle.Multiply(new Vector2(amount));
        }

        public static Rectangle Multiply(this Rectangle rectangle, Vector2 amount)
        {
            return new Rectangle(
                (int)Math.Round(rectangle.Center.X - rectangle.Width / 2f * amount.X),
                (int)Math.Round(rectangle.Center.Y - rectangle.Height / 2f * amount.Y),
                (int)Math.Max(Math.Round(rectangle.Width * amount.X), 8),
                (int)Math.Max(Math.Round(rectangle.Height * amount.Y), 8));
        }

        public static bool Contains(this Rectangle rectangle, Vector2 position)
        {
            return
                rectangle.X <= position.X && position.X < rectangle.X + rectangle.Width &&
                rectangle.Y <= position.Y && position.Y < rectangle.Y + rectangle.Height;
        }

        public static Vector2 Center(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Center.X, rectangle.Center.Y);
        }

        public static Rectangle Offset(this Rectangle rectangle, Vector2i offset)
        {
            return new Rectangle(rectangle.X + offset.X, rectangle.Y + offset.Y, rectangle.Width, rectangle.Height);
        }

        public static Vector2 GetIntersectionDepth(this Rectangle rectangleA, Rectangle rectangleB)
        {
            // Calculate half sizes.
            float halfWidthA = rectangleA.Width / 2.0f;
            float halfHeightA = rectangleA.Height / 2.0f;
            float halfWidthB = rectangleB.Width / 2.0f;
            float halfHeightB = rectangleB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectangleA.Left + halfWidthA, rectangleA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectangleB.Left + halfWidthB, rectangleB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
            {
                return Vector2.Zero;
            }

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        public static float GetHorizontalIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;

            // Calculate centers.
            float centerA = rectA.Left + halfWidthA;
            float centerB = rectB.Left + halfWidthB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA - centerB;
            float minDistanceX = halfWidthA + halfWidthB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX)
            {
                return 0f;
            }

            // Calculate and return intersection depths.
            return distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
        }

        public static float GetVerticalIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfHeightA = rectA.Height / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            float centerA = rectA.Top + halfHeightA;
            float centerB = rectB.Top + halfHeightB;

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceY = centerA - centerB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceY) >= minDistanceY)
            {
                return 0f;
            }

            // Calculate and return intersection depths.
            return distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
        }

        #endregion

        #region Texture Extensions

        /// <summary>
        /// Returns center point of the texture
        /// </summary>
        /// <param name="texture">Texture which center will be returned</param>
        public static Vector2 Center(this Texture2D texture)
        {
            return new Vector2(texture.Width / 2f, texture.Height / 2f);
        }

        public static T[] GetData<T>(this Texture2D texture)
            where T : struct
        {
            T[] data = new T[texture.Width * texture.Height];
            texture.GetData<T>(data);

            return data;
        }

        public static T[] GetData<T>(this Texture2D texture, int startIndex, int elementCount)
            where T : struct
        {
            T[] data = new T[texture.Width * texture.Height];
            texture.GetData<T>(data, startIndex, elementCount);

            return data;
        }

        public static T[] GetData<T>(this Texture2D texture, Rectangle? area)
            where T : struct
        {
            T[] data = new T[texture.Width * texture.Height];
            texture.GetData<T>(0, area, data, 0, data.Length);

            return data;
        }

        public static T[] GetData<T>(this Texture2D texture, Rectangle? area, int startIndex, int elementCount)
            where T : struct
        {
            T[] data = new T[texture.Width * texture.Height];
            texture.GetData<T>(0, area, data, startIndex, elementCount);

            return data;
        }

        public static T[] GetData<T>(this Texture2D texture, int level, Rectangle? area, int startIndex, int elementCount)
            where T : struct
        {
            T[] data = new T[texture.Width * texture.Height];
            texture.GetData<T>(level, area, data, startIndex, elementCount);

            return data;
        }

        public static T[] GetPartialData<T>(this Texture2D texture, Rectangle area)
            where T : struct
        {
            if (area.Left < 0 || area.Top < 0 || texture.Width < area.Right || texture.Height < area.Bottom)
            {
                throw new ArgumentOutOfRangeException("Area is out of range");
            }

            T[] partialData = new T[(area.Right - area.Left) * (area.Bottom - area.Top)];
            texture.GetData<T>(0, area, partialData, 0, partialData.Length);

            return partialData;
        }

        // Not sure if this is any faster than without supplying pixelData
        public static T[] GetPartialData<T>(this Texture2D texture, T[] pixelData, Rectangle area)
            where T : struct
        {
            /*
            T[] partialData = new T[(area.Right - area.Left) * (area.Bottom - area.Top)];
            texture.GetData<T>(0, area, pixelData, 0, partialData.Count);

            return partialData; */

            if (area.Left < 0 || area.Top < 0 || texture.Width < area.Right || texture.Height < area.Bottom)
            {
                throw new ArgumentOutOfRangeException("area");
            }

            int width = area.Right - area.Left;
            int height = area.Bottom - area.Top;

            T[] partialData = new T[width * height];
            for (int y = area.Top; y < area.Bottom; y++)
            {
                int cachedPartialDataY = (y - area.Top) * width;
                int cachedTextureDataY = y * texture.Width;
                for (int x = area.Left; x < area.Right; x++)
                {
                    partialData[(x - area.Left) + cachedPartialDataY] = pixelData[x + cachedTextureDataY];
                }
            }

            return partialData;
        }

        public static Texture2D GetPartialTexture(this Texture2D texture, Rectangle area)
        {
            if (area.Left < 0 || area.Top < 0 || texture.Width < area.Right || texture.Height < area.Bottom)
            {
                throw new ArgumentOutOfRangeException("area");
            }

            Texture2D partialTexture = new Texture2D(texture.GraphicsDevice, area.Width, area.Height);
            partialTexture.SetData(texture.GetPartialData<Color>(area));

            return partialTexture;
        }

        public static Texture2D GetPartialTexture(this Texture2D texture, Color[] pixelData, Rectangle area)
        {
            if (area.Left < 0 || area.Top < 0 || texture.Width < area.Right || texture.Height < area.Bottom)
            {
                throw new ArgumentOutOfRangeException("area");
            }

            Texture2D partialTexture = new Texture2D(texture.GraphicsDevice, area.Width, area.Height);
            partialTexture.SetData(texture.GetPartialData<Color>(pixelData, area));

            return partialTexture;
        }

        public static void SaveAsJpeg(this Texture2D texture, string path)
        {
            string directoryPath = Path.GetDirectoryName(Path.GetFullPath(path));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (Stream stream = File.Open(path, FileMode.Create))
            {
                texture.SaveAsJpeg(stream, texture.Width, texture.Height);
            }
        }

        public static void SaveAsPng(this Texture2D texture, string path)
        {
            string directoryPath = Path.GetDirectoryName(Path.GetFullPath(path));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (Stream stream = File.Open(path, FileMode.Create))
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
            }
        }

        public static Texture2D DeepClone(this Texture2D texture)
        {
            Texture2D copy = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height, texture.LevelCount > 1, texture.Format);
            int[] data = texture.GetData<int>();
            copy.SetData<int>(data);

            return copy;
        }

        #endregion

        #region GraphicsDevice Extensions

        public static Size GetScreenSize(this GraphicsDevice graphicsDevice)
        {
#if WP8_MONOGAME
            //return new Size(
            //    FlaiMath.Max(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight), 
            //    FlaiMath.Min(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight));
#endif

            return new Size(graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public static Rectangle GetScreenArea(this GraphicsDevice graphicsDevice)
        {
            return new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);
        }

        public static Size GetDisplaySize(this GraphicsDevice graphicsDevice)
        {
            return new Size(graphicsDevice.DisplayMode.Width, graphicsDevice.DisplayMode.Height);
        }

        public static Rectangle GetDisplayArea(this GraphicsDevice graphicsDevice)
        {
            return new Rectangle(0, 0, graphicsDevice.DisplayMode.Width, graphicsDevice.DisplayMode.Height);
        }

        public static Size GetViewportSize(this GraphicsDevice graphicsDevice)
        {
            return new Size(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        public static Rectangle GetViewportArea(this GraphicsDevice graphicsDevice)
        {
            return new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        #endregion

        #region SpriteFont Extensions

        public static Vector2 AdjustCorner<T>(this SpriteFont font, T value, Corner corner, Vector2 position)
            where T : IConvertible
        {
            Vector2 size = font.MeasureType(value);
            if (corner == Corner.BottomLeft || corner == Corner.BottomRight)
            {
                position.Y -= size.Y;
            }

            if (corner == Corner.TopRight || corner == Corner.BottomRight)
            {
                position.X -= size.X;
            }

            return position;
        }

        /// <summary>
        /// Returns center of the string when it is drawn using the font
        /// </summary>
        /// <param name="spriteFont">Font which with the string is drawn</param>
        /// <param name="text">String which's center is measured</param>
        public static Vector2 Center(this SpriteFont spriteFont, string text)
        {
            return spriteFont.MeasureString(text) / 2f;
        }

        public static Vector2 Center(this SpriteFont spriteFont, char character)
        {
            return spriteFont.MeasureString(character) / 2f;
        }

        public static Vector2 GetMaximumCharacterSize(this SpriteFont font)
        {
            Vector2 maxSize = Vector2.Zero;
            foreach (char c in font.Characters)
            {
                Vector2 size = font.MeasureString(Common.CharacterToString(c));
                if (maxSize.X < size.X)
                {
                    maxSize.X = size.X;
                }

                if (maxSize.Y < size.Y)
                {
                    maxSize.Y = size.Y;
                }
            }

            return maxSize;
        }

        public static float GetCharacterHeight(this SpriteFont spriteFont)
        {
            return spriteFont.GetStringHeight("I");
        }

        public static float GetCharacterHeight(this SpriteFont spriteFont, char c)
        {
            return spriteFont.MeasureString(Common.CharacterToString(c)).Y;
        }

        public static float GetStringHeight(this SpriteFont spriteFont, string str)
        {
            return spriteFont.MeasureString(str).Y;
        }

        public static float GetCharacterWidth(this SpriteFont spriteFont)
        {
            return spriteFont.MeasureString("M").X;
        }

        public static float GetCharacterWidth(this SpriteFont spriteFont, char c)
        {
            return spriteFont.MeasureString(Common.CharacterToString(c)).X;
        }

        public static float GetStringWidth(this SpriteFont spriteFont, string str)
        {
            return spriteFont.MeasureString(str).X;
        }

        private static bool _spriteFontStuffInitialized = false;
        private static FieldInfo _textureFieldInfo;
        private static FieldInfo _glyphDataFieldInfo;
        private static FieldInfo _croppingDataFieldInfo;
        private static FieldInfo _kerningDataFieldInfo;
        private static MethodInfo _characterIndexMethodInfo;
        private static object[] _spriteFontCharacterArray;

        private static void InitializeSpriteFontStuff()
        {
            if (_spriteFontStuffInitialized)
            {
                return;
            }

            _textureFieldInfo = typeof(SpriteFont).GetField("textureValue", BindingFlags.NonPublic | BindingFlags.Instance);
            _glyphDataFieldInfo = typeof(SpriteFont).GetField("glyphData", BindingFlags.NonPublic | BindingFlags.Instance);
            _croppingDataFieldInfo = typeof(SpriteFont).GetField("croppingData", BindingFlags.NonPublic | BindingFlags.Instance);
            _kerningDataFieldInfo = typeof(SpriteFont).GetField("kerning", BindingFlags.NonPublic | BindingFlags.Instance);
            _characterIndexMethodInfo = typeof(SpriteFont).GetMethod("GetIndexForCharacter", BindingFlags.NonPublic | BindingFlags.Instance);
            _spriteFontCharacterArray = new object[1];
        }

        public static Texture2D GetTexture(this SpriteFont spriteFont)
        {
            if (!_spriteFontStuffInitialized)
            {
                XnaExtensions.InitializeSpriteFontStuff();
            }

            return _textureFieldInfo.GetValue(spriteFont) as Texture2D;
        }

        public static Rectangle GetCharacterSourceRectangle(this SpriteFont spriteFont, char c)
        {
            if (!_spriteFontStuffInitialized)
            {
                XnaExtensions.InitializeSpriteFontStuff();
            }

            List<Rectangle> glyphDataList = _glyphDataFieldInfo.GetValue(spriteFont) as List<Rectangle>;
            _spriteFontCharacterArray[0] = c;
            int i2 = (int)_characterIndexMethodInfo.Invoke(spriteFont, _spriteFontCharacterArray);
            return glyphDataList[(int)_characterIndexMethodInfo.Invoke(spriteFont, _spriteFontCharacterArray)];
        }

        public static Rectangle GetCharacterCroppingData(this SpriteFont spriteFont, char c)
        {
            if (!_spriteFontStuffInitialized)
            {
                XnaExtensions.InitializeSpriteFontStuff();
            }

            List<Rectangle> croppingDataList = _croppingDataFieldInfo.GetValue(spriteFont) as List<Rectangle>;
            _spriteFontCharacterArray[0] = c;
            return croppingDataList[(int)_characterIndexMethodInfo.Invoke(spriteFont, _spriteFontCharacterArray)];
        }

        public static Vector2 GetCharacterKerningData(this SpriteFont spriteFont, char c)
        {
            if (!_spriteFontStuffInitialized)
            {
                XnaExtensions.InitializeSpriteFontStuff();
            }

            List<Vector3> kerningList = _kerningDataFieldInfo.GetValue(spriteFont) as List<Vector3>;
            _spriteFontCharacterArray[0] = c;
            Vector3 kerning = kerningList[(int)_characterIndexMethodInfo.Invoke(spriteFont, _spriteFontCharacterArray)];

            return new Vector2(kerning.X, kerning.Y);
        }

        public static Vector2 MeasureString<T>(this SpriteFont spriteFont, T value)
            where T : IConvertible
        {
            if (spriteFont == null || value == null)
            {
                throw new ArgumentNullException("");
            }

            return XnaExtensions.MeasureType<T>(spriteFont, value);
        }

        private static Vector2 MeasureType<T>(this SpriteFont spriteFont, T value)
            where T : IConvertible
        {
            Type type = typeof(T);
            if (type == typeof(int))
            {
                return spriteFont.MeasureInt32(value.ToInt32(null));
            }
            else if (type == typeof(long))
            {
                return spriteFont.MeasureInt64(value.ToInt64(null));
            }
            else if (type == typeof(float))
            {
                return spriteFont.MeasureFloat(value.ToSingle(null));
            }
            else if (type == typeof(double))
            {
                return spriteFont.MeasureDouble(value.ToDouble(null));
            }
            else if (type == typeof(bool))
            {
                return spriteFont.MeasureBoolean(value.ToBoolean(null));
            }
            else if (type == typeof(char))
            {
                return spriteFont.MeasureString(Common.CharacterToString(value.ToChar(null)));
            }
            else
            {
                return spriteFont.MeasureString(value.ToString());
            }
        }

        private static Vector2 MeasureInt32(this SpriteFont spriteFont, int value)
        {
            Vector2 size = Vector2.Zero;
            if (value < 0)
            {
                size = spriteFont.MeasureString("-");
                value = -value;
            }

            do
            {
                int modulus = value % 10;
                value = value / 10;

                Vector2 tempSize = spriteFont.MeasureString(digits[modulus]);
                size.X += tempSize.X;
                size.Y = Math.Max(size.Y, tempSize.Y);
            }
            while (value > 0);

            return size;
        }

        private static Vector2 MeasureInt64(this SpriteFont spriteFont, long value)
        {
            Vector2 size = Vector2.Zero;
            if (value < 0)
            {
                size = spriteFont.MeasureString("-");
                value = -value;
            }

            do
            {
                long modulus = value % 10;
                value = value / 10;

                Vector2 tempSize = spriteFont.MeasureString(digits[modulus]);
                size.X += tempSize.X;
                size.Y = Math.Max(size.Y, tempSize.Y);
            }
            while (value > 0);

            return size;
        }

        private static Vector2 MeasureFloat(this SpriteFont spriteFont, float value)
        {
            Vector2 size = Vector2.Zero;
            if (value < 0)
            {
                size = spriteFont.MeasureString("-");
                value = -value;
            }

            // Draw all digits on the left side of the comma
            int intValue = (int)value;
            do
            {
                int modulus = intValue % 10;
                intValue = intValue / 10;

                Vector2 tempSize = spriteFont.MeasureString(digits[modulus]);
                size.X += tempSize.X;
                size.Y = Math.Max(size.Y, tempSize.Y);
            }
            while (intValue > 0);

            value = value % 1;
            int decimalIndex = 0;
            if (value != 0)
            {
                Vector2 tempSize = spriteFont.MeasureString(",");
                size.X += tempSize.X;
                size.Y = Math.Max(tempSize.Y, size.Y);

                while (value != 0 && decimalIndex < GraphicalGuidelines.DecimalPrecisionInText.CurrentValue)
                {
                    value *= 10;
                    int modulus = (int)value;
                    tempSize = spriteFont.MeasureString(digits[modulus]);

                    size.X += tempSize.X;
                    size.Y = Math.Max(tempSize.Y, size.Y);

                    value = value % 1;
                    decimalIndex++;
                }
            }
            return Vector2.Zero;
        }

        private static Vector2 MeasureDouble(this SpriteFont spriteFont, double value)
        {
            Vector2 size = Vector2.Zero;
            if (value < 0)
            {
                size = spriteFont.MeasureString("-");
                value = -value;
            }

            // Draw all digits on the left side of the comma
            int intValue = (int)value;
            do
            {
                int modulus = intValue % 10;
                intValue = intValue / 10;

                Vector2 tempSize = spriteFont.MeasureString(digits[modulus]);
                size.X += tempSize.X;
                size.Y = Math.Max(size.Y, tempSize.Y);
            }
            while (intValue > 0);

            value = value % 1;
            int decimalIndex = 0;
            if (value != 0)
            {
                Vector2 tempSize = spriteFont.MeasureString(",");
                size.X += tempSize.X;
                size.Y = Math.Max(tempSize.Y, size.Y);

                while (value != 0 && decimalIndex < GraphicalGuidelines.DecimalPrecisionInText.CurrentValue)
                {
                    value *= 10;
                    int modulus = (int)value;
                    tempSize = spriteFont.MeasureString(digits[modulus]);

                    size.X += tempSize.X;
                    size.Y = Math.Max(tempSize.Y, size.Y);

                    value = value % 1;
                    decimalIndex++;
                }
            }
            return Vector2.Zero;
        }

        private static Vector2 MeasureBoolean(this SpriteFont spriteFont, bool value)
        {
            return value ? spriteFont.MeasureString("True") : spriteFont.MeasureString("False");
        }

        #endregion

        #region SpriteBatch Extensions

        private static string[] digits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private static string[] charBuffer = new string[10];
        private static float[] xposBuffer = new float[10];
        private static readonly string minValue = Int32.MinValue.ToString(CultureInfo.InvariantCulture);

        /// <summary>  
        /// Extension method for SpriteBatch that draws an integer without allocating  
        /// any memory. This function avoids garbage collections that are normally caused  
        /// by calling Int32.ToString or String.Format.  
        /// </summary>  
        /// <param name="spriteBatch">The SpriteBatch instance whose DrawString method will be invoked.</param>  
        /// <param name="spriteFont">The SpriteFont to draw the integer value with.</param>  
        /// <param name="value">The integer value to draw.</param>  
        /// <param name="position">The screen position specifying where to draw the value.</param>  
        /// <param name="color">The color of the text drawn.</param>  
        /// <returns>The next position on the line to draw text. This value uses position.Y and position.X plus the equivalent of calling spriteFont.MeasureString on value.ToString(CultureInfo.InvariantCulture).</returns>  
        private static Vector2 DrawInt32(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, int value, Vector2 position, Color color, float layerDepth)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");

            if (spriteFont == null)
                throw new ArgumentNullException("spriteFont");

            Vector2 nextPosition = position;
            if (value == Int32.MinValue)
            {
                nextPosition.X = nextPosition.X + spriteFont.MeasureString(minValue).X;
                spriteBatch.DrawString(spriteFont, minValue, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                position = nextPosition;
            }
            else
            {
                if (value < 0)
                {
                    nextPosition.X = nextPosition.X + spriteFont.MeasureString("-").X;
                    spriteBatch.DrawString(spriteFont, "-", position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                    value = -value;
                    position = nextPosition;
                }

                int index = 0;
                do
                {
                    int modulus = value % 10;
                    value = value / 10;

                    charBuffer[index] = digits[modulus];
                    xposBuffer[index] = spriteFont.MeasureString(digits[modulus]).X;
                    index += 1;
                }
                while (value > 0);

                for (int i = index - 1; i >= 0; --i)
                {
                    nextPosition.X = nextPosition.X + xposBuffer[i];
                    spriteBatch.DrawString(spriteFont, charBuffer[i], position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                    position = nextPosition;
                }
            }
            return position;
        }

        /// <summary>  
        /// Extension method for SpriteBatch that draws an integer without allocating  
        /// any memory. This function avoids garbage collections that are normally caused  
        /// by calling Int32.ToString or String.Format.  
        /// </summary>  
        /// <param name="spriteBatch">The SpriteBatch instance whose DrawString method will be invoked.</param>  
        /// <param name="spriteFont">The SpriteFont to draw the integer value with.</param>  
        /// <param name="value">The integer value to draw.</param>  
        /// <param name="position">The screen position specifying where to draw the value.</param>  
        /// <param name="color">The color of the text drawn.</param>  
        /// <returns>The next position on the line to draw text. This value uses position.Y and position.X plus the equivalent of calling spriteFont.MeasureString on value.ToString(CultureInfo.InvariantCulture).</returns>  
        private static Vector2 DrawInt64(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, long value, Vector2 position, Color color, float layerDepth)
        {
            if (spriteBatch == null)
                throw new ArgumentNullException("spriteBatch");

            if (spriteFont == null)
                throw new ArgumentNullException("spriteFont");

            Vector2 nextPosition = position;
            if (value == Int32.MinValue)
            {
                nextPosition.X = nextPosition.X + spriteFont.MeasureString(minValue).X;
                spriteBatch.DrawString(spriteFont, minValue, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                position = nextPosition;
            }
            else
            {
                if (value < 0)
                {
                    nextPosition.X = nextPosition.X + spriteFont.MeasureString("-").X;
                    spriteBatch.DrawString(spriteFont, "-", position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                    value = -value;
                    position = nextPosition;
                }

                int index = 0;
                do
                {
                    int modulus = (int)(value % 10);
                    value = value / 10;

                    charBuffer[index] = digits[modulus];
                    xposBuffer[index] = spriteFont.MeasureString(digits[modulus]).X;
                    index += 1;
                }
                while (value > 0);

                for (int i = index - 1; i >= 0; --i)
                {
                    nextPosition.X = nextPosition.X + xposBuffer[i];
                    spriteBatch.DrawString(spriteFont, charBuffer[i], position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                    position = nextPosition;
                }
            }
            return position;
        }

        /// <summary>  
        /// Extension method for SpriteBatch that draws an integer without allocating  
        /// any memory. This function avoids garbage collections that are normally caused  
        /// by calling Int32.ToString or String.Format.  
        /// </summary>  
        /// <param name="spriteBatch">The SpriteBatch instance whose DrawString method will be invoked.</param>  
        /// <param name="spriteFont">The SpriteFont to draw the integer value with.</param>  
        /// <param name="value">The integer value to draw.</param>  
        /// <param name="position">The screen position specifying where to draw the value.</param>  
        /// <param name="color">The color of the text drawn.</param>  
        /// <returns>The next position on the line to draw text. This value uses position.Y and position.X plus the equivalent of calling spriteFont.MeasureString on value.ToString(CultureInfo.InvariantCulture).</returns>  
        private static Vector2 DrawFloat(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, float value, Vector2 position, Color color, float layerDepth)
        {
            if (spriteBatch == null)
            {
                throw new ArgumentNullException("spriteBatch");
            }

            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }

            if (value < int.MinValue || value > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value");
            }


            Vector2 nextPosition = position;
            if (value < 0)
            {
                nextPosition.X = nextPosition.X + spriteFont.MeasureString("-").X;
                spriteBatch.DrawString(spriteFont, "-", position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                value = -value;
                position = nextPosition;
            }

            // Draw all digits on the left side of the comma
            int intValue = (int)value;
            int index = 0;
            do
            {
                int modulus = intValue % 10;
                intValue = intValue / 10;

                charBuffer[index] = digits[modulus];
                xposBuffer[index] = spriteFont.MeasureString(digits[modulus]).X;
                index += 1;
            }
            while (intValue > 0);

            for (int i = index - 1; i >= 0; --i)
            {
                nextPosition.X = nextPosition.X + xposBuffer[i];
                spriteBatch.DrawString(spriteFont, charBuffer[i], position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                position = nextPosition;
            }


            value = value % 1;
            int decimalIndex = 0;
            if (value != 0)
            {
                nextPosition.X = nextPosition.X + spriteFont.MeasureString(",").X;
                spriteBatch.DrawString(spriteFont, ",", position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                position = nextPosition;

                while (value != 0 && decimalIndex < GraphicalGuidelines.DecimalPrecisionInText.CurrentValue)
                {
                    value *= 10;
                    int modulus = (int)value;

                    nextPosition.X = nextPosition.X + spriteFont.MeasureString(digits[modulus]).X;
                    spriteBatch.DrawString(spriteFont, digits[modulus], position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                    position = nextPosition;

                    value = value % 1;

                    decimalIndex++;
                }
            }

            return position;
        }

        /// <summary>  
        /// Extension method for SpriteBatch that draws an integer without allocating  
        /// any memory. This function avoids garbage collections that are normally caused  
        /// by calling Int32.ToString or String.Format.  
        /// </summary>  
        /// <param name="spriteBatch">The SpriteBatch instance whose DrawString method will be invoked.</param>  
        /// <param name="spriteFont">The SpriteFont to draw the integer value with.</param>  
        /// <param name="value">The integer value to draw.</param>  
        /// <param name="position">The screen position specifying where to draw the value.</param>  
        /// <param name="color">The color of the text drawn.</param>  
        /// <returns>The next position on the line to draw text. This value uses position.Y and position.X plus the equivalent of calling spriteFont.MeasureString on value.ToString(CultureInfo.InvariantCulture).</returns>  
        private static Vector2 DrawDouble(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, double value, Vector2 position, Color color, float layerDepth)
        {
            if (spriteBatch == null)
            {
                throw new ArgumentNullException("spriteBatch");
            }

            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }

            if (value < int.MinValue || value > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            Vector2 nextPosition = position;
            if (value < 0)
            {
                nextPosition.X = nextPosition.X + spriteFont.MeasureString("-").X;
                spriteBatch.DrawString(spriteFont, "-", position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                value = -value;
                position = nextPosition;
            }

            // Draw all digits on the left side of the comma
            int intValue = (int)value;
            int index = 0;
            do
            {
                int modulus = intValue % 10;
                intValue = intValue / 10;

                charBuffer[index] = digits[modulus];
                xposBuffer[index] = spriteFont.MeasureString(digits[modulus]).X;
                index += 1;
            }
            while (intValue > 0);

            for (int i = index - 1; i >= 0; --i)
            {
                nextPosition.X = nextPosition.X + xposBuffer[i];
                spriteBatch.DrawString(spriteFont, charBuffer[i], position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                position = nextPosition;
            }


            value = value % 1;

            int decimalIndex = 0;
            if (value != 0)
            {
                nextPosition.X = nextPosition.X + spriteFont.MeasureString(",").X;
                spriteBatch.DrawString(spriteFont, ",", position, color);
                position = nextPosition;

                while (value != 0 && decimalIndex < GraphicalGuidelines.DecimalPrecisionInText.CurrentValue)
                {
                    value *= 10;
                    int modulus = (int)value;

                    nextPosition.X = nextPosition.X + spriteFont.MeasureString(digits[modulus]).X;
                    spriteBatch.DrawString(spriteFont, digits[modulus], position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
                    position = nextPosition;

                    value = value % 1;

                    decimalIndex++;
                }
            }

            return position;
        }

        private static Vector2 DrawBoolean(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, bool boolean, Vector2 position, Color color, float layerDepth)
        {
            if (spriteBatch == null)
            {
                throw new ArgumentNullException("spriteBatch");
            }

            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }

            string str = boolean ? "True" : "False";
            spriteBatch.DrawString(spriteFont, str, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);

            position.X = position.X + spriteFont.MeasureString(str).X;
            return position;
        }

        public static Vector2 DrawString<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Corner anchorCorner, Color color)
           where T : IConvertible
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            Vector2 size = spriteFont.MeasureType(value);
            if (anchorCorner == Corner.BottomLeft || anchorCorner == Corner.BottomRight)
            {
                position.Y -= size.Y;
            }

            if (anchorCorner == Corner.TopRight || anchorCorner == Corner.BottomRight)
            {
                position.X -= size.X;
            }

            return XnaExtensions.DrawType<T>(spriteBatch, spriteFont, value, position, color, 0f);
        }

        public static Vector2 DrawString<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color color, float layerDepth)
            where T : IConvertible
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            return XnaExtensions.DrawType<T>(spriteBatch, spriteFont, value, position, color, layerDepth);
        }

        public static Vector2 DrawString<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color color)
            where T : IConvertible
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            return XnaExtensions.DrawType<T>(spriteBatch, spriteFont, value, position, color);
        }

        public static Vector2 DrawString<T, T1>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, Vector2 position, Color color)
            where T : IConvertible
            where T1 : IConvertible
        {
            if (val1 == null || val2 == null)
            {
                throw new ArgumentNullException();
            }

            position = XnaExtensions.DrawType<T>(spriteBatch, spriteFont, val1, position, color);
            return XnaExtensions.DrawType<T1>(spriteBatch, spriteFont, val2, position, color);
        }

        public static Vector2 DrawString<T, T1>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, Vector2 position, Color color, float layerDepth)
            where T : IConvertible
            where T1 : IConvertible
        {
            if (val1 == null || val2 == null)
            {
                throw new ArgumentNullException();
            }

            position = XnaExtensions.DrawType<T>(spriteBatch, spriteFont, val1, position, color, layerDepth);
            return XnaExtensions.DrawType<T1>(spriteBatch, spriteFont, val2, position, color, layerDepth);
        }

        public static Vector2 DrawString<T, T1, T2>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, T2 val3, Vector2 position, Color color)
            where T : IConvertible
            where T1 : IConvertible
            where T2 : IConvertible
        {
            if (val1 == null || val2 == null)
            {
                throw new ArgumentNullException();
            }

            position = XnaExtensions.DrawType<T>(spriteBatch, spriteFont, val1, position, color);
            position = XnaExtensions.DrawType<T1>(spriteBatch, spriteFont, val2, position, color);
            return XnaExtensions.DrawType<T2>(spriteBatch, spriteFont, val3, position, color);
        }

        public static Vector2 DrawString<T, T1, T2>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, T2 val3, Vector2 position, Corner anchorCorner, Color color)
            where T : IConvertible
            where T1 : IConvertible
            where T2 : IConvertible
        {
            if (val1 == null || val2 == null)
            {
                throw new ArgumentNullException();
            }

            if (anchorCorner != Corner.TopLeft)
            {
                Vector2 val1Size = spriteFont.MeasureType(val1);
                Vector2 val2Size = spriteFont.MeasureType(val2);
                Vector2 val3Size = spriteFont.MeasureType(val3);
                if (anchorCorner == Corner.BottomLeft || anchorCorner == Corner.BottomRight)
                {
                    position.Y -= FlaiMath.Max(val1Size.Y, val2Size.Y, val3Size.Y);
                }

                if (anchorCorner == Corner.TopRight || anchorCorner == Corner.BottomRight)
                {
                    position.X -= val1Size.X + val2Size.X + val3Size.X;
                }
            }

            position = XnaExtensions.DrawType<T>(spriteBatch, spriteFont, val1, position, color);
            position = XnaExtensions.DrawType<T1>(spriteBatch, spriteFont, val2, position, color);
            return XnaExtensions.DrawType<T2>(spriteBatch, spriteFont, val3, position, color);
        }

        private static Vector2 DrawType<T>(FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color color)
            where T : IConvertible
        {
            return XnaExtensions.DrawType<T>(spriteBatch, spriteFont, value, position, color, 0f);
        }

        private static Vector2 DrawType<T>(FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color color, float layerDepth)
            where T : IConvertible
        {
            Type type = typeof(T);
            if (type == typeof(int))
            {
                return spriteBatch.DrawInt32(spriteFont, value.ToInt32(null), position, color, layerDepth);
            }
            else if (type == typeof(long))
            {
                return spriteBatch.DrawInt64(spriteFont, value.ToInt64(null), position, color, layerDepth);
            }
            else if (type == typeof(bool))
            {
                return spriteBatch.DrawBoolean(spriteFont, value.ToBoolean(null), position, color, layerDepth);
            }
            else if (type == typeof(float))
            {
                return spriteBatch.DrawFloat(spriteFont, value.ToSingle(null), position, color, layerDepth);
            }
            else if (type == typeof(double))
            {
                return spriteBatch.DrawDouble(spriteFont, value.ToDouble(null), position, color, layerDepth);
            }
            else
            {
                string str = value.ToString();
                spriteBatch.DrawString(spriteFont, str, position, color);
                position.X += spriteFont.MeasureString(str).X;
                return position;
            }
        }

        public static Vector2 DrawStringCentered<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color color)
            where T : IConvertible
        {
            return XnaExtensions.DrawTypeCentered<T>(spriteBatch, spriteFont, value, position, color, 0f);
        }

        public static Vector2 DrawStringCentered<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color color, float layerDepth)
            where T : IConvertible
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            return XnaExtensions.DrawTypeCentered<T>(spriteBatch, spriteFont, value, position, color, layerDepth);
        }

        public static Vector2 DrawStringCentered<T1, T2>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T1 value1, T2 value2, Vector2 position, Color color)
            where T1 : IConvertible
            where T2 : IConvertible
        {
            return spriteBatch.DrawStringCentered<T1, T2>(spriteFont, value1, value2, position, color, 1f);
        }

        public static Vector2 DrawStringCentered<T1, T2>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T1 value1, T2 value2, Vector2 position, Color color, float layerDepth)
            where T1 : IConvertible
            where T2 : IConvertible
        {
            if (value1 == null || value2 == null)
            {
                throw new ArgumentNullException();
            }

            Vector2 value1Size = spriteFont.MeasureString<T1>(value1);
            Vector2 value2Size = spriteFont.MeasureString<T2>(value2);

            Vector2 totalSize = new Vector2(value1Size.X + value2Size.X, Math.Max(value1Size.Y, value2Size.Y));
            position -= totalSize / 2f;
            position = XnaExtensions.DrawType<T1>(spriteBatch, spriteFont, value1, position, color, layerDepth);
            return XnaExtensions.DrawType<T2>(spriteBatch, spriteFont, value2, position, color, layerDepth);
        }

        public static Vector2 DrawStringCentered<T1, T2, T3>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T1 value1, T2 value2, T3 value3, Vector2 position, Color color)
            where T1 : IConvertible
            where T2 : IConvertible
            where T3 : IConvertible
        {
            return spriteBatch.DrawStringCentered<T1, T2, T3>(spriteFont, value1, value2, value3, position, color, 1f);
        }

        public static Vector2 DrawStringCentered<T1, T2, T3>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T1 value1, T2 value2, T3 value3, Vector2 position, Color color, float layerDepth)
            where T1 : IConvertible
            where T2 : IConvertible
            where T3 : IConvertible
        {
            if (value1 == null || value2 == null)
            {
                throw new ArgumentNullException();
            }

            Vector2 value1Size = spriteFont.MeasureString<T1>(value1);
            Vector2 value2Size = spriteFont.MeasureString<T2>(value2);
            Vector2 value3Size = spriteFont.MeasureString<T3>(value3);

            Vector2 totalSize = new Vector2(value1Size.X + value2Size.X + value3Size.X, FlaiMath.Max(value1Size.Y, value2Size.Y, value3Size.Y));
            position -= totalSize / 2f;

            position = XnaExtensions.DrawType<T1>(spriteBatch, spriteFont, value1, position, color, layerDepth);
            position = XnaExtensions.DrawType<T2>(spriteBatch, spriteFont, value2, position, color, layerDepth);
            return XnaExtensions.DrawType<T3>(spriteBatch, spriteFont, value3, position, color, layerDepth);
        }

        private static Vector2 DrawTypeCentered<T>(FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color color, float layerDepth)
            where T : IConvertible
        {
            Type type = typeof(T);
            if (type == typeof(int))
            {
                return spriteBatch.DrawInt32(spriteFont, value.ToInt32(null), position - spriteFont.MeasureInt32(value.ToInt32(null)) / 2f, color, layerDepth);
            }
            else if (type == typeof(long))
            {
                return spriteBatch.DrawInt64(spriteFont, value.ToInt64(null), position - spriteFont.MeasureInt64(value.ToInt64(null)) / 2f, color, layerDepth);
            }
            else if (type == typeof(bool))
            {
                return spriteBatch.DrawBoolean(spriteFont, value.ToBoolean(null), position - spriteFont.MeasureBoolean(value.ToBoolean(null)) / 2f, color, layerDepth);
            }
            else if (type == typeof(float))
            {
                return spriteBatch.DrawFloat(spriteFont, value.ToSingle(null), position - spriteFont.MeasureFloat(value.ToSingle(null)) / 2f, color, layerDepth);
            }
            else if (type == typeof(double))
            {
                return spriteBatch.DrawDouble(spriteFont, value.ToDouble(null), position - spriteFont.MeasureDouble(value.ToDouble(null)) / 2f, color, layerDepth);
            }
            else
            {
                string str = value.ToString();
                spriteBatch.DrawString(spriteFont, str, position, color, 0f, spriteFont.MeasureString(str) / 2f, 1f, SpriteEffects.None, layerDepth);
                position.X += spriteFont.MeasureString(str).X;
                return position;
            }
        }

        public static Vector2 DrawStringFaded<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position)
            where T : IConvertible
        {
            return XnaExtensions.DrawStringFaded<T>(spriteBatch, spriteFont, value, position, Color.Black, Color.White);
        }

        public static Vector2 DrawStringFaded<T, T1>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, Vector2 position)
            where T1 : IConvertible
            where T : IConvertible
        {
            return XnaExtensions.DrawStringFaded<T, T1>(spriteBatch, spriteFont, val1, val2, position, Color.Black, Color.White);
        }

        public static Vector2 DrawStringFaded<T, T1, T2>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, T2 val3, Vector2 position)
            where T1 : IConvertible
            where T2 : IConvertible
            where T : IConvertible
        {
            return XnaExtensions.DrawStringFaded<T, T1, T2>(spriteBatch, spriteFont, val1, val2, val3, position, Color.Black, Color.White);
        }

        public static Vector2 DrawStringFaded<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color backColor, Color frontColor)
            where T : IConvertible
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return XnaExtensions.DrawTypeFaded<T>(spriteBatch, spriteFont, value, position, backColor, frontColor);
        }

        public static Vector2 DrawStringFaded<T, T1>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, Vector2 position, Color backColor, Color frontColor)
            where T1 : IConvertible
            where T : IConvertible
        {
            if (val1 == null || val2 == null)
            {
                throw new ArgumentNullException();
            }

            position = XnaExtensions.DrawTypeFaded<T>(spriteBatch, spriteFont, val1, position, backColor, frontColor);
            return XnaExtensions.DrawTypeFaded<T1>(spriteBatch, spriteFont, val2, position, backColor, frontColor);
        }

        public static Vector2 DrawStringFaded<T, T1, T2>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, T2 val3, Vector2 position, Color backColor, Color frontColor)
            where T1 : IConvertible
            where T2 : IConvertible
            where T : IConvertible
        {
            if (val1 == null || val2 == null || val3 == null)
            {
                throw new ArgumentNullException();
            }

            position = XnaExtensions.DrawTypeFaded<T>(spriteBatch, spriteFont, val1, position, backColor, frontColor);
            position = XnaExtensions.DrawTypeFaded<T1>(spriteBatch, spriteFont, val2, position, backColor, frontColor);
            return XnaExtensions.DrawTypeFaded<T2>(spriteBatch, spriteFont, val3, position, backColor, frontColor);
        }

        private static Vector2 DrawTypeFaded<T>(FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color backColor, Color frontColor)
            where T : IConvertible
        {
            Vector2 nextPosition = XnaExtensions.DrawType<T>(spriteBatch, spriteFont, value, position - Vector2.One, backColor);
            XnaExtensions.DrawType<T>(spriteBatch, spriteFont, value, position, frontColor);

            return nextPosition;
        }

        // .....................


        public static Vector2 DrawStringFadedCentered<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position)
            where T : IConvertible
        {
            return XnaExtensions.DrawStringFaded<T>(spriteBatch, spriteFont, value, position, Color.Black, Color.White);
        }

        public static Vector2 DrawStringFadedCentered<T, T1>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, Vector2 position)
            where T1 : IConvertible
            where T : IConvertible
        {
            return XnaExtensions.DrawStringFaded<T, T1>(spriteBatch, spriteFont, val1, val2, position, Color.Black, Color.White);
        }

        public static Vector2 DrawStringFadedCentered<T, T1, T2>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, T2 val3, Vector2 position)
            where T1 : IConvertible
            where T2 : IConvertible
            where T : IConvertible
        {
            return XnaExtensions.DrawStringFaded<T, T1, T2>(spriteBatch, spriteFont, val1, val2, val3, position, Color.Black, Color.White);
        }

        public static Vector2 DrawStringFadedCentered<T>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T value, Vector2 position, Color backColor, Color frontColor)
            where T : IConvertible
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Vector2 originalPosition = position;
            position = XnaExtensions.DrawStringCentered(spriteBatch, spriteFont, value, position, backColor);
            XnaExtensions.DrawStringCentered(spriteBatch, spriteFont, value, originalPosition + Vector2.One, frontColor);

            return position;
        }

        public static Vector2 DrawStringFadedCentered<T, T1>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, Vector2 position, Color backColor, Color frontColor)
            where T1 : IConvertible
            where T : IConvertible
        {
            if (val1 == null || val2 == null)
            {
                throw new ArgumentNullException();
            }

            Vector2 originalPosition = position;
            position = XnaExtensions.DrawStringCentered(spriteBatch, spriteFont, val1, val2, position, backColor);
            XnaExtensions.DrawStringCentered(spriteBatch, spriteFont, val1, val2, originalPosition + Vector2.One, frontColor);

            return position;
        }

        public static Vector2 DrawStringFadedCentered<T, T1, T2>(this FlaiSpriteBatch spriteBatch, SpriteFont spriteFont, T val1, T1 val2, T2 val3, Vector2 position, Color backColor, Color frontColor)
            where T1 : IConvertible
            where T2 : IConvertible
            where T : IConvertible
        {
            if (val1 == null || val2 == null || val3 == null)
            {
                throw new ArgumentNullException();
            }

            Vector2 originalPosition = position;
            position = XnaExtensions.DrawStringCentered(spriteBatch, spriteFont, val1, val2, val3, position, backColor);
            XnaExtensions.DrawStringCentered(spriteBatch, spriteFont, val1, val2, val3, originalPosition + Vector2.One, frontColor);

            return position;
        }

        #endregion

        #region Viewport Extensions

        public static Vector2i GetViewportSize(this Viewport viewport)
        {
            return new Vector2i(viewport.Width, viewport.Height);
        }

        #endregion

        #region Color Extensions

        public static HsvColor ToHsv(this Color color)
        {
            return HsvColor.FromRgb(color);
        }

        public static HslColor ToHsl(this Color color)
        {
            return HslColor.FromRgb(color);
        }

        public static Color AddNoise(this Color color, int maxNoise)
        {
            color.R = (byte)FlaiMath.Clamp(color.R + Global.Random.Next(-maxNoise, maxNoise), 0, 255);
            color.G = (byte)FlaiMath.Clamp(color.G + Global.Random.Next(-maxNoise, maxNoise), 0, 255);
            color.B = (byte)FlaiMath.Clamp(color.B + Global.Random.Next(-maxNoise, maxNoise), 0, 255);

            return color;
        }

        public static Color AddMonochromaticNoise(this Color color, int maxNoise)
        {
            int noise = Global.Random.Next(-maxNoise, maxNoise);
            color.R = (byte)FlaiMath.Clamp(color.R + noise, 0, 255);
            color.G = (byte)FlaiMath.Clamp(color.G + noise, 0, 255);
            color.B = (byte)FlaiMath.Clamp(color.B + noise, 0, 255);

            return color;
        }

        public static Color AddMonochromaticNoise(this Color color, int maxNoise, float inputNoise)
        {
            int noise = (int)SimplexNoise.GetPseudoRandom1D(inputNoise, -maxNoise, maxNoise);
            color.R = (byte)FlaiMath.Clamp(color.R + noise, 0, 255);
            color.G = (byte)FlaiMath.Clamp(color.G + noise, 0, 255);
            color.B = (byte)FlaiMath.Clamp(color.B + noise, 0, 255);

            return color;
        }

        public static Color MultiplyRGB(this Color color, float multiplier)
        {
            return new Color(
                (byte)FlaiMath.Clamp(color.R * multiplier, 0, byte.MaxValue),
                (byte)FlaiMath.Clamp(color.G * multiplier, 0, byte.MaxValue),
                (byte)FlaiMath.Clamp(color.B * multiplier, 0, byte.MaxValue),
                color.A);
        }

        public static Color MultiplyRGB(this Color color, float redMultiplier, float greenMultiplier, float blueMultiplier)
        {
            return new Color(
                (byte)FlaiMath.Clamp(color.R * redMultiplier, 0, byte.MaxValue),
                (byte)FlaiMath.Clamp(color.G * greenMultiplier, 0, byte.MaxValue),
                (byte)FlaiMath.Clamp(color.B * blueMultiplier, 0, byte.MaxValue),
                color.A);
        }

        #endregion

        #region Effect Extensions

        public static void SetCurrentTechnique(this Effect effect, string newTechniqueName)
        {
            if (effect != null)
            {
                effect.CurrentTechnique = effect.Techniques[newTechniqueName];
            }
        }

        public static bool TrySetCurrentTechnique(this Effect effect, string newTechniqueName)
        {
            if (effect != null && !string.IsNullOrWhiteSpace(newTechniqueName))
            {
                EffectTechnique newTechnique = effect.Techniques[newTechniqueName];
                if (newTechnique != null)
                {
                    effect.CurrentTechnique = effect.Techniques[newTechniqueName];
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region MouseState Extensions

        public static Vector2i GetMousePosition(this MouseState mouseState)
        {
            return new Vector2i(mouseState.X, mouseState.Y);
        }

        #endregion

#if WINDOWS_PHONE
        public static bool ContainsFlag(this GestureType gestureType, GestureType other)
        {
            return (gestureType & other) == other;
        }
#endif
    }
}
