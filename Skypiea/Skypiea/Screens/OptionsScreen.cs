using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.Misc;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Settings;
using System;

namespace Skypiea.Screens
{
    public class OptionsScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        private readonly SkypieaSettingsManager _settingsManager;

        public override bool IsPopup
        {
            get { return true; }
        }

        public OptionsScreen()
        {
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            this.FadeType = FadeType.FadeAlpha;
            this.FadeIn = FadeDirection.Up;
            this.FadeOut = FadeDirection.Up;

            _settingsManager = FlaiGame.Current.Services.Get<SkypieaSettingsManager>();
        }

        protected override void LoadContent(bool instancePreserved)
        {
            this.CreateUI();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsExiting)
            {
                return;
            }

            _uiContainer.Update(updateContext);
            if (updateContext.InputState.IsBackButtonPressed)
            {
                _settingsManager.SaveToFile();
                this.ExitScreen();
                this.ScreenManager.AddScreen(new MainMenuScreen(FadeDirection.Down), new Delay(0.25f));
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);
            _uiContainer.Draw(graphicsContext, true);
            graphicsContext.SpriteBatch.End();
        }

        private void CreateUI()
        {
            _uiContainer.Add(new TextBlock("Thumbstick style", new Vector2(this.Game.ScreenSize.Width / 2f, 180)) { Font = "Minecraftia.32" });
            _uiContainer.Add(new TextMultiToggleButton<ThumbstickStyle>(
                RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 230), new SizeF(100, 50)), 
                EnumHelper.GetValues<ThumbstickStyle>(), 
                EnumHelper.GetName,
                this.OnThumbstickStyleChanged) { Font = "Minecraftia.24" }).SetSelectedValue(_settingsManager.Settings.ThumbstickStyle);
        }

        private void OnThumbstickStyleChanged(ThumbstickStyle thumbstickStyle)
        {
            _settingsManager.Settings.ThumbstickStyle = thumbstickStyle;
        }
    }
}
