using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Input.Touch;
using Skypiea.Leaderboards;

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

        public GameOverScreen(IGameContainer gameContainer, int score)
        {
            _gameContainer = gameContainer;
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeType = FadeType.FadeAlpha;
            this.EnabledGestures = GestureType.Tap;

            this.CreateUiElements(score);
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsActive)
            {
                if (updateContext.InputState.IsBackButtonPressed)
                {
                    this.OnExitClicked();
                }

                _uiContainer.Update(updateContext);
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin();
            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.BlankTexture, Color.Black * 0.5f);
            //if (!this.IsExiting)
            {
                _uiContainer.Draw(graphicsContext, true);
            }

            graphicsContext.SpriteBatch.End();
        }

        private void CreateUiElements(int playerScore)
        {
            _uiContainer.Add(new TextButton("Play again!", new Vector2(Screen.Width / 4f, Screen.Height / 2f), this.OnReplayClicked));
            _uiContainer.Add(new TextButton("Exit", new Vector2(Screen.Width / 4f * 3f, Screen.Height / 2f), this.OnExitClicked));

            _uiContainer.Add(new TextBlock("Your score was " + playerScore + "!", new Vector2(Screen.Width / 2f, Screen.Height / 4f)));
            if (HighscoreHelper.IsHighscore(playerScore))
            {
                _uiContainer.Add(new TextBlock("New highscore!", new Vector2(Screen.Width / 2f, Screen.Height / 4f - 32)) { Color = new Color(64, 255, 64) });
            }
        }

        private void OnReplayClicked()
        {
            if (this.IsExiting)
            {
                return;
            }

            this.ExitScreen();
            _gameContainer.Restart();
        }

        private void OnExitClicked()
        {
            if (this.IsExiting)
            {
                return;
            }

            this.Exited += () =>
            {
                this.ScreenManager.AddScreen(new MenuBackgroundScreen());
                this.ScreenManager.AddScreen(new MainMenuScreen(FadeDirection.Up));
            };

            this.ScreenManager.ExitAllScreens();
        }
    }
}
