using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            if (drop.DropType == DropType.Weapon)
            {
                this.DrawWeaponDrop(graphicsContext, entity);
            }
            else
            {
                this.DrawLifeDrop(graphicsContext, entity);
            }
        }

        private void DrawWeaponDrop(GraphicsContext graphicsContext, Entity entity)
        {
            if (DropHelper.IsBlinking(entity.Get<CLifeTime>()))
            {
                return;
            }

            const float Size = 27;
            const string FontName = "Minecraftia.16";

            CWeaponDrop weaponDrop = entity.Get<CWeaponDrop>();
            graphicsContext.PrimitiveRenderer.DrawRectangle(entity.Transform.Position, Size, new Color(72, 72, 228));
            graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer[FontName], weaponDrop.Type.ToChar(), entity.Transform.Position, 0, 0.85f);
            graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(RectangleF.CreateCentered(entity.Transform.Position, Size), Color.White, 2);
        }

        private void DrawLifeDrop(GraphicsContext graphicsContext, Entity entity)
        {
            // fans
            TextureDefinition fanTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "Drops/LifeDropFan");
            float rotationOffset = graphicsContext.TotalSeconds;

            const int Fans = 6;
            for (int i = 0; i < Fans; i++)
            {
                float rotation = FlaiMath.TwoPi / Fans * i + rotationOffset;
                graphicsContext.SpriteBatch.Draw(fanTexture, entity.Transform.Position, Color.PaleVioletRed * 0.5f, rotation, new Vector2(fanTexture.Width / 2f, fanTexture.Height), 0.5f);
            }

            // if the drop is blinking, then don't render the "heart"
            if (DropHelper.IsBlinking(entity.Get<CLifeTime>()))
            {
                return;
            }

            // heart
            const float Scale = SkypieaViewConstants.PixelSize * 1.5f;
            graphicsContext.SpriteBatch.DrawCentered(SkypieaViewConstants.LoadTexture(_contentProvider, "Drops/Life"), entity.Transform.Position, Color.White, 0f, Scale * (1f + FlaiMath.Sin(graphicsContext.TotalSeconds * 2f) * 0.1f));
        }
    }
}
