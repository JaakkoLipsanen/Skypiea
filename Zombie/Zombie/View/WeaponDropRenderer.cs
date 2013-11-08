using Flai;
using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Model;
using Zombie.Model.Weapons;

namespace Zombie.View
{
    public class WeaponDropRenderer : ComponentProcessingRenderer<WeaponDropComponent, TransformComponent>
    {
        public WeaponDropRenderer(EntityWorld entityWorld)
            : base(entityWorld)
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity, WeaponDropComponent weaponDrop, TransformComponent transform)
        {
            const float Size = Tile.Size * 0.5f;
            const string FontName = "SegoeWP.16";
            graphicsContext.PrimitiveRenderer.DrawRectangle(transform.Position, Size, new Color(72, 72, 228));
            graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer[FontName], weaponDrop.Type.ToChar(), transform.Position);
            graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(RectangleF.CreateCentered(transform.Position, Size), Color.White);
        }
    }
}
