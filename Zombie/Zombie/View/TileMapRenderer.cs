using Flai;
using Flai.General;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Model;

namespace Zombie.View
{
    public class TileMapRenderer : FlaiRenderer
    {
        private readonly ITileMap<TileType> _tileMap;
 
        public TileMapRenderer(ITileMap<TileType> tileMap)
        {
            _tileMap = tileMap;
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            RectangleF cameraArea = CameraComponent.Active.GetArea(graphicsContext.ScreenSize);

            int cameraLeft = FlaiMath.Clamp(Tile.WorldToTileCoordinate(cameraArea.Left), 0, _tileMap.Width);
            int cameraRight = FlaiMath.Clamp(Tile.WorldToTileCoordinate(cameraArea.Right) + 1, 0, _tileMap.Width); // todo: + 1 is not necessary if cameraArea.Right is precisely % Tile.Size == 0. but doesnt matter probably
            int cameraTop = FlaiMath.Clamp(Tile.WorldToTileCoordinate(cameraArea.Top), 0, _tileMap.Height);
            int cameraBottom = FlaiMath.Clamp(Tile.WorldToTileCoordinate(cameraArea.Bottom) + 1, 0, _tileMap.Height);

            for (int y = cameraTop; y < cameraBottom; y++)
            {
                for (int x = cameraLeft; x < cameraRight; x++)
                {
                    TileType tile = _tileMap[x, y];
                    if (tile == TileType.Grass)
                    {
                        graphicsContext.SpriteBatch.Draw(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Grass"), Tile.GetTileBounds(x, y), Color.DarkGray);
                    }
                }
            }

            graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(graphicsContext, new RectangleF(0, 0, _tileMap.Width * Tile.Size, _tileMap.Height * Tile.Size), Color.Red, 2f);
        }
    }
}
