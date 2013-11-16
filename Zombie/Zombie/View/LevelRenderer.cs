using Flai;
using Flai.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Zombie.Components;
using Zombie.Model;

namespace Zombie.View
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
            graphicsContext.SpriteBatch.Begin(SamplerState.LinearWrap, CameraComponent.Active);
            _worldRenderer.Draw(graphicsContext);
            graphicsContext.SpriteBatch.End();

            graphicsContext.SpriteBatch.Begin();
            _worldRenderer.DrawUI(graphicsContext);
            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Vignette"));
            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Vignette"));
            graphicsContext.SpriteBatch.End();
        }
    }
}
