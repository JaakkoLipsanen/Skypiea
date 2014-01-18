using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;

namespace Skypiea.View
{
    public class DropRenderer : ComponentProcessingRenderer<CDrop>
    {
        public DropRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.Any<CDrop>())
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity, CDrop drop)
        {
            if (!this.IsVisible(graphicsContext, drop))
            {
                return;
            }

            if (drop.DropType == DropType.Weapon)
            {
                this.DrawWeaponDrop(graphicsContext, entity);
            }
            else if (drop.DropType == DropType.Life)
            {
                this.DrawLifeDrop(graphicsContext, entity);
            }
            else if (drop.DropType == DropType.BlackBox)
            {
                this.DrawBlackBox(graphicsContext, entity);
            }
        }

        private bool IsVisible(GraphicsContext graphicsContext, CDrop drop)
        {
            const float MaximumDropRadius = 160;
            RectangleF dropArea = RectangleF.CreateCentered(drop.Transform.Position, MaximumDropRadius);
            if (CCamera2D.Active.GetArea(graphicsContext.ScreenSize).Intersects(dropArea))
            {
                return true;
            }

            return false;
        }

        private void DrawWeaponDrop(GraphicsContext graphicsContext, Entity entity)
        {
            this.DrawFans(graphicsContext, entity.Transform.Position, Color.White * 0.5f, 0.3f);
            if (DropHelper.IsBlinking(entity.Get<CLifeTime>()))
            {
                return;
            }

            const float Size = 30;
            const string FontName = "Minecraftia.16";

            CWeaponDrop weaponDrop = entity.Get<CWeaponDrop>();
            Vector2i position = Vector2i.Round(entity.Transform.Position);

            graphicsContext.PrimitiveRenderer.DrawRectangle(position, Size, Color.White);
            graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(RectangleF.CreateCentered(position, Size), Color.Black, 4);
            graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer[FontName], weaponDrop.Type.ToChar(), position, Color.White, Color.Black, 0, 1f);
        }

        private void DrawLifeDrop(GraphicsContext graphicsContext, Entity entity)
        {
            this.DrawFans(graphicsContext, entity.Transform.Position, Color.Lerp(Color.Red, Color.PaleVioletRed, 0.5f) * 0.5f, 0.5f);
            if (DropHelper.IsBlinking(entity.Get<CLifeTime>()))
            {
                return;
            }

            // heart
            const float BaseScale = SkypieaViewConstants.PixelSize * 1.5f;
            float scale = BaseScale * (1f + FlaiMath.Sin(_entityWorld.TotalUpdateTime * 2f) * 0.1f);
            graphicsContext.SpriteBatch.DrawCentered(SkypieaViewConstants.LoadTexture(_contentProvider, "Life"), entity.Transform.Position, Color.White, 0f, scale);
        }

        private void DrawBlackBox(GraphicsContext graphicsContext, Entity entity)
        {
            Color color = graphicsContext.TotalSeconds % 0.5f > 0.25f ? Color.White : Color.Black;
            this.DrawFans(graphicsContext, entity.Transform.Position, color * 0.5f, 0.5f);

            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, entity.Transform.Position, Color.Black, graphicsContext.TotalSeconds * 3, 32 * (1f + FlaiMath.Sin(graphicsContext.TotalSeconds * 10f) * 0.5f));
            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, entity.Transform.Position, Color.White, 2 + graphicsContext.TotalSeconds * 3, 16 * (1f + FlaiMath.Sin(FlaiMath.Pi + graphicsContext.TotalSeconds * 10f) * 0.5f));
        }

        private void DrawFans(GraphicsContext graphicsContext, Vector2 position, Color color, float scale)
        {
            TextureDefinition fanTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "LifeDropFan");
            float fanRotation = _entityWorld.TotalUpdateTime;

            const int FanCount = 6;
            for (int i = 0; i < FanCount; i++)
            {
                float rotation = FlaiMath.TwoPi / FanCount * i + fanRotation;
                graphicsContext.SpriteBatch.Draw(fanTexture, position, color, rotation, new Vector2(fanTexture.Width / 2f, fanTexture.Height), scale);
            }
        }
    }
}
