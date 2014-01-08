using Flai;
using Flai.CBES.Components;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.View
{
    public class MapRenderer : FlaiRenderer
    {
        private readonly Color _color;
        private readonly string _mapTextureName = MapRenderer.GetRandomMap();
        private readonly SpriteEffects _spriteEffects;

        // ookaayy... world isn't used... but I won't remove it since it's strange if the renderer doesnt take any parameters :D :D
        public MapRenderer(World world)
        {
            _color = Color.LightGray;

            // create sprite effect that is either None, Horizontal, Vertical or Both 
            _spriteEffects = Global.Random.NextBoolean() ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            _spriteEffects = Global.Random.NextBoolean() ? _spriteEffects | SpriteEffects.FlipVertically : _spriteEffects;
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            // draws the actual map texture
            Vector2i size = SkypieaConstants.MapSizeInPixels / SkypieaViewConstants.PixelSize + Vector2i.One * SkypieaViewConstants.FadeLength / SkypieaViewConstants.PixelSize * 2;
            graphicsContext.SpriteBatch.Draw(
                 SkypieaViewConstants.LoadTexture(_contentProvider, _mapTextureName),
                 -Vector2.One * SkypieaViewConstants.FadeLength, new Rectangle(0, 0, size.X, size.Y), _color, 0, Vector2.Zero, SkypieaViewConstants.PixelSize, _spriteEffects, 0f);
        }

        // draws the side "fades" (makes the map fade to black)
        public void DrawFades(GraphicsContext graphicsContext)
        {
            this.DrawSides(graphicsContext);
            this.DrawCorners(graphicsContext);
            this.DrawBackgroundClear(graphicsContext);         
        }

        private void DrawSides(GraphicsContext graphicsContext)
        {
            TextureDefinition texture = SkypieaViewConstants.LoadTexture(_contentProvider, "SideFadeTexture");
            Rectangle horizontalSourceRectangle = new Rectangle(0, 0, texture.Width, FlaiMath.Min(texture.Height, SkypieaConstants.MapWidthInPixels / SkypieaViewConstants.PixelSize));
            Rectangle verticalSourceRectangle = new Rectangle(0, 0, texture.Width, FlaiMath.Min(texture.Height, SkypieaConstants.MapHeightInPixels / SkypieaViewConstants.PixelSize));

            // left/right
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.TopRight, verticalSourceRectangle, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize); // left
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitX * SkypieaConstants.MapWidthInPixels, Corner.TopLeft, verticalSourceRectangle, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize, SpriteEffects.FlipHorizontally); // right

            // top/bottom
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.TopLeft, horizontalSourceRectangle, SkypieaViewConstants.ClearColor, -FlaiMath.PiOver2, SkypieaViewConstants.PixelSize, SpriteEffects.FlipHorizontally); // top
            graphicsContext.SpriteBatch.Draw(
                texture, Vector2.UnitY * SkypieaConstants.MapHeightInPixels + Vector2.UnitY * texture.Width * SkypieaViewConstants.PixelSize,
                Corner.TopLeft, horizontalSourceRectangle, SkypieaViewConstants.ClearColor, -FlaiMath.PiOver2, SkypieaViewConstants.PixelSize); // bottom
        }

        private void DrawCorners(GraphicsContext graphicsContext)
        {
            TextureDefinition texture = SkypieaViewConstants.LoadTexture(_contentProvider, "CornerFadeTexture");

            // top
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.BottomRight, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize); // top-left
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitX * SkypieaConstants.MapWidthInPixels, Corner.BottomLeft, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize, SpriteEffects.FlipHorizontally); // top-right

            // bottom
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitY * SkypieaConstants.MapHeightInPixels, Corner.TopRight, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize, SpriteEffects.FlipVertically); // bottom-left
            graphicsContext.SpriteBatch.Draw(texture, SkypieaConstants.MapSizeInPixels, Corner.TopLeft, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically); // bottom-right
        }

        private void DrawBackgroundClear(GraphicsContext graphicsContext)
        { // clear to black
            RectangleF cameraArea = CCamera2D.Active.GetArea();
            if (cameraArea.Left < -SkypieaViewConstants.FadeLength)
            {
                graphicsContext.PrimitiveRenderer.DrawRectangle(cameraArea.TopLeft, new Vector2(-SkypieaViewConstants.FadeLength, cameraArea.Bottom), SkypieaViewConstants.ClearColor);
            }
            else if (cameraArea.Right > SkypieaConstants.MapWidthInPixels + SkypieaViewConstants.FadeLength)
            {
                graphicsContext.PrimitiveRenderer.DrawRectangle(new Vector2(SkypieaConstants.MapWidthInPixels + SkypieaViewConstants.FadeLength, cameraArea.Top), cameraArea.BottomRight, SkypieaViewConstants.ClearColor);
            }

            if (cameraArea.Top < -SkypieaViewConstants.FadeLength)
            {
                graphicsContext.PrimitiveRenderer.DrawRectangle(cameraArea.TopLeft, new Vector2(cameraArea.Right, -SkypieaViewConstants.FadeLength), SkypieaViewConstants.ClearColor);
            }
            else if (cameraArea.Bottom > SkypieaConstants.MapHeightInPixels + SkypieaViewConstants.FadeLength)
            {
                graphicsContext.PrimitiveRenderer.DrawRectangle(new Vector2(cameraArea.Left, SkypieaConstants.MapHeightInPixels + SkypieaViewConstants.FadeLength), cameraArea.BottomRight, SkypieaViewConstants.ClearColor);
            }
        }

        private static string GetRandomMap()
        {
            const int Count = 9;
            return "Map" + Global.Random.Next(1, Count + 1);
        }
    }
}
