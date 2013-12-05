using Flai;
using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Model;
using Skypiea.Model.Weapons;

namespace Skypiea.View
{
    public class WeaponDropRenderer : ComponentProcessingRenderer<CWeaponDrop>
    {
        public WeaponDropRenderer(EntityWorld entityWorld)
            : base(entityWorld)
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity, CWeaponDrop weaponDrop)
        {
            const float Size = 27;
            const string FontName = "SegoeWP.16";

            const int Scale = 4;
            graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("SpecialDropBase"), Vector2i.Round(entity.Transform.Position), Color.White, 0, Scale);
            graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("Special Drops/Life"), Vector2i.Round(entity.Transform.Position), Color.White, 0, Scale);
         //   graphicsContext.PrimitiveRenderer.DrawRectangle(entity.Transform.Position, Size, new Color(72, 72, 228));
          //  graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer[FontName], weaponDrop.Type.ToChar(), entity.Transform.Position, 0, 0.85f);
          //  graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(RectangleF.CreateCentered(entity.Transform.Position, Size), Color.White, 2);
        }
    }
}
