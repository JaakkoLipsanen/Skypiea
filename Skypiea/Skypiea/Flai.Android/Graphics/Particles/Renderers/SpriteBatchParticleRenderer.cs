using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics.Particles.Renderers
{
    public class SpriteBatchParticleRenderer : ParticleRenderer
    {
        private readonly ParticleBuffer.IteratorMode _iteratorMode;
        protected override ParticleBuffer.IteratorMode IteratorMode
        {
            get { return _iteratorMode; }
        }

        public SpriteBatchParticleRenderer()
            : this(ParticleBuffer.IteratorMode.Normal)
        {
        }

        public SpriteBatchParticleRenderer(ParticleBuffer.IteratorMode iteratorMode)
        {
            _iteratorMode = iteratorMode;
        }

        // awful name. basically, should the renderer not do spriteBatch.Begin/End (and thus also ignore th blend mode)?
        public bool IsSpriteBatchAlreadyRunning { get; set; }

        protected override void Render(GraphicsContext graphicsContext, ref ParticleBuffer.Iterator iterator, TextureDefinition texture, ParticleBlendMode blendMode)
        {
            if (!this.IsSpriteBatchAlreadyRunning)
            {
                graphicsContext.SpriteBatch.Begin(SpriteSortMode.Deferred, blendMode.ToBlendState(), SamplerState.PointClamp, null, null, null);
            }

            float inverseTextureWidth = 1f / texture.Width;
            Vector2 origin = new Vector2(texture.Width, texture.Height) / 2f;

            Particle particle = iterator.First;
            do
            {
                float scale = particle.Scale * inverseTextureWidth;          // Color * alpha is slow....
                graphicsContext.SpriteBatch.Draw(texture, particle.Position, new Color(particle.Color) * particle.Opacity, particle.Rotation, origin, scale);
            } while (iterator.MoveNext(ref particle));

            if (!this.IsSpriteBatchAlreadyRunning)
            {
                graphicsContext.SpriteBatch.End();
            }
        }
    }
}
