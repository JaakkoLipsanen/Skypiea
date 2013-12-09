using System;

namespace Skypiea.Model
{
    public enum WorldType
    {
        Grass,
        Desert,
    }

    public static class WorldTypeExtensions
    {
        public static string GetMapTextureName(this WorldType worldType)
        {
            switch (worldType)
            {
                case WorldType.Grass:
                    return "Map/GrassMap";

                case WorldType.Desert:
                    return "Map/DesertMap";

                default:
                    throw new ArgumentException("worldType");
            }
        }
    }
}
