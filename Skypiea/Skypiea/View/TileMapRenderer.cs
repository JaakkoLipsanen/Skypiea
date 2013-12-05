using Flai;
using Flai.General;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Model;

namespace Skypiea.View
{
    public class TileMapRenderer : FlaiRenderer
    {
        private const int PixelSizeMultiplier = 4;
        private static readonly Color Color = Color.DarkGray;

        private readonly ITileMap<TileType> _tileMap;
        public TileMapRenderer(ITileMap<TileType> tileMap)
        {
            _tileMap = tileMap;
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            this.DrawMap(graphicsContext);
            this.DrawSides(graphicsContext);
            this.DrawCorners(graphicsContext);

        //  graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(new Rectangle(0, 0, _tileMap.Width * Tile.Size, _tileMap.Height * Tile.Size));
        }

        private void DrawMap(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Draw(
                 graphicsContext.ContentProvider.DefaultManager.LoadTexture("GrassFullscreen"),
                 Vector2.Zero, new Rectangle(0, 0, _tileMap.Width * Tile.Size / TileMapRenderer.PixelSizeMultiplier, _tileMap.Height * Tile.Size / TileMapRenderer.PixelSizeMultiplier), TileMapRenderer.Color, 0, TileMapRenderer.PixelSizeMultiplier);
        }

        private void DrawSides(GraphicsContext graphicsContext)
        {
            Texture2D texture = _contentProvider.DefaultManager.LoadTexture("GrassFullscreenSide");
            Rectangle horizontalSourceRectangle = new Rectangle(0, 0, texture.Width, FlaiMath.Min(texture.Height, _tileMap.Width * Tile.Size / TileMapRenderer.PixelSizeMultiplier));
            Rectangle verticalSourceRectangle = new Rectangle(0, 0, texture.Width, FlaiMath.Min(texture.Height, _tileMap.Height * Tile.Size / TileMapRenderer.PixelSizeMultiplier));

            // left/right
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.TopRight, verticalSourceRectangle, TileMapRenderer.Color, 0, TileMapRenderer.PixelSizeMultiplier); // left
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitX * _tileMap.Width * Tile.Size, Corner.TopLeft, verticalSourceRectangle, TileMapRenderer.Color, 0, TileMapRenderer.PixelSizeMultiplier, SpriteEffects.FlipHorizontally); // right

            // top/bottom
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.TopLeft, horizontalSourceRectangle, TileMapRenderer.Color, -FlaiMath.PiOver2, TileMapRenderer.PixelSizeMultiplier, SpriteEffects.FlipHorizontally); // top
            graphicsContext.SpriteBatch.Draw(
                texture, Vector2.UnitY * _tileMap.Height * Tile.Size + Vector2.UnitY * texture.Width * TileMapRenderer.PixelSizeMultiplier,
                Corner.TopLeft, horizontalSourceRectangle, TileMapRenderer.Color, -FlaiMath.PiOver2, TileMapRenderer.PixelSizeMultiplier); // bottom
        }

        private void DrawCorners(GraphicsContext graphicsContext)
        {
            Texture2D texture = _contentProvider.DefaultManager.LoadTexture("GrassFullscreenCorner");

            // top
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.BottomRight, TileMapRenderer.Color, 0, TileMapRenderer.PixelSizeMultiplier); // top-left
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitX * _tileMap.Width * Tile.Size, Corner.BottomLeft, TileMapRenderer.Color, 0, TileMapRenderer.PixelSizeMultiplier, SpriteEffects.FlipHorizontally); // top-right

            // bottom
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitY * _tileMap.Height * Tile.Size, Corner.TopRight, TileMapRenderer.Color, 0, TileMapRenderer.PixelSizeMultiplier, SpriteEffects.FlipVertically); // bottom-left
            graphicsContext.SpriteBatch.Draw(texture, new Vector2(_tileMap.Width, _tileMap.Height) * Tile.Size, Corner.TopLeft, TileMapRenderer.Color, 0, TileMapRenderer.PixelSizeMultiplier, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically); // bottom-right
        }
    }
}
