using Flai;
using Flai.Achievements;
using Flai.DataStructures;
using Flai.General;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Skypiea.Achievements;
using Skypiea.Stats;
using Skypiea.Ui;
using Skypiea.View;
using System;

namespace Skypiea.Screens
{
    public class AchievementScreen : GameScreen
    {
        private const float SlotHeight = 120;
        private const float TextOffsetFromBorder = 8;
        private const float OffsetFromTop = 206;

        private readonly AchievementManager _achievementManager = AchievementHelper.CreateAchivementManager();
        private readonly ReadOnlyArray<Achievement> _achievements;
        private readonly Scroller _scroller = new Scroller(Flai.Range.Zero, Alignment.Vertical);
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        private bool _needsSaving = false;

        public override bool IsPopup
        {
            get { return true; }
        }

        public AchievementScreen()
        {
            // get achievements with only one achievements per group
            _achievements = new ReadOnlyArray<Achievement>(_achievementManager.GetAchievementsWithoutGroups());
            _scroller.ScrollingRange = new Flai.Range(0, _achievements.Count * SlotHeight - Screen.Height * 0.5f);

            this.EnabledGestures = GestureType.Flick;
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeType = FadeType.FadeAlpha;
            this.FadeIn = FadeDirection.Left;
            this.FadeOut = FadeDirection.Left;
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

            _scroller.Update(updateContext);
            _uiContainer.Update(updateContext);
            if (updateContext.InputState.IsBackButtonPressed)
            {
                if (_needsSaving)
                {
                    _achievementManager.Save();
                }

                this.ExitScreen();
                this.ScreenManager.AddScreen(new MainMenuScreen(FadeDirection.Right), new Delay(0.25f));
            }
        }

        #region Draw

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.LinearClamp, _scroller.GetTransformMatrix(graphicsContext.ScreenSize) * SkypieaViewConstants.RenderScaleMatrix);
            this.DrawTitle(graphicsContext);
            this.DrawStats(graphicsContext);
            this.DrawAchievements(graphicsContext);
            graphicsContext.SpriteBatch.End();

            graphicsContext.SpriteBatch.Begin(SamplerState.LinearClamp, SkypieaViewConstants.RenderScaleMatrix);
            _uiContainer.Draw(graphicsContext, true);
            graphicsContext.SpriteBatch.End();
        }

        private void DrawTitle(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.32"], "Achievements", new Vector2(graphicsContext.ScreenSize.Width / 2f, 50));
        }

        private void DrawStats(GraphicsContext graphicsContext)
        {
            StatsManager statsManager = this.Game.Services.Get<StatsManager>();

            GraphicalGuidelines.DecimalPrecisionInText.Push(1);
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer.DefaultFont, "Kills: ", statsManager.Kills, new Vector2(graphicsContext.ScreenSize.Width / 2f, 110), Color.White);
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer.DefaultFont, "Time played: ", statsManager.TimePlayedInMinutes, " min", new Vector2(graphicsContext.ScreenSize.Width / 2f, 146), Color.White);
            graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer.DefaultFont, "Distance run: ", (int)statsManager.TotalMovement, " meters", new Vector2(graphicsContext.ScreenSize.Width / 2f, 182), Color.White);
            GraphicalGuidelines.DecimalPrecisionInText.Pop();
        }

        private void DrawAchievements(GraphicsContext graphicsContext)
        {
            RectangleF scrollerArea = _scroller.GetArea(graphicsContext.ScreenSize);
            int topVisible = (int)FlaiMath.Clamp((scrollerArea.Top - OffsetFromTop) / SlotHeight, 0, _achievements.Count);
            int bottomVisible = (int)FlaiMath.Clamp(FlaiMath.Ceiling((scrollerArea.Bottom - OffsetFromTop) / SlotHeight), 0, _achievements.Count);

            for (int i = topVisible; i < bottomVisible; i++)
            {
                float verticalPosition = OffsetFromTop + i * AchievementScreen.SlotHeight;

                Achievement achievement = _achievements[i];
                RectangleF slotArea = new RectangleF(0, verticalPosition, graphicsContext.ScreenSize.Width, SlotHeight);

                this.DrawSlotBackground(graphicsContext, achievement, slotArea);
                this.DrawAchievementInfo(graphicsContext, achievement, slotArea);
                this.DrawProgression(graphicsContext, achievement, slotArea);
                this.DrawGroupIndex(graphicsContext, achievement, slotArea);
            }
        }

        private void DrawSlotBackground(GraphicsContext graphicsContext, Achievement achievement, RectangleF slotArea)
        {
            Color color = achievement.IsUnlocked ? new Color(32, 64, 32) : new Color(64, 32, 32);
            graphicsContext.PrimitiveRenderer.DrawRectangle(slotArea, color * 0.25f);
            graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(slotArea, Color.Black, 2);
        }

        private void DrawAchievementInfo(GraphicsContext graphicsContext, Achievement achievement, RectangleF slotArea)
        {
            AchievementInfo achievementInfo = (AchievementInfo)achievement.Tag;
            SpriteFont nameFont = graphicsContext.FontContainer["Minecraftia.24"];
            SpriteFont descriptionFont = graphicsContext.FontContainer["Minecraftia.16"];

            Vector2 topLeftPosition = new Vector2(AchievementScreen.TextOffsetFromBorder, slotArea.Top + TextOffsetFromBorder);
            graphicsContext.SpriteBatch.DrawString(nameFont, achievement.Name, topLeftPosition, Color.White);
            graphicsContext.SpriteBatch.DrawString(descriptionFont, achievement.Description, topLeftPosition + Vector2.UnitY * nameFont.GetCharacterHeight() * 0.75f, Color.White);
            graphicsContext.SpriteBatch.DrawString(descriptionFont, "Reward: ", achievementInfo.RewardPassiveSkill.Description, topLeftPosition + Vector2.UnitY * nameFont.GetCharacterHeight() * 1.5f, Color.White);
        }

        private void DrawProgression(GraphicsContext graphicsContext, Achievement achievement, RectangleF slotArea)
        {
            if (achievement.Progression is PercentableAchievementProgression)
            {
                SpriteFont font = graphicsContext.FontContainer["Minecraftia.16"];
                PercentableAchievementProgression progression = (PercentableAchievementProgression)achievement.Progression;
                graphicsContext.SpriteBatch.DrawString(font, progression.ProgressionDisplayString, new Vector2(graphicsContext.ScreenSize.Width - TextOffsetFromBorder, slotArea.Y + TextOffsetFromBorder + font.GetCharacterHeight() * 0.4f), Corner.TopRight, Color.White);
            }
        }

        private void DrawGroupIndex(GraphicsContext graphicsContext, Achievement achievement, RectangleF slotArea)
        {
            if (_achievementManager.IsPartOfAnyGroup(achievement))
            {
                AchievementGroup achievementGroup = _achievementManager.GetGroupOf(achievement);
                int index = achievementGroup.IndexOf(achievement) + (achievementGroup.CurrentAchievement.IsUnlocked ? 1 : 0);

                graphicsContext.SpriteBatch.DrawString(graphicsContext.FontContainer["Minecraftia.12"], index, "/", achievementGroup.TotalCount, slotArea.BottomRight - Vector2.One * 8, Corner.BottomRight, Color.Gray);
            }
        }

        #endregion

        #region Create UI

        private void CreateUI()
        {
            this.CreateTogglePassiveSkillButtons();
        }

        private void CreateTogglePassiveSkillButtons()
        {
            Color enabledColor = Color.White;
            Color disabledColor = Color.Gray;

            for (int i = 0; i < _achievements.Count; i++)
            {
                Achievement achievement = _achievements[i];
                if (achievement.IsUnlocked)
                {
                    AchievementInfo achievementInfo = (AchievementInfo)achievement.Tag;
                    if (achievementInfo.RewardPassiveSkill is ToggleablePassiveSkill)
                    {
                        ToggleablePassiveSkill passiveSkill = (ToggleablePassiveSkill)achievementInfo.RewardPassiveSkill;

                        float verticalPosition = AchievementScreen.OffsetFromTop + (i + 0.5f) * AchievementScreen.SlotHeight;
                        ScrollerToggleButton toggleButton = new ScrollerToggleButton(RectangleF.CreateCentered(new Vector2(this.Game.ScreenSize.Width - 90, verticalPosition), new SizeF(160, 50)), "Enabled", "Disabled", _scroller)
                        {
                            DrawOutlines = true,
                            IsToggled = passiveSkill.IsEnabled,
                            Color = passiveSkill.IsEnabled ? enabledColor : disabledColor,
                        };

                        toggleButton.Toggled += isToggled =>
                        {
                            passiveSkill.IsEnabled = isToggled;
                            toggleButton.Color = isToggled ? enabledColor : disabledColor;

                            _needsSaving = true;
                        };

                        _uiContainer.Add(toggleButton);
                    }
                }
            }
        }

        #endregion
    }
}
