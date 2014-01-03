using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.Misc;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Achievements;
using Skypiea.Leaderboards;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;
using Skypiea.View;
using System;

namespace Skypiea.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        private TextMultiToggleButton<WorldType> _worldTypeToggleButton;
        private TextMultiToggleButton<WeaponType> _weaponTypeToggleButton;
        private TextToggleButton _zombiesToggleButton;
        private TextMultiToggleButton<GraphicalQuality> _graphicsToggleButton;

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
            _uiContainer.Add(new TextBlock("SKYPIEA", new Vector2(56, 18)) { Color = Color.White * 0.15f, Font = "SegoeWP.16" });
            _uiContainer.Add(new TextBlock(ApplicationInfo.ShortVersion, new Vector2(24, 40)) { Color = Color.White * 0.15f, Font = "SegoeWP.16" });

            // DEBUG STUFF
            _uiContainer.Add(_worldTypeToggleButton = new TextMultiToggleButton<WorldType>(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 440), 150), EnumHelper.GetValues<WorldType>().ToArray(), EnumHelper.GetName) { Font = "SegoeWP.24", Tag = "D" });
            _uiContainer.Add(_zombiesToggleButton = new TextToggleButton(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 4f, 440), 150), "All zombies", "Only Rushers") { Font = "SegoeWP.24", Tag = "D" });

            _uiContainer.Add(_weaponTypeToggleButton = new TextMultiToggleButton<WeaponType>(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 40), 150), EnumHelper.GetValues<WeaponType>().ToArray(), EnumHelper.GetName) { Font = "SegoeWP.24", Tag = "D" });
            _uiContainer.Add(_graphicsToggleButton = new TextMultiToggleButton<GraphicalQuality>(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 4f * 3f, 40), 150), EnumHelper.GetValues<GraphicalQuality>().ToArray(), EnumHelper.GetName) { Font = "SegoeWP.24", Tag = "D" });

            _uiContainer.Add(new TextButton("Reset Achievements", new Vector2(this.Game.ScreenSize.Width / 4f * 3 + 30, 440), AchievementHelper.ResetAchievements) { Font = "SegoeWP.24", Tag = "D" });
            _uiContainer.Add(new TextToggleButton(RectangleF.CreateCentered(new Vector2(Screen.Width - 100, 30), 50), "Debug", "No debug", isToggled =>
            {
                TestingGlobals.Debug = isToggled;
                this.ScreenManager.Game.Components.Get<DebugInformationComponent>().Visible = isToggled;
                _uiContainer.FindAllWithTag("D").ForEach(uiObject => uiObject.Visible = isToggled);
            }) { IsToggled = true }).ManualClick();

            
            _graphicsToggleButton.SetSelectedValue(TestingGlobals.GraphicalQuality);
            _graphicsToggleButton.Click += () => TestingGlobals.GraphicalQuality = _graphicsToggleButton.SelectedValue;
        }

        private void OnPlayClicked()
        {
            if (this.IsExiting)
            {
                return;
            }

            TestingGlobals.SpawnOnlyRushers = !_zombiesToggleButton.IsToggled;
            TestingGlobals.DefaultWeaponType = _weaponTypeToggleButton.SelectedValue;
            TestingGlobals.GraphicalQuality = _graphicsToggleButton.SelectedValue;

            this.FadeOut = FadeDirection.Up;
            this.ScreenManager.LoadScreen(new GameplayScreen(_worldTypeToggleButton.SelectedValue));
        }

        private void OnAchievementsClicked()
        {
            if (!this.IsExiting)
            {
                // this.Exited += () => this.ScreenManager.AddScreen(new AchievementScreen());
                this.FadeOut = FadeDirection.Right;
                this.ExitScreen(); 
                this.ScreenManager.AddScreen(new AchievementScreen(), new Delay(0.25f));
            }
        }

        private void OnLeaderboardsClicked()
        {
            if (!this.IsExiting)
            {
                // this.Exited += () => this.ScreenManager.AddScreen(new LeaderboardScreen());
                this.FadeOut = FadeDirection.Left;
                this.ExitScreen();

                this.ScreenManager.AddScreen(new LeaderboardScreen(), new Delay(0.25f));
            }
        }

        private void OnOptionsClicked()
        {
            if (!this.IsExiting)
            {
                // this.Exited += () => this.ScreenManager.AddScreen(new LeaderboardScreen());
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
