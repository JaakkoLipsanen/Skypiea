

namespace Flai.Graphics.Particles.Renderers
{
    public abstract class ParticleRenderer 
    {
        protected virtual ParticleBuffer.IteratorMode IteratorMode
        {
            get {  return ParticleBuffer.IteratorMode.Normal;}
        }

        protected abstract void Render(GraphicsContext graphicsContext, ref ParticleBuffer.Iterator iterator, TextureDefinition texture, ParticleBlendMode blendMode);

        public void RenderAllEffects(GraphicsContext graphicsContext, ParticleEngine particleEngine)
        {
            for (int i = 0; i < particleEngine.ParticleEffects.Count; i++)
            {
                this.RenderEffect(graphicsContext, particleEngine.ParticleEffects[i]);
            }
        }

        public void RenderEffect(GraphicsContext graphicsContext, ParticleEffect particleEffect)
        {
            for (int i = 0; i < particleEffect.Emitters.Count; i++)
            {
                ParticleEmitter emitter = particleEffect.Emitters[i];
                if (emitter.Buffer.Count == 0)
                {
                    continue;
                }

                ParticleBuffer.Iterator iterator = emitter.Buffer.CreateIterator(this.IteratorMode);
                TextureDefinition textureDefinition = (emitter.Texture != TextureDefinition.Empty) ? emitter.Texture : new TextureDefinition(graphicsContext.BlankTexture);
                this.Render(graphicsContext, ref iterator, textureDefinition, emitter.BlendMode);
            }
        }
    }
}
