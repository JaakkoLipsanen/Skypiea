using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.Modifiers
{
    // this is pretty damn cool class. from mercury. no idea what YIQ namespace is :D probably similiar to HSV..?
    public class HueShiftModifier : ParticleModifier
    {
        public float HueShiftAmount { get; set; }
        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            float hueChange = this.HueShiftAmount * updateContext.DeltaSeconds * FlaiMath.Pi / 180f;
            Matrix hueShiftMatrix = HueShifter.CreateHueTransformationMatrix(hueChange);

            Particle particle = iterator.First;
            do
            {
                HueShifter.Shift(ref particle.Color, ref hueShiftMatrix);
            } while (iterator.MoveNext(ref particle));
        }
    }
}
