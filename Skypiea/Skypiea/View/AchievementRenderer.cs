using Flai;
using Flai.Achievements;
using Flai.CBES;
using Flai.General;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Systems;

namespace Skypiea.View
{
    public class AchievementRenderer : FlaiRenderer
    {
        private const float AchievementShowTime = 5f;

        private readonly EntityWorld _entityWorld;
        private readonly Timer _achievementShowTimer = new Timer(AchievementRenderer.AchievementShowTime);
        private Achievement _currentAchievement;

        public AchievementRenderer(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
            _entityWorld.Services.Get<IAchievementManager>().AchievementUnlocked += this.OnAchievementUnlocked;
        }

        protected override void UpdateInner(UpdateContext updateContext)
        {
            _achievementShowTimer.Update(updateContext);
            if (_currentAchievement != null && _achievementShowTimer.HasFinished)
            {
                _currentAchievement = null;
            }
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            if (_currentAchievement != null)
            {
                const float OffsetFromBorder = 8;
                const float TextOffset = 8;
                const float FadeTime = 0.25f;
                const float Height = 96;
                const float VerticalPosition = 128;

                float alpha = FlaiMath.Min(_achievementShowTimer.ElapsedTime / FadeTime, (AchievementRenderer.AchievementShowTime - _achievementShowTimer.ElapsedTime) / FadeTime, 1);

                SpriteFont font = graphicsContext.FontContainer["Minecraftia.16"];
                float width = FlaiMath.Max(font.GetStringWidth(_currentAchievement.Name), font.GetStringWidth(_currentAchievement.Description)) + TextOffset * 2;

                float left = graphicsContext.ScreenSize.Width - OffsetFromBorder - width + (1 - alpha) * width / 2f;
                float centerX = left + width / 2f;

                // background
                graphicsContext.PrimitiveRenderer.DrawRectangle(
                    new RectangleF(left, VerticalPosition, width, Height), Color.Black * 0.75f * alpha);

                // name
                graphicsContext.SpriteBatch.DrawStringCentered(
                    font, _currentAchievement.Name, new Vector2(centerX, VerticalPosition + TextOffset + font.GetCharacterHeight() / 2f), Color.White * alpha);

                // description
                graphicsContext.SpriteBatch.DrawStringCentered(
                    font, _currentAchievement.Description, new Vector2(centerX, VerticalPosition + TextOffset * 2 + font.GetCharacterHeight() * 3 / 2f), Color.White * alpha);
            }
        }

        private void OnAchievementUnlocked(Achievement achievement)
        {
            _currentAchievement = achievement;
            _achievementShowTimer.Restart();
        }
    }
}
