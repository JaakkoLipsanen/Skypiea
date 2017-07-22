using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    // Pretty much the same as PrimitiveRenderer
    public class DebugRenderer
    {
        private const float DefaultLineThickness = 4f;
        private static readonly Color DefaultColor = Color.Red * 0.5f;
        private static DebugRenderer _instance;

        private readonly List<IDebugDrawable> _debugDrawables = new List<IDebugDrawable>();
        private Matrix _spriteBatchMatrix = Matrix.Identity;

        internal DebugRenderer()
        {
            _instance = this;
        }

        #region Draw Rectangle

        [Conditional("DEBUG")]
        public static void DrawRectangle(GraphicsContext graphicsContext, RectangleF area)
        {
            DebugRenderer.DrawRectangle(area, DebugRenderer.DefaultColor);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangle(GraphicsContext graphicsContext, Vector2 position, float sideSize)
        {
            if (sideSize < 0)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            Vector2 size = new Vector2(sideSize, sideSize);
            DebugRenderer.DrawRectangle(new RectangleF(position - size / 2f, position + size / 2f), DebugRenderer.DefaultColor);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangle(GraphicsContext graphicsContext, Vector2 position, float sideSize, Color color)
        {
            if (sideSize < 0)
            {
                throw new ArgumentOutOfRangeException("size");
            }

            Vector2 size = new Vector2(sideSize, sideSize);
            DebugRenderer.DrawRectangle(new RectangleF(position - size / 2f, position + size / 2f), color);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangle(GraphicsContext graphicsContext, Vector2 min, Vector2 max)
        {
            if (min.X > max.X || min.Y > max.Y)
            {
                throw new ArgumentOutOfRangeException("min max");
            }

            DebugRenderer.DrawRectangle(new RectangleF(min, max), DebugRenderer.DefaultColor);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangle(GraphicsContext graphicsContext, Vector2 min, Vector2 max, Color color)
        {
            if (min.X > max.X || min.Y > max.Y)
            {
                throw new ArgumentOutOfRangeException("min max");
            }

            DebugRenderer.DrawRectangle(new RectangleF(min, max), color);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangle(GraphicsContext graphicsContext, Rectangle area)
        {
            DebugRenderer.DrawRectangle(new RectangleF(area), DebugRenderer.DefaultColor);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangle(GraphicsContext graphicsContext, Rectangle area, Color color)
        {
            DebugRenderer.DrawRectangle(new RectangleF(area), color);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangle(RectangleF area, Color color)
        {
            _instance._debugDrawables.Add(new DebugRectangle { Rectangle = area, Color = color });
        }

        #endregion

        #region Draw Rectangles

        [Conditional("DEBUG")]
        public static void DrawRectangles(GraphicsContext graphicsContext, Rectangle area, params Rectangle[] otherAreas)
        {
            DebugRenderer.DrawRectangles(graphicsContext, DebugRenderer.DefaultColor, area, otherAreas);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangles(Color color, Rectangle area, params Rectangle[] otherAreas)
        {
            DebugRenderer.DrawRectangle(new RectangleF(area), color);
            foreach (Rectangle otherArea in otherAreas)
            {
                DebugRenderer.DrawRectangle(new RectangleF(otherArea), color);
            }
        }

        [Conditional("DEBUG")]
        public static void DrawRectangles(GraphicsContext graphicsContext, Rectangle area, IEnumerable<Rectangle> otherAreas)
        {
            DebugRenderer.DrawRectangles(graphicsContext, DebugRenderer.DefaultColor, area, otherAreas);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangles(GraphicsContext graphicsContext, Color color, Rectangle area, IEnumerable<Rectangle> otherAreas)
        {
            DebugRenderer.DrawRectangle(new RectangleF(area), color);
            foreach (Rectangle rectangle in otherAreas)
            {
                DebugRenderer.DrawRectangle(new RectangleF(rectangle), color);
            }
        }

        [Conditional("DEBUG")]
        public static void DrawRectangles(GraphicsContext graphicsContext, RectangleF area, params RectangleF[] otherAreas)
        {
            DebugRenderer.DrawRectangles(graphicsContext, DebugRenderer.DefaultColor, area, otherAreas);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangles(GraphicsContext graphicsContext, Color color, RectangleF area, params RectangleF[] otherAreas)
        {
            DebugRenderer.DrawRectangle(area, color);
            foreach (RectangleF otherRectangle in otherAreas)
            {
                DebugRenderer.DrawRectangle(otherRectangle, color);
            }
        }

        [Conditional("DEBUG")]
        public static void DrawRectangles(GraphicsContext graphicsContext, RectangleF area, IEnumerable<RectangleF> otherAreas)
        {
            DebugRenderer.DrawRectangles(graphicsContext, DebugRenderer.DefaultColor, area, otherAreas);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangles(GraphicsContext graphicsContext, Color color, RectangleF area, IEnumerable<RectangleF> otherAreas)
        {
            DebugRenderer.DrawRectangle(area, color);
            foreach (RectangleF rectangle in otherAreas)
            {
                DebugRenderer.DrawRectangle(rectangle, color);
            }
        }

        #endregion

        #region Draw Rectangle Outlines

        [Conditional("DEBUG")]
        public static void DrawRectangleOutlines(Rectangle rectangle)
        {
            DebugRenderer.DrawRectangleOutlines(new RectangleF(rectangle), DebugRenderer.DefaultColor, DebugRenderer.DefaultLineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangleOutlines(RectangleF rectangle)
        {
            DebugRenderer.DrawRectangleOutlines(rectangle, DebugRenderer.DefaultColor, DebugRenderer.DefaultLineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangleOutlines(Rectangle rectangle, Color color)
        {
            DebugRenderer.DrawRectangleOutlines(new RectangleF(rectangle), color, DebugRenderer.DefaultLineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangleOutlines(RectangleF rectangle, Color color)
        {
            DebugRenderer.DrawRectangleOutlines(rectangle, color, DebugRenderer.DefaultLineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangleOutlines(Rectangle rectangle, Color color, float lineThickness)
        {
            DebugRenderer.DrawRectangleOutlines(new RectangleF(rectangle), color, lineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawRectangleOutlines(RectangleF rectangle, Color color, float lineThickness)
        {
            Vector2 halfX = Vector2.UnitX * lineThickness * 0.5f;
            Vector2 halfY = Vector2.UnitY * lineThickness * 0.5f;

            DebugRenderer.DrawLine(rectangle.TopLeft - halfX, rectangle.TopRight + halfX, color, lineThickness);
            DebugRenderer.DrawLine(rectangle.TopRight - halfY, rectangle.BottomRight + halfY, color, lineThickness);
            DebugRenderer.DrawLine(rectangle.BottomRight + halfX, rectangle.BottomLeft - halfX, color, lineThickness);
            DebugRenderer.DrawLine(rectangle.BottomLeft + halfY, rectangle.TopLeft - halfY, color, lineThickness);
        }

        #endregion

        #region Draw Line

        [Conditional("DEBUG")]
        public static void DrawLine(Vector2 startPosition, Vector2 endPosition)
        {
            DebugRenderer.DrawLine(startPosition, DebugRenderer.DefaultColor, FlaiMath.GetAngle(startPosition - endPosition), Vector2.Distance(startPosition, endPosition), DebugRenderer.DefaultLineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawLine(Vector2 startPosition, Vector2 endPosition, float lineThickness)
        {
            DebugRenderer.DrawLine(startPosition, DebugRenderer.DefaultColor, FlaiMath.GetAngle(startPosition - endPosition), Vector2.Distance(startPosition, endPosition), lineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawLine(Vector2 startPosition, Vector2 endPosition, Color color)
        {
            DebugRenderer.DrawLine(startPosition, color, FlaiMath.GetAngle(startPosition - endPosition), Vector2.Distance(startPosition, endPosition), DebugRenderer.DefaultLineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawLine(Vector2 startPosition, Vector2 endPosition, Color color, float lineThickness)
        {
            DebugRenderer.DrawLine(startPosition, color, FlaiMath.GetAngle(startPosition - endPosition), Vector2.Distance(startPosition, endPosition), lineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawLine(Vector2 startPosition, float rotation, float length)
        {
            DebugRenderer.DrawLine(startPosition, DebugRenderer.DefaultColor, rotation, length, DebugRenderer.DefaultLineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawLine(Vector2 startPosition, float rotation, float length, float lineThickness)
        {
            DebugRenderer.DrawLine(startPosition, DebugRenderer.DefaultColor, rotation, length, lineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawLine(Vector2 startPosition, Color color, float rotation, float length)
        {
            DebugRenderer.DrawLine(startPosition, color, rotation, length, DebugRenderer.DefaultLineThickness);
        }

        [Conditional("DEBUG")]
        public static void DrawLine(Vector2 startPosition, Color color, float rotation, float length, float lineThickness)
        {
            _instance._debugDrawables.Add(new DebugLine2D { StartPosition = startPosition, Color = color, Rotation = rotation, Length = length, Thickness = lineThickness });
        }

        #endregion

        #region Draw String

        [Conditional("DEBUG")]
        public static void DrawString(string fontName, string text, Vector2 position)
        {
            DebugRenderer.DrawString(FlaiGame.Current.Content.LoadFont(fontName), text, position, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(SpriteFont font, string text, Vector2 position)
        {
            DebugRenderer.DrawString(font, text, position, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(string fontName, string text, Vector2 position, Color color)
        {
            DebugRenderer.DrawString(FlaiGame.Current.Content.LoadFont(fontName), text, position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            DebugRenderer.DrawString(font, text, position, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(string fontName, string text, Vector2 position, Color color, float rotation)
        {
            DebugRenderer.DrawString(FlaiGame.Current.Content.LoadFont(fontName), text, position, color, rotation, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(string fontName, string text, Vector2 position, Color color, float rotation, float scale)
        {
            DebugRenderer.DrawString(FlaiGame.Current.Content.LoadFont(fontName), text, position, color, rotation, Vector2.Zero, new Vector2(scale), SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, float scale)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, Vector2.Zero, new Vector2(scale), SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(string fontName, string text, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            DebugRenderer.DrawString(FlaiGame.Current.Content.LoadFont(fontName), text, position, color, rotation, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, Vector2.Zero, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(string fontName, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            DebugRenderer.DrawString(FlaiGame.Current.Content.LoadFont(fontName), text, position, color, rotation, origin, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, origin, Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawString(string fontName, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects)
        {
            DebugRenderer.DrawString(FlaiGame.Current.Content.LoadFont(fontName), text, position, color, rotation, origin, new Vector2(scale), spriteEffects);
        }

        [Conditional("DEBUG")]
        public static void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, origin, new Vector2(scale), spriteEffects);
        }

        [Conditional("DEBUG")]
        public static void DrawString(string fontName, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects)
        {
            DebugRenderer.DrawString(FlaiGame.Current.Content.LoadFont(fontName), text, position, color, rotation, origin, scale, spriteEffects);
        }

        [Conditional("DEBUG")]
        public static void DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects)
        {
            _instance._debugDrawables.Add(new DebugText(font, text, position, color, rotation, origin, scale, spriteEffects));
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(string fontName, string text, Vector2 position)
        {
            SpriteFont font = FlaiGame.Current.Content.LoadFont(fontName);
            DebugRenderer.DrawString(font, text, position, Color.White, 0f, font.Center(text), Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(SpriteFont font, string text, Vector2 position)
        {
            DebugRenderer.DrawString(font, text, position, Color.White, 0f, font.Center(text), Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(string fontName, string text, Vector2 position, Color color)
        {
            SpriteFont font = FlaiGame.Current.Content.LoadFont(fontName);
            DebugRenderer.DrawString(font, text, position, color, 0f, font.Center(text), Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color)
        {
            DebugRenderer.DrawString(font, text, position, color, 0f, font.Center(text), Vector2.One, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(string fontName, string text, Vector2 position, Color color, float rotation, float scale)
        {
            SpriteFont font = FlaiGame.Current.Content.LoadFont(fontName);
            DebugRenderer.DrawString(font, text, position, color, rotation, font.Center(text), scale, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color, float rotation, float scale)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, font.Center(text), scale, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(string fontName, string text, Vector2 position, Color color, float rotation, float scale, SpriteEffects spriteEffects)
        {
            SpriteFont font = FlaiGame.Current.Content.LoadFont(fontName);
            DebugRenderer.DrawString(font, text, position, color, rotation, font.Center(text), scale, spriteEffects);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color, float rotation, float scale, SpriteEffects spriteEffects)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, font.Center(text), scale, spriteEffects);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(string fontName, string text, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            SpriteFont font = FlaiGame.Current.Content.LoadFont(fontName);
            DebugRenderer.DrawString(font, text, position, color, rotation, font.Center(text), scale, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 scale)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, font.Center(text), scale, SpriteEffects.None);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(string fontName, string text, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects)
        {
            SpriteFont font = FlaiGame.Current.Content.LoadFont(fontName);
            DebugRenderer.DrawString(font, text, position, color, rotation, font.Center(text), scale, spriteEffects);
        }

        [Conditional("DEBUG")]
        public static void DrawStringCentered(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects spriteEffects)
        {
            DebugRenderer.DrawString(font, text, position, color, rotation, font.Center(text), scale, spriteEffects);
        }

        #endregion

        [Conditional("DEBUG")]
        internal void Flush(GraphicsContext graphicsContext)
        {
            // 2D
            if (_debugDrawables.Count > 0)
            {
                graphicsContext.SpriteBatch.Begin(_spriteBatchMatrix);

                // Draw rectangles
                foreach (IDebugDrawable drawable in _debugDrawables)
                {
                    drawable.Draw(graphicsContext);
                }
                _debugDrawables.Clear();

                graphicsContext.SpriteBatch.End();
            }

            // TODO: 3D
        }

        [Conditional("DEBUG")]
        public void SetSpriteBatchMatrix(Matrix matrix)
        {
            _spriteBatchMatrix = matrix;
        }

        #region Structs

        private struct DebugRectangle : IDebugDrawable
        {
            public Color Color;
            public RectangleF Rectangle;

            #region IDebugDrawable Members

            void IDebugDrawable.Draw(GraphicsContext graphicsContext)
            {
                graphicsContext.SpriteBatch.Draw(graphicsContext.BlankTexture, this.Rectangle.TopLeft, null, this.Color, 0f, Vector2.Zero, this.Rectangle.Size, SpriteEffects.None, 0f);
            }

            #endregion
        }

        private struct DebugLine2D : IDebugDrawable
        {
            public Color Color;
            public Vector2 StartPosition;
            public float Rotation;
            public float Length;
            public float Thickness;

            #region IDebugDrawable Members

            void IDebugDrawable.Draw(GraphicsContext graphicsContext)
            {
                graphicsContext.SpriteBatch.Draw(graphicsContext.BlankTexture, this.StartPosition, null, this.Color, this.Rotation, new Vector2(0.5f, 0), new Vector2(this.Thickness, this.Length), SpriteEffects.None, 0f);
            }

            #endregion
        }

        private struct DebugText : IDebugDrawable
        {
            public string Text;
            public SpriteFont Font;
            public Vector2 Position;
            public Color Color;
            public float Rotation;
            public Vector2 Origin;
            public Vector2 Scale;
            public SpriteEffects SpriteEffects;

            internal DebugText(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects spriteEffects)
            {
                this.Font = font;
                this.Text = text;
                this.Position = position;
                this.Color = color;
                this.Rotation = rotation;
                this.Origin = origin;
                this.Scale = scale;
                this.SpriteEffects = spriteEffects;
            }

            #region IDebugDrawable Members

            void IDebugDrawable.Draw(GraphicsContext graphicsContext)
            {
                if (this.Font != null && !string.IsNullOrEmpty(this.Text))
                {
                    graphicsContext.SpriteBatch.DrawString(this.Font, this.Text, this.Position, this.Color, this.Rotation, this.Origin, this.Scale, this.SpriteEffects, 0f);
                }
            }

            #endregion
        }

        #endregion

        private interface IDebugDrawable
        {
            void Draw(GraphicsContext graphicsContext);
        }
    }
}
