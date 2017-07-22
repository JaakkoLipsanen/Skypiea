using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.Modifiers
{
    public class PointAttractionModifier : ParticleModifier
    {
        public Vector2 AttractionPoint;
        public float AttractionPower = 2f;

        public PointAttractionModifier(Vector2 attractionPoint)
        {
            this.AttractionPoint = attractionPoint;
        }

        public PointAttractionModifier(Vector2 attractionPoint, float attractionPower)
        {
            this.AttractionPoint = attractionPoint;
            this.AttractionPower = attractionPower;
        }


        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            const float Multiplier = 200;
            float tempAttractionPower = this.AttractionPower * updateContext.DeltaSeconds * Multiplier;
            Particle particle = iterator.First;

            do
            {
                float distance = Vector2.Distance(this.AttractionPoint, particle.Position);
                particle.Velocity += (this.AttractionPoint - particle.Position) * tempAttractionPower / distance;
            } while (iterator.MoveNext(ref particle));
        }
    }
}
