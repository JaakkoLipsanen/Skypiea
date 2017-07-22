#region License
// 
// Copyright © 2012, Jaakko Lipsanen
// All rights reserved.
// 
// Originally built as part of IgnoringEngine
// 
#endregion
/*
 * @The Communist Duck
 * http://gamedev.stackexchange.com/a/13375/9790
 */

namespace Flai.Misc
{
    public static class NoiseGenerator
    {
        // Seed is not used currently anywhere in the generation, not sure how to make the code use it. It'd probably need some modifications to SimplexNoise.cs 
        //  public static int Seed { get; private set; }
        public static int Octaves { get; set; }
        public static float Amplitude { get; set; }
        public static float Persistence { get; set; }
        public static float Frequency { get; set; }

        static NoiseGenerator()
        {
            // NoiseGenerator.Seed = Global.Random.Next(Int32.MaxValue);
            NoiseGenerator.Octaves = 8;
            NoiseGenerator.Amplitude = 1f;
            NoiseGenerator.Frequency = 0.015f;
            NoiseGenerator.Persistence = 0.65f;
        }

        // umm?? clearly returns [-1, 1]... dunno what i was thinking..?
        // returns ~[0, 1] (in my tests only ~[0.08, 1] but not sure if my tests just were bad :P)
        public static float GetNoise(float x, float y)
        {
            float total = 0.0f;
            float frequency = NoiseGenerator.Frequency;
            float amplitude = NoiseGenerator.Amplitude;

            for (int i = 0; i < NoiseGenerator.Octaves; ++i)
            {
                total = total + SimplexNoise.GetNoise(x * frequency, y * frequency) * amplitude;
                frequency *= 2;
                amplitude *= NoiseGenerator.Persistence;
            }

            if (total < -2.4f)
            {
                total = -2.4f;
            }
            else if (total > 2.4f)
            {
                total = 2.4f;
            }

            return total / 2.4f;
        }

        // returns ~[0, 1] (in my tests only ~[0.08, 1] but not sure if my tests just were bad :P)
        public static float GetNoise(float x, float y, float z)
        {
            float total = 0.0f;
            float frequency = NoiseGenerator.Frequency;
            float amplitude = NoiseGenerator.Amplitude;

            for (int i = 0; i < NoiseGenerator.Octaves; ++i)
            {
                total = total + SimplexNoise.GetNoise(x * frequency, y * frequency, z * frequency) * amplitude;
                frequency *= 2;
                amplitude *= NoiseGenerator.Persistence;
            }

            const float Min = -2.107838f;
            const float Max = 2.15875816f;
            if (total < Min)
            {
                total = Min;
            }
            else if (total > Max)
            {
                total = Max;
            }

            // flaimath.scale
            return (total - Min) / (Max - Min);
        }

        public static float GetNoise(float x, float y, float z, float w)
        {
            float total = 0.0f;
            float frequency = NoiseGenerator.Frequency;
            float amplitude = NoiseGenerator.Amplitude;

            for (int i = 0; i < NoiseGenerator.Octaves; ++i)
            {
                total = total + SimplexNoise.GetNoise(x * frequency, y * frequency, z * frequency, w * frequency) * amplitude;
                frequency *= 2;
                amplitude *= NoiseGenerator.Persistence;
            }

            if (total < -2.4f)
            {
                total = -2.4f;
            }
            else if (total > 2.4f)
            {
                total = 2.4f;
            }

            return total / 2.4f;
        }
    }
}
