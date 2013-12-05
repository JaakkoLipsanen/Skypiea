using Flai;
using Microsoft.Xna.Framework;

namespace Skypiea.Model
{
    public enum TileType
    {
        Grass
    }

    public static class Tile
    {
        public const int Size = 32;
        public static int WorldToTileCoordinate(float x)
        {
            return x >= 0 ? (int)(x / Tile.Size) : (int)(FlaiMath.Floor(x / Tile.Size));
        }

        public static Vector2i WorldToTileCoordinate(Vector2 v)
        {
            return new Vector2i(
                Tile.WorldToTileCoordinate(v.X),
                Tile.WorldToTileCoordinate(v.Y));
        }

        public static RectangleF GetTileBounds(int x, int y)
        {
            return new RectangleF(x * Tile.Size, y * Tile.Size, Tile.Size, Tile.Size);
        }
    }
}
