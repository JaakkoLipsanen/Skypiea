using System;
using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Settings;

namespace Skypiea.Screens
{
    public class HelpScreen : GameScreen
    {
        private int _currentScreen = 0;
        private readonly bool _isPlay;

        public HelpScreen(bool isPlay)
        {
            _isPlay = isPlay;

            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeType = FadeType.FadeAlpha;
            this.FadeIn = FadeDirection.None;
            this.FadeOut = FadeDirection.None;
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsExiting)
            {
                return;
            }

            if (updateContext.InputState.IsBackButtonPressed)
            {
                this.LoadMainMenu();
            }

            if (updateContext.InputState.IsNewTouchAt(updateContext.ScreenArea) && !this.IsExiting)
            {
                _currentScreen++;
                if (_currentScreen == 4)
                {
                    SkypieaSettingsManager settings = this.Game.Services.Get<SkypieaSettingsManager>();
                    if (!settings.Settings.IsFirstHelpShown)
                    {
                        settings.Settings.IsFirstHelpShown = true;
                        settings.Save();

                        if (_isPlay)
                        {
                            this.LoadGameplayScreen();
                            return;
                        }
                    }

                    this.LoadMainMenu();
                }
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            SpriteFont font = graphicsContext.FontContainer["Minecraftia.16"];

            graphicsContext.SpriteBatch.Begin();
            const float FadeAlpha = 0.75f;

            if (_currentScreen == 0 || _currentScreen == 1)
            {
                // background
                graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Tutorial/Background1"));

                if (_currentScreen == 0)
                {
                    graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Tutorial/Fade1"), Color.White * FadeAlpha);
                    graphicsContext.SpriteBatch.DrawStringCentered(font, "Here is your life, score and weapon", new Vector2(graphicsContext.ScreenSize.Width / 2f + 64, 64), Color.White);
                }
                else
                {
                    graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Tutorial/Fade2"), Color.White * FadeAlpha);
                    graphicsContext.SpriteBatch.DrawStringCentered(font, "These are thumbsticks that are used for controlling", new Vector2(graphicsContext.ScreenSize.Width / 2f, 184), Color.White);
                }
            }
            else if (_currentScreen == 2 || _currentScreen == 3)
            {
                // background
                graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Tutorial/Background2"));

                if (_currentScreen == 2)
                {
                    graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Tutorial/Fade3"), Color.White * FadeAlpha);
                    graphicsContext.SpriteBatch.DrawStringCentered(font, "This drop gives you an extra life", new Vector2(graphicsContext.ScreenSize.Width / 4f + 72, 148), Color.White);
                    graphicsContext.SpriteBatch.DrawStringCentered(font, "This drop grants you a weapon", new Vector2(graphicsContext.ScreenSize.Width / 4f * 3 - 96, 388), Color.White);
                }
                else
                {
                    graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Tutorial/Fade4"), Color.White * FadeAlpha);
                    graphicsContext.SpriteBatch.DrawStringCentered(font, "This is a booster that changes elements in the game", new Vector2(graphicsContext.ScreenSize.Width / 2f, 224), Color.White);
                }
            }

            graphicsContext.SpriteBatch.DrawStringCentered(font, "Tap to continue", new Vector2(graphicsContext.ScreenSize.Width / 2f, 460), Color.White * 0.2f);

            graphicsContext.SpriteBatch.End();
        }

        private void LoadMainMenu()
        {
            this.Exited += () => this.ScreenManager.LoadScreen(new MenuBackgroundScreen(), new MainMenuScreen(FadeDirection.None));
            this.ExitScreen();
        }

        private void LoadGameplayScreen()
        {

            this.Exited += () => this.ScreenManager.LoadScreen(new GameplayScreen());
            this.ExitScreen();
        }
    }
}
