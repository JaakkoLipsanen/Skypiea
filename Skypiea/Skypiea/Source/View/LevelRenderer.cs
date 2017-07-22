using Flai;
using Flai.CBES.Components;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Model;
using Skypiea.Settings;

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
            Matrix scaleMatrix = Matrix.CreateScale(FlaiGame.Current.GraphicsDevice.PresentationParameters.BackBufferWidth / 800f, FlaiGame.Current.GraphicsDevice.PresentationParameters.BackBufferHeight / 480f, 1);
            graphicsContext.SpriteBatch.Begin(
                SamplerState.PointClamp, 
                CCamera2D.Active.GetTransform(new Size(800, 480)) * scaleMatrix);
            _worldRenderer.Draw(graphicsContext);
            graphicsContext.SpriteBatch.End();

            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp, scaleMatrix); //, Matrix.CreateScale(FlaiGame.Current.ScreenSize.Width / 800f, FlaiGame.Current.ScreenSize.Height / 480f, 1f));
            this.DrawWithoutCamera(graphicsContext);
            graphicsContext.SpriteBatch.End();
        }

        private void DrawWithoutCamera(GraphicsContext graphicsContext)
        {
            _worldRenderer.DrawUI(graphicsContext, _level.UiContainer);

            SkypieaSettingsManager _settingsManager = FlaiGame.Current.Services.Get<SkypieaSettingsManager>();
            if (_settingsManager.Settings.GraphicalQuality == GraphicalQuality.High)
            {
                this.DrawNoise(graphicsContext);
                this.DrawVignette(graphicsContext);
            }
        }

        private void DrawNoise(GraphicsContext graphicsContext)
        {
            // todo: draw only stuff thats visible in the screen? or just simply use texture wrapping....
            TextureDefinition noiseTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "Noise");
            for (int x = Global.Random.Next(-noiseTexture.Width, 0); x < graphicsContext.ScreenSize.Width; x += noiseTexture.Width)
            {
                for (int y = Global.Random.Next(-360, 0); y < graphicsContext.ScreenSize.Height; y += noiseTexture.Height)
                {
                    graphicsContext.SpriteBatch.Draw(noiseTexture, new Vector2(x, y), Color.White * 0.1f);
                }
            }
        }

        private void DrawVignette(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.DrawFullscreen(SkypieaViewConstants.LoadTexture(_contentProvider, "Vignette"));
        }
    }
}
