using Flai;
using Flai.Graphics;
using Flai.Misc;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Settings;
using Skypiea.View;
using System;

namespace Skypiea.Screens
{
    public class RateScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        public override bool IsPopup
        {
            get { return true; }
        }

        public RateScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.3f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.3f);
            this.FadeType = FadeType.FadeAlpha;

            this.CreateUi();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsActive)
            {
                if (updateContext.InputState.IsBackButtonPressed)
                {
                    this.ExitScreen();
                }

                _uiContainer.Update(updateContext);
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp, SkypieaViewConstants.RenderScaleMatrix);
            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.BlankTexture, Color.Black * 0.675f);
            _uiContainer.Draw(graphicsContext, true);
            graphicsContext.SpriteBatch.End();
        }

        private void CreateUi()
        {
            _uiContainer.Add(new TextBlock("Please rate the game if you like it!", new Vector2(Screen.Width / 2f, Screen.Height / 10f)) { Color = Color.White });
            _uiContainer.Add(new TextBlock("Rating helps us make the game even better!", new Vector2(Screen.Width / 2f, Screen.Height / 10f + 34)) { Color = Color.White });

            _uiContainer.Add(new TextButton("Rate now!", new Vector2(Screen.Width / 5f, Screen.Height / 4f * 2), this.OnRateClicked));
            _uiContainer.Add(new TextButton("No thanks", new Vector2(Screen.Width / 5f * 4f, Screen.Height / 4f * 2), () => this.ExitScreen()));
        }

        private void OnRateClicked()
        {
            ApplicationInfo.OpenApplicationReviewPage();
            this.ExitScreen();

            SkypieaSettingsManager settingsManager = FlaiGame.Current.Services.Get<SkypieaSettingsManager>();
            settingsManager.Settings.IsRateWindowShown = true;
            settingsManager.Save();

        }
    }
}
