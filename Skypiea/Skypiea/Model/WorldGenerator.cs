using Flai;
using Flai.General;
using Microsoft.Xna.Framework;
using Skypiea.Misc;
using Skypiea.Prefabs;

namespace Skypiea.Model
{
    public static class WorldGenerator
    {
        public static World Generate()
        {
            const int Width = 60;
            const int Height = 30;
            World world = new World(new TileMap<TileType>(new TileType[Width * Height], Width, Height));
            WorldGenerator.CreateEntities(world);

            return world;
        }

        private static void CreateEntities(World world)
        {
            world.EntityWorld.CreateEntityFromPrefab<PlayerPrefab>(EntityNames.Player, new Vector2(world.Width, world.Height) * Tile.Size / 2f);

            const float OffsetFromBorder = 120f;
            world.EntityWorld.CreateEntityFromPrefab<VirtualThumbStickPrefab>(EntityNames.MovementThumbStick, new Vector2(OffsetFromBorder, FlaiGame.Current.ScreenSize.Height - OffsetFromBorder));
            world.EntityWorld.CreateEntityFromPrefab<VirtualThumbStickPrefab>(EntityNames.RotationThumbStick, new Vector2(FlaiGame.Current.ScreenSize.Width - OffsetFromBorder, FlaiGame.Current.ScreenSize.Height - OffsetFromBorder));
        }
    }
}
