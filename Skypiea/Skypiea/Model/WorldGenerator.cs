﻿using Skypiea.Misc;
using Skypiea.Prefabs;

namespace Skypiea.Model
{
    public static class WorldGenerator
    {
        public static World Generate()
        {
            World world = new World();
            WorldGenerator.CreateEntities(world);
            world.Initialize();
            
            return world;
        }

        private static void CreateEntities(World world)
        {
            world.EntityWorld.CreateEntityFromPrefab<PlayerPrefab>(EntityNames.Player, SkypieaConstants.MapSizeInPixels / 2f);
        }
    }
}
