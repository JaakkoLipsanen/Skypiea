using Flai;
using Flai.Graphics;
using Flai.Misc;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Leaderboards;
using Skypiea.Misc;
using Skypiea.Settings;
using System;

namespace Skypiea.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        public override bool IsPopup
        {
            get { return true; }
        }

        public MainMenuScreen(FadeDirection fadeIn)
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeType = FadeType.FadeAlpha;
            this.FadeIn = fadeIn;
            this.FadeOut = FadeDirection.Right;

            HighscoreHelper.ResubmitHighscoreIfNeeded();
        }

        protected override void LoadContent(bool instancePreserved)
        {
            if (instancePreserved)
            {
                return;
            }

            this.CreateUI();
            RateHelper.ShowRateMessageIfNeeded();
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
            graphicsContext.SpriteBatch.Begin(SamplerState.LinearClamp);
            _uiContainer.Draw(graphicsContext, true);
            graphicsContext.SpriteBatch.End();
        }

        private void CreateUI()
        {
            _uiContainer.Add(new TextButton("Play", new Vector2(Screen.Width / 2f, 140), this.OnPlayClicked) { InflateAmount = -0, Font = "Minecraftia.20" });
            _uiContainer.Add(new TextButton("Achievements", new Vector2(Screen.Width / 2f, 205), this.OnAchievementsClicked) { InflateAmount = -0, Font = "Minecraftia.20" });
            _uiContainer.Add(new TextButton("Leaderboards", new Vector2(Screen.Width / 2f, 270), this.OnLeaderboardsClicked) { InflateAmount = -0, Font = "Minecraftia.20" });
            _uiContainer.Add(new TextButton("Options", new Vector2(Screen.Width / 2f, 335), this.OnOptionsClicked) { InflateAmount = -0, Font = "Minecraftia.20" });
            _uiContainer.Add(new TextButton("Exit", new Vector2(Screen.Width / 2f, 400), this.OnExitClicked) { InflateAmount = -0, Font = "Minecraftia.20" });

            _uiContainer.Add(new TextButton("Rate", new Vector2(Screen.Width - 44, Screen.Height - 24), this.OnRateClicked) { InflateAmount = 48, Font = "Minecraftia.20" });
            _uiContainer.Add(new TextButton("More Games", new Vector2(110, Screen.Height - 24), this.OnMoreGamesClicked) { InflateAmount = 48, Font = "Minecraftia.20" });
            _uiContainer.Add(new TextButton("Help", new Vector2(Screen.Width - 44, 24), this.OnHelpClicked) { InflateAmount = 48, Font = "Minecraftia.20" });

            _uiContainer.Add(new TextBlock("SKYPIEA", new Vector2(56, 18)) { Color = Color.White * 0.15f, Font = "SegoeWP.16" });
            _uiContainer.Add(new TextBlock(ApplicationInfo.ShortVersion, new Vector2(24, 40)) { Color = Color.White * 0.15f, Font = "SegoeWP.16" });
        }

        private void OnPlayClicked()
        {
            if (this.IsExiting)
            {
                return;
            }

            this.FadeOut = FadeDirection.Up;

            SkypieaSettingsManager settings = this.Game.Services.Get<SkypieaSettingsManager>();
            if (settings.Settings.IsFirstHelpShown)
            {
                this.ScreenManager.LoadScreen(new GameplayScreen());
            }
            else
            {
                this.ScreenManager.LoadScreen(new HelpScreen(true));
            }
        }

        private void OnAchievementsClicked()
        {
            if (!this.IsExiting)
            {
                this.FadeOut = FadeDirection.Right;
                this.ExitScreen();
                this.ScreenManager.AddScreen(new AchievementScreen(), new Delay(0.25f));
            }
        }

        private void OnLeaderboardsClicked()
        {
            if (!this.IsExiting)
            {
                this.FadeOut = FadeDirection.Left;
                this.ExitScreen();
                this.ScreenManager.AddScreen(new LeaderboardScreen(), new Delay(0.25f));
            }
        }

        private void OnOptionsClicked()
        {
            if (!this.IsExiting)
            {
                this.FadeOut = FadeDirection.Down;
                this.ExitScreen();
                this.ScreenManager.AddScreen(new OptionsScreen(), new Delay(0.25f));
            }
        }

        private void OnExitClicked()
        {
            if (!this.IsExiting)
            {
                this.Game.Exit();
            }
        }

        private void OnHelpClicked()
        {
            this.FadeOut = FadeDirection.None;
            this.ScreenManager.LoadScreen(new HelpScreen(false));
        }

        private void OnRateClicked()
        {
            if (!this.IsExiting)
            {
                ApplicationInfo.OpenApplicationReviewPage();
            }
        }

        private void OnMoreGamesClicked()
        {
            if (!this.IsExiting)
            {
                ApplicationInfo.OpenDeveloperApplicationList();
            }
        }
    }
}
