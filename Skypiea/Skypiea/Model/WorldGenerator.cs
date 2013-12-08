using Flai;
using Microsoft.Xna.Framework;
using Skypiea.Misc;
using Skypiea.Prefabs;

namespace Skypiea.Model
{
    public static class WorldGenerator
    {
        public static World Generate(WorldType worldType)
        {
            World world = new World(worldType);
            WorldGenerator.CreateEntities(world);

            return world;
        }

        private static void CreateEntities(World world)
        {
            world.EntityWorld.CreateEntityFromPrefab<PlayerPrefab>(EntityNames.Player, new Vector2(world.Width, world.Height) * Tile.Size / 2f);

            world.EntityWorld.CreateEntityFromPrefab<VirtualThumbStickPrefab>(EntityNames.MovementThumbStick, new Vector2(120f, FlaiGame.Current.ScreenSize.Height - 120f));
            world.EntityWorld.CreateEntityFromPrefab<VirtualThumbStickPrefab>(EntityNames.RotationThumbStick, new Vector2(FlaiGame.Current.ScreenSize.Width - 60, FlaiGame.Current.ScreenSize.Height - 120f));
        }
    }
}
