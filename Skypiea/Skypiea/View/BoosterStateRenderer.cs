using Flai;
using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Model.Boosters;

namespace Skypiea.View
{
    public class BoosterStateRenderer : FlaiRenderer
    {
        private readonly EntityWorld _entityWorld;
        private readonly IBoosterState _boosterState;
        private Booster _currentBooster;
        private float _alpha = 0f;

        public BoosterStateRenderer(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
            _boosterState = _entityWorld.Services.Get<IBoosterState>();
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
            }
            else
            {
                graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("BoosterTextBackground"), new Vector2(graphicsContext.ScreenSize.Width / 2f, 10), Color.White * _alpha, 0, new Vector2(2.2f, 1.6f));

                Color color = _currentBooster.IsPlayerBooster ? new Color(0, 230, 100) : new Color(255, 64, 64);
                graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer["Minecraftia.24"], _currentBooster.DisplayName, new Vector2(graphicsContext.ScreenSize.Width / 2f, 36), Color.Black * _alpha, color * _alpha);
                graphicsContext.SpriteBatch.DrawStringFadedCentered(
                    graphicsContext.FontContainer["Minecraftia.20"], 
                    (_currentBooster.TimeRemaining >= 10 ? "0:" : "0:0"), _currentBooster.TimeRemaining,
                    new Vector2(graphicsContext.ScreenSize.Width / 2f, 72), Color.Black * 0.85f * _alpha, Color.White * _alpha);
            }
        }

        private void UpdateAlpha(UpdateContext graphicsContext)
        {
            if (_boosterState.ActiveBooster == null)
            {
                // fade out
                _alpha = FlaiMath.Max(_alpha - graphicsContext.DeltaSeconds * 2f, 0);
                if (_alpha <= 0)
                {
                    _currentBooster = null;
                }
            }
            else
            {
                // fade in
                _alpha = FlaiMath.Min(_alpha + graphicsContext.DeltaSeconds * 2f, 1);
            }
        }
    }
}
