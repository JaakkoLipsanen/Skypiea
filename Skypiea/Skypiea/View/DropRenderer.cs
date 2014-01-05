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
            : base(entityWorld, Aspect.Any<CWeaponDrop, CLifeDrop>())
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
            else
            {
                this.DrawLifeDrop(graphicsContext, entity);
            }
        }

        private bool IsVisible(GraphicsContext graphicsContext, CDrop drop)
        {
            const float MaximumDropRadius = 160;
            if (CCamera2D.Active.GetArea(graphicsContext.ScreenSize).Intersects(RectangleF.CreateCentered(drop.Transform.Position, MaximumDropRadius)))
            {
                return true;
            }

            return false;
        }

        private void DrawWeaponDrop(GraphicsContext graphicsContext, Entity entity)
        {
            Vector2i position = Vector2i.Round(entity.Transform.Position);
            CWeaponDrop weaponDrop = entity.Get<CWeaponDrop>();

            TextureDefinition fanTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "Drops/LifeDropFan");
            float rotationOffset = _entityWorld.TotalUpdateTime;
            const int Fans = 6;
            for (int i = 0; i < Fans; i++)
            {
                float rotation = FlaiMath.TwoPi / Fans * i + rotationOffset;
                graphicsContext.SpriteBatch.Draw(fanTexture, entity.Transform.Position, Color.White * 0.5f, rotation, new Vector2(fanTexture.Width / 2f, fanTexture.Height), 0.3f);
            }

            if (DropHelper.IsBlinking(entity.Get<CLifeTime>()))
            {
                return;
            }

            const float Size = 30;
            const string FontName = "Minecraftia.16";

            graphicsContext.PrimitiveRenderer.DrawRectangle(position, Size, Color.White);
            graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(RectangleF.CreateCentered(position, Size), Color.Black, 4);
            graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer[FontName], weaponDrop.Type.ToChar(), position, Color.White, Color.Black, 0, 1f);
        }

        private void DrawLifeDrop(GraphicsContext graphicsContext, Entity entity)
        {
            // fans
            TextureDefinition fanTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "Drops/LifeDropFan");
            float rotationOffset = _entityWorld.TotalUpdateTime;
            Color color = Color.Lerp(Color.Red, Color.PaleVioletRed, 0.5f);
            const int Fans = 6;
            for (int i = 0; i < Fans; i++)
            {
                float rotation = FlaiMath.TwoPi / Fans * i + rotationOffset;
                graphicsContext.SpriteBatch.Draw(fanTexture, entity.Transform.Position, color * 0.5f, rotation, new Vector2(fanTexture.Width / 2f, fanTexture.Height), 0.5f);
            }

            // if the drop is blinking, then don't render the "heart"
            if (DropHelper.IsBlinking(entity.Get<CLifeTime>()))
            {
                return;
            }

            // heart
            const float Scale = SkypieaViewConstants.PixelSize * 1.5f;
            graphicsContext.SpriteBatch.DrawCentered(SkypieaViewConstants.LoadTexture(_contentProvider, "Drops/Life"), entity.Transform.Position, Color.White, 0f, Scale * (1f + FlaiMath.Sin(_entityWorld.TotalUpdateTime * 2f) * 0.1f));
        }
    }
}
