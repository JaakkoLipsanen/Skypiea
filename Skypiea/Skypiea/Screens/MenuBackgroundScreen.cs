using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Misc;
using Skypiea.View;
using System;

namespace Skypiea.Screens
{
    public class MenuBackgroundScreen : GameScreen
    {
        public MenuBackgroundScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeType = FadeType.FadeToBlack;
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);
            graphicsContext.SpriteBatch.DrawFullscreen(SkypieaViewConstants.LoadTexture(graphicsContext.ContentProvider, "MenuBackground"), new Color(64, 64, 64));
            graphicsContext.SpriteBatch.End();
        }

        protected override void PostDraw(GraphicsContext graphicsContext)
        {
            if (Settings.Current.GraphicalQuality == GraphicalQuality.High)
            {
                graphicsContext.SpriteBatch.Begin(SamplerState.PointWrap);
                this.DrawNoise(graphicsContext);
                graphicsContext.SpriteBatch.End();

                graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);
                this.DrawVignette(graphicsContext);
                graphicsContext.SpriteBatch.End();
            }
        }

        private void DrawVignette(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.DrawFullscreen(SkypieaViewConstants.LoadTexture(graphicsContext.ContentProvider, "PostProcessing/Vignette")); // ...
            graphicsContext.SpriteBatch.DrawFullscreen(SkypieaViewConstants.LoadTexture(graphicsContext.ContentProvider, "PostProcessing/Vignette")); // ...
        }

        private void DrawNoise(GraphicsContext graphicsContext)
        {
            var noiseTexture = SkypieaViewConstants.LoadTexture(graphicsContext.ContentProvider, "PostProcessing/Noise");
            for (int x = Global.Random.Next(-noiseTexture.Width, 0); x < graphicsContext.ScreenSize.Width; x += noiseTexture.Width)
            {
                for (int y = Global.Random.Next(-360, 0); y < graphicsContext.ScreenSize.Height; y += noiseTexture.Height)
                {
                    graphicsContext.SpriteBatch.Draw(noiseTexture, new Vector2(x, y), Color.White * 0.05f);
                }
            }
        }
    }
}
