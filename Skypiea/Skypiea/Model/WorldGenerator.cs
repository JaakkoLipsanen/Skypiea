using Flai;
using Flai.CBES.Components;
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
            world.Initialize();
            
            return world;
        }

        private static void CreateEntities(World world)
        {
            world.EntityWorld.CreateEntityFromPrefab<PlayerPrefab>(EntityNames.Player, SkypieaConstants.MapSizeInPixels / 2f);

            world.EntityWorld.CreateEntityFromPrefab<VirtualThumbStickPrefab>(EntityNames.MovementThumbStick, new Vector2(120f, FlaiGame.Current.ScreenSize.Height - 120f)).Get<CVirtualThumbstick>().ThumbStick.SmoothingPower = 0;
            world.EntityWorld.CreateEntityFromPrefab<VirtualThumbStickPrefab>(EntityNames.RotationThumbStick, new Vector2(FlaiGame.Current.ScreenSize.Width - 60, FlaiGame.Current.ScreenSize.Height - 120f)).Get<CVirtualThumbstick>().ThumbStick.SmoothingPower = 0.35f;
        }
    }
}
