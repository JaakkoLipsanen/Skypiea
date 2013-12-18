using System;

namespace Skypiea.Model
{
    public enum WorldType
    {
        Grass,
        Desert,
        Combined,
    }

    public static class WorldTypeExtensions
    {
        public static string GetMapTextureName(this WorldType worldType)
        {
            switch (worldType)
            {
                case WorldType.Grass:
                    return "Map/GrassMap";

                case WorldType.Combined:
                    return "Map/CombineTest";

                case WorldType.Desert:
                    return "Map/DesertMap";

                default:
                    throw new ArgumentException("worldType");
            }
        }
    }
}
