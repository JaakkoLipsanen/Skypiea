using System;
using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.ScreenManagement.Screens;
using Flai.Ui;
using Microsoft.Xna.Framework;

namespace Skypiea.Screens
{
    public class PauseScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        public override bool IsPopup
        {
            get { return true; }
        }

        public PauseScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = false;

            this.CreateUiElements();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsActive)
            {
                _uiContainer.Update(updateContext);
                if (updateContext.InputState.IsBackButtonPressed)
                {
                    this.OnContinueClicked();
                }
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin();

            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.BlankTexture, Color.Black * (1f - this.TransitionPosition) * 0.35f);
            if (!this.IsExiting)
            {
                _uiContainer.Draw(graphicsContext, true);
            }

            graphicsContext.SpriteBatch.End();
        }

        private void CreateUiElements()
        {
            Size screenSize = FlaiGame.Current.ScreenSize;
            _uiContainer.Add(new TextButton("Continue", new Vector2(screenSize.Width / 4f, screenSize.Height / 2f), this.OnContinueClicked));
            _uiContainer.Add(new TextButton("Exit", new Vector2(screenSize.Width / 4f * 3f, screenSize.Height / 2f), this.OnExitClicked));
        }

        private void OnContinueClicked()
        {
            this.ExitScreen();
        }

        private void OnExitClicked()
        {
            LoadingScreen.Load(this.ScreenManager, false, new MainMenuScreen());
        }

    }
}
