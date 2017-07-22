using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Flai.Graphics
{
    /// <summary>
    /// 2D!
    /// </summary>
    public class PrimitiveRenderer
    {
        private const float DefaultLineThickness = 4f;
        private static readonly Color DefaultColor = Color.Red * 0.5f;
        private readonly GraphicsContext _graphicsContext;

        // ???
        internal PrimitiveRenderer(GraphicsContext graphicsContext)
        {
            _graphicsContext = graphicsContext;
        }

        #region Draw Line

        public void DrawLine(Vector2 startPosition, Vector2 endPosition)
        {
            this.DrawLine(startPosition, PrimitiveRenderer.DefaultColor, FlaiMath.GetAngle(endPosition - startPosition), Vector2.Distance(startPosition, endPosition), PrimitiveRenderer.DefaultLineThickness, 0f);
        }

        public void DrawLine(Vector2 startPosition, Vector2 endPosition, float lineThickness)
        {
            this.DrawLine(startPosition, PrimitiveRenderer.DefaultColor, FlaiMath.GetAngle(endPosition - startPosition), Vector2.Distance(startPosition, endPosition), lineThickness, 0f);
        }

        public void DrawLine(Vector2 startPosition, Vector2 endPosition, Color color)
        {
            this.DrawLine(startPosition, color, FlaiMath.GetAngle(endPosition - startPosition), Vector2.Distance(startPosition, endPosition), PrimitiveRenderer.DefaultLineThickness, 0f);
        }

        public void DrawLine(Vector2 startPosition, Vector2 endPosition, Color color, float lineThickness)
        {
            this.DrawLine(startPosition, color, FlaiMath.GetAngle(endPosition - startPosition), Vector2.Distance(startPosition, endPosition), lineThickness, 0f);
        }

        public void DrawLine(Vector2 startPosition, Vector2 endPosition, Color color, float lineThickness, float layerDepth)
        {
            this.DrawLine(startPosition, color, FlaiMath.GetAngle(endPosition - startPosition), Vector2.Distance(startPosition, endPosition), lineThickness, layerDepth);
        }

        public void DrawLine(Vector2 startPosition, float rotation, float length)
        {
            this.DrawLine(startPosition, PrimitiveRenderer.DefaultColor, rotation, length, PrimitiveRenderer.DefaultLineThickness, 0f);
        }

        public void DrawLine(Vector2 startPosition, float rotation, float length, float lineThickness)
        {
            this.DrawLine(startPosition, PrimitiveRenderer.DefaultColor, rotation, length, lineThickness, 0f);
        }

        public void DrawLine(Vector2 startPosition, Color color, float rotation, float length)
        {
            this.DrawLine(startPosition, color, rotation, length, PrimitiveRenderer.DefaultLineThickness, 0f);
        }

        public void DrawLine(Vector2 startPosition, Color color, float rotation, float length, float lineThickness)
        {
            _graphicsContext.SpriteBatch.Draw(_graphicsContext.BlankTexture, startPosition, null, color, rotation, new Vector2(0.5f, 0), new Vector2(lineThickness, length), SpriteEffects.None, 0);
        }

        public void DrawLine(Vector2 startPosition, Color color, float rotation, float length, float lineThickness, float layerDepth)
        {
            _graphicsContext.SpriteBatch.Draw(_graphicsContext.BlankTexture, startPosition, null, color, rotation, new Vector2(0, 0.5f), new Vector2(length, lineThickness), SpriteEffects.None, layerDepth);
        }

        public void DrawLineCentered(Vector2 startPosition, Color color, float rotation, float length, float lineThickness)
        {
            startPosition -= -FlaiMath.GetAngleVector(rotation)*length*0.5f;
            _graphicsContext.SpriteBatch.Draw(_graphicsContext.BlankTexture, startPosition, null, color, rotation, new Vector2(0.5f, 0), new Vector2(lineThickness, length), SpriteEffects.None, 0);
        }

        public void DrawLineCentered(Segment2D segment, Color color, float lineThickness)
        {
            this.DrawLineCentered(segment.Start, color, FlaiMath.GetAngle(segment.Direction), segment.Length(), lineThickness);
        }

        public void DrawLine(Segment2D segment)
        {
            this.DrawLine(segment.Start, segment.End);
        }

        public void DrawLine(Segment2D segment, Color color)
        {
            this.DrawLine(segment.Start, segment.End, color);
        }

        public void DrawLine(Segment2D segment, Color color, float lineThickness)
        {
            this.DrawLine(segment.Start, segment.End, color, lineThickness);
        }

        #endregion

        #region Draw Rectangle

        public void DrawRectangle( RectangleF area)
        {
            this.DrawRectangle(area, PrimitiveRenderer.DefaultColor);
        }

        public void DrawRectangle(Vector2 position, float sideSize)
        {
            if (sideSize < 0)
            {
                throw new ArgumentOutOfRangeException("sideSize");
            }

            Vector2 size = new Vector2(sideSize, sideSize);
            this.DrawRectangle(new RectangleF(position - size / 2f, position + size / 2f), PrimitiveRenderer.DefaultColor);
        }

        public void DrawRectangle(Vector2 position, float sideSize, Color color)
        {
            if (sideSize < 0)
            {
                throw new ArgumentOutOfRangeException("sideSize");
            }

            Vector2 size = new Vector2(sideSize, sideSize);
            this.DrawRectangle(new RectangleF(position - size / 2f, position + size / 2f), color);
        }

        public void DrawRectangle(Vector2 min, Vector2 max)
        {
            if (min.X > max.X || min.Y > max.Y)
            {
                throw new ArgumentOutOfRangeException("min max");
            }

            this.DrawRectangle(new RectangleF(min, max), PrimitiveRenderer.DefaultColor);
        }

        public void DrawRectangle(Vector2 min, Vector2 max, Color color)
        {
            if (min.X > max.X || min.Y > max.Y)
            {
                throw new ArgumentOutOfRangeException("min max");
            }

            this.DrawRectangle(new RectangleF(min, max), color);
        }

        public void DrawRectangle(Rectangle area)
        {
            this.DrawRectangle(new RectangleF(area), PrimitiveRenderer.DefaultColor);
        }

        public void DrawRectangle(Rectangle area, Color color)
        {
            this.DrawRectangle(new RectangleF(area), color);
        }

        public void DrawRectangle(RectangleF area, Color color)
        {
            _graphicsContext.SpriteBatch.Draw(_graphicsContext.BlankTexture, area.TopLeft, null, color, 0f, Vector2.Zero, area.Size, SpriteEffects.None, 0f);
        }

        #endregion

        #region Draw Rectangles

        public void DrawRectangles(Rectangle area, params Rectangle[] otherAreas)
        {
            this.DrawRectangles(PrimitiveRenderer.DefaultColor, area, otherAreas);
        }

        public void DrawRectangles(Color color, Rectangle area, params Rectangle[] otherAreas)
        {
            this.DrawRectangle(new RectangleF(area), color);
            foreach (Rectangle otherArea in otherAreas)
            {
                this.DrawRectangle(new RectangleF(otherArea), color);
            }
        }

        public void DrawRectangles(Rectangle area, IEnumerable<Rectangle> otherAreas)
        {
            this.DrawRectangles(PrimitiveRenderer.DefaultColor, area, otherAreas);
        }

        public void DrawRectangles(Color color, Rectangle area, IEnumerable<Rectangle> otherAreas)
        {
            this.DrawRectangle(new RectangleF(area), color);
            foreach (Rectangle rectangle in otherAreas)
            {
                this.DrawRectangle(new RectangleF(rectangle), color);
            }
        }

        public void DrawRectangles(RectangleF area, params RectangleF[] otherAreas)
        {
            this.DrawRectangles(PrimitiveRenderer.DefaultColor, area, otherAreas);
        }

        public void DrawRectangles(Color color, RectangleF area, params RectangleF[] otherAreas)
        {
            this.DrawRectangle(area, color);
            foreach (RectangleF otherArea in otherAreas)
            {
                this.DrawRectangle(otherArea, color);
            }
        }

        public void DrawRectangles(RectangleF area, IEnumerable<RectangleF> otherAreas)
        {
            this.DrawRectangles(PrimitiveRenderer.DefaultColor, area, otherAreas);
        }

        public void DrawRectangles(Color color, RectangleF area, IEnumerable<RectangleF> otherAreas)
        {
            this.DrawRectangle(area, color);
            foreach (RectangleF rectangle in otherAreas)
            {
                this.DrawRectangle(rectangle, color);
            }
        }

        #endregion

        #region Draw Rectangle Outlines

        public void DrawRectangleOutlines(Rectangle rectangle)
        {
            this.DrawRectangleOutlines(new RectangleF(rectangle), PrimitiveRenderer.DefaultColor, PrimitiveRenderer.DefaultLineThickness);
        }

        public void DrawRectangleOutlines(RectangleF rectangle)
        {
            this.DrawRectangleOutlines(rectangle, PrimitiveRenderer.DefaultColor, PrimitiveRenderer.DefaultLineThickness);
        }

        public void DrawRectangleOutlines(Rectangle rectangle, Color color)
        {
            this.DrawRectangleOutlines(new RectangleF(rectangle), color, PrimitiveRenderer.DefaultLineThickness);
        }

        public void DrawRectangleOutlines(RectangleF rectangle, Color color)
        {
            this.DrawRectangleOutlines(rectangle, color, PrimitiveRenderer.DefaultLineThickness);
        }

        public void DrawRectangleOutlines(Rectangle rectangle, Color color, float lineThickness)
        {
            this.DrawRectangleOutlines(new RectangleF(rectangle), color, lineThickness);
        }

        public void DrawRectangleOutlines(RectangleF rectangle, Color color, float lineThickness)
        {
            // todo: corners are wrong (they overlap -> with alpha != 1 looks wrong)
            Vector2 halfX = Vector2.UnitX * lineThickness * 0.5f;
            Vector2 halfY = Vector2.UnitY * lineThickness * 0.5f;
            this.DrawLine(rectangle.TopLeft - halfX, rectangle.TopRight + halfX, color, lineThickness);
            this.DrawLine(rectangle.TopRight - halfY, rectangle.BottomRight + halfY, color, lineThickness);
            this.DrawLine(rectangle.BottomRight + halfX, rectangle.BottomLeft - halfX, color, lineThickness);
            this.DrawLine(rectangle.BottomLeft + halfY, rectangle.TopLeft - halfY, color, lineThickness);
        }

        #endregion
    }
}
