using System;
using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.EmitterStyles
{
    // todo: emitter that emits the particles in spiral. so basically offset = 0 and direction/force changes always a bit between the emitted particles
    public class SpiralEmitter : EmitterStyle
    {
        public override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 initialForce)
        {
            throw new NotImplementedException("");
        }
    }
}
