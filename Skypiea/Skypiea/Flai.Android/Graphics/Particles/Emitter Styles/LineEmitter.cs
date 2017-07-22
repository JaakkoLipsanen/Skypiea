using System;
using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.EmitterStyles
{
    public class LineEmitter : EmitterStyle
    {
        public float Length { get; set; }
        // rotation, emit in every direction vs perpendicular to the normal etc

        public LineEmitter(float length)
        {
            this.Length = length;
        }

        public override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 initialForce)
        {
            throw new NotImplementedException("");
        }
    }
}
