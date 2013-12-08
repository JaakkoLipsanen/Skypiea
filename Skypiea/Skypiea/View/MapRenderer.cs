﻿using Flai;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Components;
using Skypiea.Model;

namespace Skypiea.View
{
    public static class SkypieaViewConstants
    {
        public static readonly Color ClearColor = Color.Black;

        public const int PixelSize = 4;
        public const int FadeLength = 20 * SkypieaViewConstants.PixelSize;
    }

    public class MapRenderer : FlaiRenderer
    {
        private readonly Color _color;

        private readonly World _world;
        public MapRenderer(World world)
        {
            _world = world;
            _color = (world.WorldType == WorldType.Grass) ? Color.DarkGray : Color.LightGray;
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            // draws the actual map texture
            Vector2i size = new Vector2i(_world.Width, _world.Height) * Tile.Size / SkypieaViewConstants.PixelSize + Vector2i.One * SkypieaViewConstants.FadeLength / SkypieaViewConstants.PixelSize * 2;
            graphicsContext.SpriteBatch.Draw(
                 graphicsContext.ContentProvider.DefaultManager.LoadTexture(_world.WorldType.GetMapTextureName()),
                 -Vector2.One * SkypieaViewConstants.FadeLength, new Rectangle(0, 0, size.X, size.Y), _color, 0, SkypieaViewConstants.PixelSize);
        }

        // draws the side "fades" (makes the map fade to black)
        public void DrawFades(GraphicsContext graphicsContext)
        {
            this.DrawSides(graphicsContext);
            this.DrawCorners(graphicsContext);

            RectangleF cameraArea = CCamera2D.Active.GetArea();
            if (cameraArea.Left < -SkypieaViewConstants.FadeLength)
            {
                graphicsContext.PrimitiveRenderer.DrawRectangle(cameraArea.TopLeft, new Vector2(-SkypieaViewConstants.FadeLength, cameraArea.Bottom), SkypieaViewConstants.ClearColor);
            }
            else if (cameraArea.Right > _world.Width * Tile.Size + SkypieaViewConstants.FadeLength)
            {
                graphicsContext.PrimitiveRenderer.DrawRectangle(new Vector2(_world.Width * Tile.Size + SkypieaViewConstants.FadeLength, cameraArea.Top), cameraArea.BottomRight, SkypieaViewConstants.ClearColor);
            }

            if (cameraArea.Top < -SkypieaViewConstants.FadeLength)
            {
                graphicsContext.PrimitiveRenderer.DrawRectangle(cameraArea.TopLeft, new Vector2(cameraArea.Right, -SkypieaViewConstants.FadeLength), SkypieaViewConstants.ClearColor);
            }
            else if (cameraArea.Bottom > _world.Height * Tile.Size + SkypieaViewConstants.FadeLength)
            {
                graphicsContext.PrimitiveRenderer.DrawRectangle(new Vector2(cameraArea.Left, _world.Height * Tile.Size + SkypieaViewConstants.FadeLength), cameraArea.BottomRight, SkypieaViewConstants.ClearColor);
            }
        }

        private void DrawSides(GraphicsContext graphicsContext)
        {
            Texture2D texture = _contentProvider.DefaultManager.LoadTexture("Map/SideFadeTexture");
            Rectangle horizontalSourceRectangle = new Rectangle(0, 0, texture.Width, FlaiMath.Min(texture.Height, _world.Width * Tile.Size / SkypieaViewConstants.PixelSize));
            Rectangle verticalSourceRectangle = new Rectangle(0, 0, texture.Width, FlaiMath.Min(texture.Height, _world.Height * Tile.Size / SkypieaViewConstants.PixelSize));

            // left/right
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.TopRight, verticalSourceRectangle, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize); // left
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitX * _world.Width * Tile.Size, Corner.TopLeft, verticalSourceRectangle, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize, SpriteEffects.FlipHorizontally); // right

            // top/bottom
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.TopLeft, horizontalSourceRectangle, SkypieaViewConstants.ClearColor, -FlaiMath.PiOver2, SkypieaViewConstants.PixelSize, SpriteEffects.FlipHorizontally); // top
            graphicsContext.SpriteBatch.Draw(
                texture, Vector2.UnitY * _world.Height * Tile.Size + Vector2.UnitY * texture.Width * SkypieaViewConstants.PixelSize,
                Corner.TopLeft, horizontalSourceRectangle, SkypieaViewConstants.ClearColor, -FlaiMath.PiOver2, SkypieaViewConstants.PixelSize); // bottom
        }

        private void DrawCorners(GraphicsContext graphicsContext)
        {
            Texture2D texture = _contentProvider.DefaultManager.LoadTexture("Map/CornerFadeTexture");

            // top
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, Corner.BottomRight, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize); // top-left
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitX * _world.Width * Tile.Size, Corner.BottomLeft, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize, SpriteEffects.FlipHorizontally); // top-right

            // bottom
            graphicsContext.SpriteBatch.Draw(texture, Vector2.UnitY * _world.Height * Tile.Size, Corner.TopRight, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize, SpriteEffects.FlipVertically); // bottom-left
            graphicsContext.SpriteBatch.Draw(texture, new Vector2(_world.Width, _world.Height) * Tile.Size, Corner.TopLeft, SkypieaViewConstants.ClearColor, 0, SkypieaViewConstants.PixelSize, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically); // bottom-right
        }
    }
}