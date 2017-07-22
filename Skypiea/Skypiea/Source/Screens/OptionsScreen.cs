using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.IO;
using Flai.Misc;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Settings;
using System;
using Skypiea.View;

namespace Skypiea.Screens
{
    public class OptionsScreen : GameScreen
    {
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        private readonly SkypieaSettingsManager _settingsManager;
        private readonly ScoreloopManager _scoreloopManager;
        private TextBlock _renameResultTextBlock;

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
          _scoreloopManager = FlaiGame.Current.Services.Get<ScoreloopManager>();
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
                _settingsManager.Save();
                this.ExitScreen();
                this.ScreenManager.AddScreen(new MainMenuScreen(FadeDirection.Down), new Delay(0.25f));
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.LinearClamp, SkypieaViewConstants.RenderScaleMatrix);
            _uiContainer.Draw(graphicsContext, true);
            graphicsContext.SpriteBatch.End();
        }

        private void CreateUI()
        {
            // thumbstick style
            _uiContainer.Add(new TextBlock("Thumbstick style", new Vector2(this.Game.ScreenSize.Width / 2f, 180)) { Font = "Minecraftia.32", Color = Color.Gray });
            _uiContainer.Add(new TextMultiToggleButton<ThumbstickStyle>(
                RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 230), new SizeF(100, 50)),
                EnumHelper.GetValues<ThumbstickStyle>(),
                EnumHelper.GetName,
                this.OnThumbstickStyleChanged) { Font = "Minecraftia.24" }).SetSelectedValue(_settingsManager.Settings.ThumbstickStyle);

            // rename scoreloop name
            _uiContainer.Add(new TextButton("Rename leaderboard username", new Vector2(this.Game.ScreenSize.Width / 2f, 300), this.OnRenameUserNameClicked) { Font = "Minecraftia.24", InflateAmount = 12 });
            _uiContainer.Add(_renameResultTextBlock = new TextBlock("", new Vector2(this.Game.ScreenSize.Width / 2f, 330)) { Visible = false, Font = "Minecraftia.16" });

            // contacxt
            _uiContainer.Add(new TextBlock("Contact/Feedback", new Vector2(this.Game.ScreenSize.Width / 2f, 400)) { Font = "Minecraftia.20", Color = Color.Gray });
            _uiContainer.Add(new TextButton("jaakko.lipsanen@outlook.com", new Vector2(this.Game.ScreenSize.Width / 2f, 435) /*, () => // TODO: https://developer.xamarin.com/recipes/android/networking/email/send_an_email/
            {
                if (this.IsExiting || Guide.IsVisible)
                {
                    return;
                }

                EmailComposeTask composeTask = new EmailComposeTask { Subject = "Feedback for Final Fight Z", To = "jaakko.lipsanen@outlook.com" };
                composeTask.Show();
            } */) { Font = "Minecraftia.16" });

            // show graphical quality only on WP7
            if (OperatingSystemHelper.Version == WindowsPhoneVersion.WP7)
            {
                _uiContainer.Add(new TextBlock("Graphical quality", new Vector2(this.Game.ScreenSize.Width / 2f, 70)) { Font = "Minecraftia.32", Color = Color.Gray });
                _uiContainer.Add(new TextMultiToggleButton<GraphicalQuality>(
                    RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width / 2f, 120), new SizeF(100, 50)),
                    EnumHelper.GetValues<GraphicalQuality>(),
                    EnumHelper.GetName,
                    this.OnGraphicalQualityChanged) { Font = "Minecraftia.24" }).SetSelectedValue(_settingsManager.Settings.GraphicalQuality);
            }
        }

        private void OnThumbstickStyleChanged(ThumbstickStyle thumbstickStyle)
        {
            _settingsManager.Settings.ThumbstickStyle = thumbstickStyle;
        }

        private void OnGraphicalQualityChanged(GraphicalQuality graphicalQuality)
        {
            _settingsManager.Settings.GraphicalQuality = graphicalQuality;
        }

        private void OnRenameUserNameClicked()
        {
            GuideHelper.ShowKeyboardInput("Change username", "Change the username that will be used on the global leaderboards", input =>
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (input.Length < 4)
                    {
                        this.SetRenameErrorText("Username is too short");
                    }
                    else if (input.Length > 26) // scoreloop limit is 40 characters, but 26 is the last one that fits properly to the leaderboards
                    {
                        this.SetRenameErrorText("Username is too long");
                    }
                  else if (!_scoreloopManager.IsNetworkAvailable)
                    {
                        this.SetRenameErrorText("No network available. Try again later");
                    }
                    else
                    {
                        _renameResultTextBlock.Visible = false;
                        _scoreloopManager.RenameUser(input, r => { }); //  this.OnUserRenamed); // TODO
                    }
                }
            });
        }

        /*
        private void OnUserRenamed(ScoreloopResponse response)
        {
            if (response.Success)
            {
                this.SetRenameSuccessText("Username updated!");
            }
            else
            {
                this.SetRenameErrorText(response.Error.ErrorCode == ErrorCode.UserInvalidArguments ? "Username is already taken" : "Something went wrong. Try again later");
            } 
        }
        */

        private void SetRenameSuccessText(string text)
        {
            const float ResultTextBlockAlpha = 0.5f;

            _renameResultTextBlock.Visible = true;
            _renameResultTextBlock.Color = Color.Green * ResultTextBlockAlpha;
            _renameResultTextBlock.Text = "Username updated!";
        }

        private void SetRenameErrorText(string text)
        {
            const float ResultTextBlockAlpha = 0.5f;

            _renameResultTextBlock.Visible = true;
            _renameResultTextBlock.Color = Color.Red * ResultTextBlockAlpha;
            _renameResultTextBlock.Text = text;
        }
    }
}
