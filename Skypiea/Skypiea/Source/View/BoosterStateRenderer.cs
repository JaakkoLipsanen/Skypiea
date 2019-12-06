using Flai;
using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Model.Boosters;

namespace Skypiea.View
{
    public class BoosterStateRenderer : FlaiRenderer
    {
        private readonly IBoosterState _boosterState;
        private Booster _currentBooster;
        private float _alpha = 0f;

        public BoosterStateRenderer(EntityWorld entityWorld)
        {
            _boosterState = entityWorld.Services.Get<IBoosterState>();
        }

        protected override void UpdateInner(UpdateContext updateContext)
        {
            this.UpdateAlpha(updateContext);
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            if (_currentBooster == null)
            {
                if (_boosterState.ActiveBooster != null)
                {
                    _currentBooster = _boosterState.ActiveBooster;
                }

                return;
            }

            this.DrawBooster(graphicsContext);           
        }

        private void DrawBooster(GraphicsContext graphicsContext)
        {
            const float BaseAlpha = 0.85f;

            // draw background texture
            graphicsContext.SpriteBatch.DrawCentered(SkypieaViewConstants.LoadTexture(graphicsContext.ContentProvider, "BoosterTextBackground"), new Vector2(graphicsContext.ScreenSize.Width / 2f, 32), Color.White * _alpha * 0.75f * BaseAlpha, 0, new Vector2(2.2f, 1.6f));

            // draw text
            Color textColor = _currentBooster.IsPlayerBooster ? new Color(0, 230, 100) : new Color(255, 64, 64);
            graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer["Minecraftia.24"], _currentBooster.DisplayName, new Vector2(graphicsContext.ScreenSize.Width / 2f, 36), Color.Black * _alpha * BaseAlpha, textColor * _alpha);
            graphicsContext.SpriteBatch.DrawStringFadedCentered(
                graphicsContext.FontContainer["Minecraftia.20"],
                (_currentBooster.TimeRemaining >= 10 ? "0:" : "0:0"), _currentBooster.TimeRemaining.ToString(),
                new Vector2(graphicsContext.ScreenSize.Width / 2f, 72), Color.Black * _alpha * BaseAlpha, Color.White * _alpha * BaseAlpha);
        }

        private void UpdateAlpha(UpdateContext graphicsContext)
        {
            const float FadeSpeed = 2f;
            if (_boosterState.ActiveBooster == null)
            {
                // fade out
                _alpha = FlaiMath.Max(_alpha - graphicsContext.DeltaSeconds * FadeSpeed, 0);
                if (_alpha <= 0)
                {
                    _currentBooster = null;
                }
            }
            else
            {
                // fade in
                _alpha = FlaiMath.Min(_alpha + graphicsContext.DeltaSeconds * FadeSpeed, 1);
            }
        }
    }
}
