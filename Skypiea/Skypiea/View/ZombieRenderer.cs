using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Model;

namespace Skypiea.View
{
    public class ZombieRenderer : EntityProcessingRenderer
    {
        public ZombieRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<CZombieInfo>())
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity)
        {
            CZombieInfo zombieInfo = entity.Get<CZombieInfo>();
            if (CCamera2D.Active.GetArea(graphicsContext.ScreenSize).Intersects(zombieInfo.AreaRectangle))
            {
                const float RealTextureSize = 48;
                float scale = zombieInfo.Size / RealTextureSize;
             //   graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("ZombieShadow"), entity.Transform.Position, Color.White * 0.5f, 0, scale * 1.4f);
                graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("Zombie"), entity.Transform.Position, ZombieRenderer.GetColor(zombieInfo), entity.Transform.Rotation, scale);
            }
        }

        private static Color GetColor(CZombieInfo zombieInfo)
        {
            switch (zombieInfo.Type)
            {
                case ZombieType.Normal:
                    return new Color(0, 255, 255);

                case ZombieType.Rusher:
                    return new Color(72, 72, 255);

                case ZombieType.Fat:
                    return new Color(255, 108, 108);

                default:
                    return Color.HotPink;
            }
        }
    }
}
