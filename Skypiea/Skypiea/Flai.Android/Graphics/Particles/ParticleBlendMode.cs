using Microsoft.Xna.Framework.Graphics;
using System;

namespace Flai.Graphics.Particles
{
    public enum ParticleBlendMode
    {
        Alpha,
        Additive,
    }

    public static class ParticleBlendModeExtensions
    {
        public static BlendState ToBlendState(this ParticleBlendMode blendMode)
        {
            switch (blendMode)
            {
                case ParticleBlendMode.Alpha:
                    return BlendState.AlphaBlend;

                case ParticleBlendMode.Additive:
                    return BlendState.Additive;

                default:
                    throw new ArgumentException("blendMode");
            }
        }
    }
}
