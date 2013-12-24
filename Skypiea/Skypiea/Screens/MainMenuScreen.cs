using System.Collections.Generic;
using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Achievements;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;
using Skypiea.View;
using System;
using System.Linq;

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

        public MainMenuScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = false;
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
            _uiContainer.Add(new TextButton("Exit", new Vector2(this.Game.ScreenSize.Width / 2F, 380), this.OnExitClicked) { InflateAmount = -8 });

            _uiContainer.Add(_worldTypeToggleButton = new TextMultiToggleButton<WorldType>(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 440), 150), EnumHelper.GetValues<WorldType>().ToArray()) { Font = "SegoeWP.24" });
            _uiContainer.Add(_zombiesToggleButton = new TextToggleButton(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 4f, 440), 150), "All zombies", "Only Rushers") { Font = "SegoeWP.24" });

            _uiContainer.Add(_weaponTypeToggleButton = new TextMultiToggleButton<WeaponType>(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 40), 150), EnumHelper.GetValues<WeaponType>().ToArray()) { Font = "SegoeWP.24" });
            _uiContainer.Add(_graphicsToggleButton = new TextMultiToggleButton<GraphicalQuality>(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 4f * 3f, 40), 150), EnumHelper.GetValues<GraphicalQuality>().ToArray()) { Font = "SegoeWP.24" });

            _uiContainer.Add(new TextButton("Reset Achievements", new Vector2(this.Game.ScreenSize.Width / 4f * 3 + 30, 440), AchievementHelper.ResetAchievements) { Font = "SegoeWP.24" });

            _graphicsToggleButton.SetSelectedValue(Settings.Current.GraphicalQuality);
            _graphicsToggleButton.Click += () => Settings.Current.GraphicalQuality = _graphicsToggleButton.SelectedValue;
        }

        private void OnPlayClicked()
        {
            if (this.IsExiting)
            {
                return;
            }

            TestingGlobals.SpawnOnlyRushers = !_zombiesToggleButton.IsToggled;
            TestingGlobals.DefaultWeaponType = _weaponTypeToggleButton.SelectedValue;
            Settings.Current.GraphicalQuality = _graphicsToggleButton.SelectedValue;

            this.ScreenManager.LoadScreen(new GameplayScreen(_worldTypeToggleButton.SelectedValue));
        }

        private void OnAchievementsClicked()
        {
            if (!this.IsExiting)
            {
                this.Exited += () => this.ScreenManager.AddScreen(new AchievementScreen());
                this.ExitScreen();
            }
        }

        private void OnLeaderboardsClicked()
        {
            if (!this.IsExiting)
            {
                this.Exited += () => this.ScreenManager.AddScreen(new LeaderboardScreen());
                this.ExitScreen();
            }
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
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);
            graphicsContext.SpriteBatch.GlobalAlpha.Push(this.TransitionAlpha);
            _uiContainer.Draw(graphicsContext, true);
            graphicsContext.SpriteBatch.GlobalAlpha.Pop();
            graphicsContext.SpriteBatch.End();
        }
    }

    public class MenuBackgroundScreen : GameScreen
    {
        public MenuBackgroundScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = true;
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);
            graphicsContext.SpriteBatch.DrawFullscreen(this.ContentProvider.DefaultManager.LoadTexture("MainMenuBackground"), new Color(64, 64, 64));

            if (Settings.Current.GraphicalQuality == GraphicalQuality.High)
            {
                this.DrawNoise(graphicsContext);
                this.DrawVignette(graphicsContext);
            }

            graphicsContext.SpriteBatch.End();
        }

        private void DrawVignette(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Vignette")); // ...
            graphicsContext.SpriteBatch.DrawFullscreen(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Vignette")); // ...
        }

        private void DrawNoise(GraphicsContext graphicsContext)
        {
            Texture2D noiseTexture = this.ContentProvider.DefaultManager.LoadTexture("Noise");
            for (int x = Global.Random.Next(-noiseTexture.Width, 0); x < graphicsContext.ScreenSize.Width; x += noiseTexture.Width)
            {
                for (int y = Global.Random.Next(-360, 0); y < graphicsContext.ScreenSize.Height; y += noiseTexture.Height)
                {
                    graphicsContext.SpriteBatch.Draw(noiseTexture, new Vector2(x, y), Color.White * 0.05f);
                }
            }
        }
    }
}
