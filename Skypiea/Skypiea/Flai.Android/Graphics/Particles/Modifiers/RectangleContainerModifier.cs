
namespace Flai.Graphics.Particles.Modifiers
{
    public class RectangleContainerModifier : ParticleModifier
    {
        public RectangleF ContainerArea { get; set; }
        public RectangleContainerModifier()
        {
        }

        public RectangleContainerModifier(RectangleF containerArea)
        {
            this.ContainerArea = containerArea;
        }

        public override void Process(UpdateContext updateContext, ref ParticleBuffer.Iterator iterator)
        {
            Particle particle = iterator.First;

            float left = this.ContainerArea.Left;
            float right = this.ContainerArea.Right;
            float top = this.ContainerArea.Top;
            float bottom = this.ContainerArea.Bottom;

            do
            {
                if (particle.Position.X < left)
                {
                    particle.Position.X = left + (left - particle.Position.X); // could be simplified = 2 * left - particle.Pos.X
                    particle.Velocity.X *= -1;
                }
                else if (particle.Position.X > right)
                {
                    particle.Position.X = right - (particle.Position.X - right); // 2 * right - y
                    particle.Velocity.X *= -1;
                }

                if (particle.Position.Y < top)
                {
                    particle.Position.Y = top + (top - particle.Position.Y);
                    particle.Velocity.Y *= -1;
                }
                else if (particle.Position.Y > bottom)
                {
                    particle.Position.Y = bottom - (particle.Position.Y - bottom);
                    particle.Velocity.Y *= -1;
                }
                
            } while (iterator.MoveNext(ref particle));
        }
    }
}
