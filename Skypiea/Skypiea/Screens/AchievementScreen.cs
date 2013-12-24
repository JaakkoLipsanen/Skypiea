using System;
using Flai;
using Flai.Achievements;
using Flai.General;
using Flai.Graphics;
using Flai.ScreenManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Skypiea.Achievements;

namespace Skypiea.Screens
{
    public class AchievementScreen : GameScreen
    {
        private const float SlotHeight = 120;

        private readonly AchievementManager _achievementManager = AchievementHelper.CreateAchivementManager();
        private readonly Scroller _scroller = new Scroller(Range.Zero, Alignment.Vertical);

        public override bool IsPopup
        {
            get { return true; }
        }

        public AchievementScreen()
        {
            _scroller.ScrollingRange = new Range(0, _achievementManager.Achievements.Count * SlotHeight);

            this.EnabledGestures = GestureType.Flick;
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = false;
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsExiting)
            {
                return;
            }

            _scroller.Update(updateContext);
            if (updateContext.InputState.IsBackButtonPressed)
            {
                this.Exited += () => this.ScreenManager.AddScreen(new MainMenuScreen());
                this.ExitScreen();
            }
        }

        // todo: implement scroller "properly"
        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(SamplerState.PointClamp);
            graphicsContext.SpriteBatch.GlobalAlpha.Push(this.TransitionAlpha);

            const float OffsetFromTop = 128;
            const float TextOffsetFromBorder = 8;

            SpriteFont nameFont = graphicsContext.FontContainer["Minecraftia.24"];
            SpriteFont descriptionFont = graphicsContext.FontContainer["Minecraftia.16"];

            for (int i = 0; i < _achievementManager.Achievements.Count; i++)
            {
                float y = OffsetFromTop + i * AchievementScreen.SlotHeight - _scroller.ScrollValue;

                Achievement achievement = _achievementManager.Achievements[i];
                AchievementInfo achievementInfo = (AchievementInfo)achievement.Tag;

                Color color = achievement.IsUnlocked ? new Color(32, 192, 32) : new Color(255, 64, 64);
                RectangleF slotArea = new RectangleF(0, y, graphicsContext.ScreenSize.Width, SlotHeight);
                graphicsContext.PrimitiveRenderer.DrawRectangle(slotArea, color);
                graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(slotArea, Color.Black, 1);

                graphicsContext.SpriteBatch.DrawString(nameFont, achievement.Name, new Vector2(TextOffsetFromBorder, y + TextOffsetFromBorder), Color.White);
                graphicsContext.SpriteBatch.DrawString(descriptionFont, achievement.Description, new Vector2(TextOffsetFromBorder, y + TextOffsetFromBorder + nameFont.GetCharacterHeight() * 0.75f), Color.White);
                graphicsContext.SpriteBatch.DrawString(descriptionFont, "Reward: ", achievementInfo.RewardPassiveSkill.Description, new Vector2(TextOffsetFromBorder, y + TextOffsetFromBorder + nameFont.GetCharacterHeight() * 1.5f), Color.White);

                if (achievement.Progression is PercentableAchievementProgression)
                {
                    PercentableAchievementProgression progression = (PercentableAchievementProgression)achievement.Progression;
                    graphicsContext.SpriteBatch.DrawString(descriptionFont, progression.ProgressionDisplayString, new Vector2(graphicsContext.ScreenSize.Width - TextOffsetFromBorder, y + TextOffsetFromBorder + nameFont.GetCharacterHeight() * 0.4f), Corner.TopRight, Color.White);
                }
            }

            graphicsContext.SpriteBatch.GlobalAlpha.Pop();
            graphicsContext.SpriteBatch.End();
        }
    }
}
