using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.ScreenManagement.Screens;
using Flai.Ui;
using Microsoft.Xna.Framework;
using System;

namespace Skypiea.Screens
{
    public class GameOverScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        private readonly IGameContainer _gameContainer;

        public override bool IsPopup
        {
            get { return true; }
        }

        public GameOverScreen(IGameContainer gameContainer)
        {
            _gameContainer = gameContainer;
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = false;

            this.CreateUiElements();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _uiContainer.Update(updateContext);
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin();

            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.BlankTexture, Color.Black * (1f - this.TransitionPosition) * 0.5f);
            if (!this.IsExiting)
            {
                _uiContainer.Draw(graphicsContext, true);
            }

            graphicsContext.SpriteBatch.End();
        }

        private void CreateUiElements()
        {
            Size screenSize = FlaiGame.Current.ScreenSize;
            _uiContainer.Add(new TextButton("Play again!", new Vector2(screenSize.Width / 4f, screenSize.Height / 2f), this.OnReplayClicked));
            _uiContainer.Add(new TextButton("Exit", new Vector2(screenSize.Width / 4f * 3f, screenSize.Height / 2f), this.OnExitClicked));
        }

        private void OnReplayClicked()
        {
            this.ExitScreen();
            _gameContainer.Restart();
        }

        private void OnExitClicked()
        {
            LoadingScreen.Load(this.ScreenManager, false, new MainMenuScreen());
        }
    }
}
