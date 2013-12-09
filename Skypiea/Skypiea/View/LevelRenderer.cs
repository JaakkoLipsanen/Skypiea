using Flai;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Components;
using Skypiea.Model;

namespace Skypiea.View
{
    public class LevelRenderer : FlaiRenderer
    {
        private readonly Level _level;
        private readonly WorldRenderer _worldRenderer;

        public LevelRenderer(Level level)
        {
            _level = level;
            _worldRenderer = new WorldRenderer(level.World);
        }

        protected override void LoadContentInner()
        {
            _worldRenderer.LoadContent();
        }

        protected override void UnloadInner()
        {
            _worldRenderer.Unload();
        }

        protected override void UpdateInner(UpdateContext updateContext)
        {
            _worldRenderer.Update(updateContext);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp, CCamera2D.Active);
            _worldRenderer.Draw(graphicsContext);
            graphicsContext.SpriteBatch.End();

            this.DrawUI(graphicsContext);
            this.DrawNoise(graphicsContext);
        }

        // todo: move to it's own class along with vignette etc
        private void DrawNoise(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);

            Texture2D noiseTexture = _contentProvider.DefaultManager.LoadTexture("Noise");
            for (int x = Global.Random.Next(-noiseTexture.Width, 0); x < graphicsContext.ScreenSize.Width; x += noiseTexture.Width)
            {
                for (int y = Global.Random.Next(-360, 0); y < graphicsContext.ScreenSize.Height; y += noiseTexture.Height)
                {
                    graphicsContext.SpriteBatch.Draw(noiseTexture, new Vector2(x, y), Color.White * 0.1f);
                }
            }

            graphicsContext.SpriteBatch.End();
        }

        private void DrawUI(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);
            _worldRenderer.DrawUI(graphicsContext);

            // vignette in DrawUI -method? maybe not.. :P
            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Vignette"));
            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Vignette")); // ...
            graphicsContext.SpriteBatch.End();
        }
    }
}
