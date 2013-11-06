using Flai;
using Flai.CBES;
using Flai.General;
using Microsoft.Xna.Framework;
using Zombie.Misc;
using Zombie.Prefabs;

namespace Zombie.Model
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
            world.EntityWorld.AddEntity(Prefab.CreateInstance<PlayerPrefab>(EntityTags.Player, new Vector2(world.Width, world.Height) * Tile.Size / 2f));

            const float OffsetFromBorder = 120f;
            world.EntityWorld.AddEntity(Prefab.CreateInstance<VirtualThumbStickPrefab>(EntityTags.MovementThumbStick, new Vector2(OffsetFromBorder, FlaiGame.Current.ScreenSize.Height - OffsetFromBorder)));
            world.EntityWorld.AddEntity(Prefab.CreateInstance<VirtualThumbStickPrefab>(EntityTags.RotationThumbStick, new Vector2(FlaiGame.Current.ScreenSize.Width - OffsetFromBorder, FlaiGame.Current.ScreenSize.Height - OffsetFromBorder)));
        }
    }
}
