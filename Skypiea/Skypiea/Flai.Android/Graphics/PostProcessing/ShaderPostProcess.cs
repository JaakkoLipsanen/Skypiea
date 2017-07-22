using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics.PostProcessing
{
    public class ShaderPostProcess : PostProcess
    {
        protected readonly Effect _effect;

        public EffectParameterCollection Parameters
        {
            get { return _effect.Parameters; }
        }

        public ShaderPostProcess(Effect effect)
        {
            _effect = effect;
        }

        public override void Apply(GraphicsContext graphicsContext, RenderTarget2D input, RenderTarget2D output)
        {
            _graphicsDevice.SetRenderTarget(output);

            graphicsContext.SpriteBatch.Begin(_effect);
            graphicsContext.SpriteBatch.DrawFullscreen(input);
            graphicsContext.SpriteBatch.End();
        }
    }
}
