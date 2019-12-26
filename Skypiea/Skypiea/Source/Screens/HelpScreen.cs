using System;
using Flai;
using Flai.Graphics;
using Flai.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Settings;
using Skypiea.Source.Screens;
using Skypiea.View;

namespace Skypiea.Screens
{
    public class HelpScreen : GameScreen
    {
        private const int ScreenCount = 4;
        private const float FadeAlpha = 0.75f;

        private readonly bool _isPlay;
        private int _currentScreen = 0;

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
                this.AdvanceToNextScreen();
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp, SkypieaViewConstants.RenderScaleMatrix);

            // if you make more screens, having something more flexible could be nice
            if (_currentScreen == 0)
            {
                this.DrawScreen1(graphicsContext);
            }
            else if (_currentScreen == 1)
            {
                this.DrawScreen2(graphicsContext);
            }
            else if (_currentScreen == 2)
            {
                this.DrawScreen3(graphicsContext);
            }
            else if (_currentScreen == 3)
            {
                this.DrawScreen4(graphicsContext);
            }

            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.16"], "Tap to continue", new Vector2(graphicsContext.ScreenSize.Width / 2f, 460), Color.White * 0.2f);
            graphicsContext.SpriteBatch.End();
        }

        private void DrawScreen1(GraphicsContext graphicsContext)
        {
            this.DrawFullScreen(graphicsContext, "Tutorial/Background1");
            this.DrawFullScreen(graphicsContext, "Tutorial/Fade1", Color.White * HelpScreen.FadeAlpha);
            this.DrawText(graphicsContext, "Here is your life, score and weapon", new Vector2(graphicsContext.ScreenSize.Width / 2f + 64, 64));
        }

        private void DrawScreen2(GraphicsContext graphicsContext)
        {
            this.DrawFullScreen(graphicsContext, "Tutorial/Background1");
            this.DrawFullScreen(graphicsContext, "Tutorial/Fade2", Color.White * HelpScreen.FadeAlpha);
            this.DrawText(graphicsContext, "These are thumbsticks that are used for controlling", new Vector2(graphicsContext.ScreenSize.Width / 2f, 184));
        }

        private void DrawScreen3(GraphicsContext graphicsContext)
        {
            this.DrawFullScreen(graphicsContext, "Tutorial/Background2");
            this.DrawFullScreen(graphicsContext, "Tutorial/Fade3", Color.White * HelpScreen.FadeAlpha);
            this.DrawText(graphicsContext, "This drop gives you an extra life", new Vector2(graphicsContext.ScreenSize.Width / 4f + 72, 148));
            this.DrawText(graphicsContext, "This drop grants you a weapon", new Vector2(graphicsContext.ScreenSize.Width / 4f * 3 - 96, 388));
        }

        private void DrawScreen4(GraphicsContext graphicsContext)
        {
            this.DrawFullScreen(graphicsContext, "Tutorial/Background2");
            this.DrawFullScreen(graphicsContext, "Tutorial/Fade4", Color.White * HelpScreen.FadeAlpha);
            this.DrawText(graphicsContext, "This is a booster that changes elements in the game", new Vector2(graphicsContext.ScreenSize.Width / 2f, 224));
        }

        private void DrawText(GraphicsContext graphicsContext, string text, Vector2 position)
        {
            SpriteFont font = graphicsContext.FontContainer["Minecraftia.16"];
            graphicsContext.SpriteBatch.DrawStringCentered(font, text, position, Color.White);
        }

        private void DrawFullScreen(GraphicsContext graphicsContext, string textureName)
        {
            this.DrawFullScreen(graphicsContext, textureName, Color.White);
        }

        private void DrawFullScreen(GraphicsContext graphicsContext, string textureName, Color color)
        {
            Texture2D texture = graphicsContext.ContentProvider.DefaultManager.LoadTexture(textureName);
            graphicsContext.SpriteBatch.Draw(texture, Vector2.Zero, color, 0, 
                new Vector2(graphicsContext.GraphicsDevice.PresentationParameters.BackBufferWidth, graphicsContext.GraphicsDevice.PresentationParameters.BackBufferHeight) / 
                new Vector2(texture.Width, texture.Height) / 
                SkypieaViewConstants.RenderScale);
        }

        private void AdvanceToNextScreen()
        {
            if (_currentScreen == HelpScreen.ScreenCount - 1)
            {
                this.FadeType = FadeType.FadeToBlack;
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
            else
            {
                _currentScreen++;
            }
        }

        private void LoadMainMenu()
        {
            this.Exited += () => this.ScreenManager.LoadScreen(new MenuBackgroundScreen(), new MainMenuScreen(FadeDirection.None));
            this.ExitScreen();
        }

        private void LoadGameplayScreen()
        {

            this.Exited += () => this.ScreenManager.LoadScreen(new ChangeNavigationBarVisibilityScreen(new GameplayScreen(), false));
            this.ExitScreen();
        }
    }
}
