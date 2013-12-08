using System;
using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.ScreenManagement.Screens;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Skypiea.Model;

namespace Skypiea.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        private TextToggleButton _worldTypeToggleButton;

        public MainMenuScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = true;
        }

        protected override void LoadContent(bool instancePreserved)
        {
            if (instancePreserved)
            {
                return;
            }

            _uiContainer.Add(new TextButton("Play", new Vector2(this.Game.ScreenSize.Width / 2f, 200), this.OnPlayClicked));
            _uiContainer.Add(new TextButton("Exit", new Vector2(this.Game.ScreenSize.Width / 2F, 280), this.OnExitClicked));
            _uiContainer.Add(_worldTypeToggleButton = new TextToggleButton(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 440), 150), "Grass", "Desert") { Font =  "SegoeWP.24" });
        }

        private void OnPlayClicked()
        {
            LoadingScreen.Load(this.ScreenManager, false, new GameplayScreen(_worldTypeToggleButton.IsToggled ? WorldType.Grass : WorldType.Desert));
        }

        private void OnExitClicked()
        {
            this.Game.Exit();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsActive)
            {
                _uiContainer.Update(updateContext);
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.GraphicsDevice.Clear(Color.Black);
            _uiContainer.Draw(graphicsContext);
        }
    }
}
