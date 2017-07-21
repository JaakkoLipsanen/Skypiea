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
        private const float AchievementShowTime = 3f;

        private readonly Timer _achievementShowTimer = new Timer(AchievementRenderer.AchievementShowTime);
        private Achievement _currentAchievement;

        public AchievementRenderer(EntityWorld entityWorld)
        {
            entityWorld.Services.Get<IAchievementManager>().AchievementUnlocked += this.OnAchievementUnlocked;
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
                const float Height = 96;
                const float VerticalPosition = 90;

                const float BackgroundAlpha = 0.3f;
                const float TextAlpha = 0.75f;

                float alpha = this.GetAlpha();

                SpriteFont font = graphicsContext.FontContainer["Minecraftia.16"];
                float maxWidth = FlaiMath.Max(font.GetStringWidth(_currentAchievement.Name), font.GetStringWidth(_currentAchievement.ShortDescription)) + TextOffset * 2;

                float left = graphicsContext.ScreenSize.Width - OffsetFromBorder - maxWidth + (1 - alpha) * maxWidth / 2f;
                float centerX = left + maxWidth / 2f;
                RectangleF backgroundArea = new RectangleF(left, VerticalPosition, maxWidth, Height);

                // background & outlines
                graphicsContext.PrimitiveRenderer.DrawRectangle(backgroundArea, Color.Black * BackgroundAlpha * alpha);
                graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(backgroundArea, Color.Black * TextAlpha * alpha, 1);

                // name
                graphicsContext.SpriteBatch.DrawStringCentered(
                    font, _currentAchievement.Name, new Vector2(centerX, VerticalPosition + TextOffset + font.GetCharacterHeight() / 2f), Color.White * alpha * TextAlpha);

                // description
                graphicsContext.SpriteBatch.DrawStringCentered(
                    font, _currentAchievement.ShortDescription, new Vector2(centerX, VerticalPosition + TextOffset * 2 + font.GetCharacterHeight() * 3 / 2f), Color.White * alpha * TextAlpha);
            }
        }

        private float GetAlpha()
        {
            const float FadeTime = 0.25f;
            return FlaiMath.Min(1,
                _achievementShowTimer.ElapsedTime / FadeTime, 
                (AchievementRenderer.AchievementShowTime - _achievementShowTimer.ElapsedTime) / FadeTime);       
        }

        private void OnAchievementUnlocked(Achievement achievement)
        {
            _currentAchievement = achievement;
            _achievementShowTimer.Restart();
        }
    }
}
