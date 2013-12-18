using System;
using System.Linq;
using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.ScreenManagement.Screens;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Skypiea.Achievements;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;
using Skypiea.View;

namespace Skypiea.Screens
{
    public class MainMenuScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        private TextMultiToggleButton<WorldType> _worldTypeToggleButton;
        private TextMultiToggleButton<WeaponType> _weaponTypeToggleButton;
        private TextToggleButton _zombiesToggleButton;
        private TextToggleButton _graphicsToggleButton;

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

            _uiContainer.Add(new TextButton("Play", new Vector2(this.Game.ScreenSize.Width / 2f, 200), this.OnPlayClicked) { InflateAmount = -8 });
            _uiContainer.Add(new TextButton("Achievements", new Vector2(this.Game.ScreenSize.Width / 2F, 260), this.OnAchievementsClicked) { InflateAmount = -8 });
            _uiContainer.Add(new TextButton("Leaderboards", new Vector2(this.Game.ScreenSize.Width / 2F, 320), this.OnLeaderboardsClicked) { InflateAmount = -8 });
            _uiContainer.Add(new TextButton("Exit", new Vector2(this.Game.ScreenSize.Width / 2F, 380), this.OnExitClicked) { InflateAmount =  -8 });

            _uiContainer.Add(_worldTypeToggleButton = new TextMultiToggleButton<WorldType>(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 440), 150), EnumHelper.GetValues<WorldType>().ToArray()) { Font = "SegoeWP.24" });
            _uiContainer.Add(_zombiesToggleButton = new TextToggleButton(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 4f, 440), 150), "All zombies", "Only Rushers") { Font = "SegoeWP.24" });

            _uiContainer.Add(_weaponTypeToggleButton = new TextMultiToggleButton<WeaponType>(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 40), 150), EnumHelper.GetValues<WeaponType>().ToArray()) { Font = "SegoeWP.24" });
            _uiContainer.Add(_graphicsToggleButton = new TextToggleButton(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 4f * 3f, 40), 150), "Low", "High") { Font = "SegoeWP.24", IsToggled = false });

            _uiContainer.Add(new TextButton("Reset Achievements", new Vector2(this.Game.ScreenSize.Width / 4f * 3 + 30, 440), AchievementHelper.ResetAchievements) { Font = "SegoeWP.24" });
        }

        private void OnPlayClicked()
        {
            TestingGlobals.SpawnOnlyRushers = !_zombiesToggleButton.IsToggled;
            TestingGlobals.DefaultWeaponType = _weaponTypeToggleButton.SelectedValue;
            Settings.Current.GraphicalQuality = _graphicsToggleButton.IsToggled ? GraphicalQuality.Low : GraphicalQuality.High;

            this.ScreenManager.LoadScreen(new GameplayScreen(_worldTypeToggleButton.SelectedValue));
        }

        private void OnAchievementsClicked()
        {
            this.ScreenManager.LoadScreen(new AchievementScreen());
        }

        private void OnLeaderboardsClicked()
        {
            this.ScreenManager.LoadScreen(new LeaderboardScreen());
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
