using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.DataStructures;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Model;
using Skypiea.Prefabs;

namespace Skypiea.View
{
    public class ZombieRenderer : EntityRenderer
    {
        public ZombieRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<CZombieInfo>())
        {
        }

        // using _spatialMap.GetAllIntersecting(CCamera2D.Active.GetArea(graphicsContext.ScreenSize))) causes all kinds of problems (render order changes when zombies move from cell and pretty sure other things too)
        protected override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            // the texture size is actually 63x63, but the area in the texture that is of the actual zombie (no shadows) is 48x48
            const float BaseTextureSize = 48;

            RectangleF cameraArea = CCamera2D.Active.GetArea(graphicsContext.ScreenSize);
            TextureDefinition texture = SkypieaViewConstants.LoadTexture(_contentProvider, "Zombie");

            foreach (Entity zombie in entities)
            {
                CZombieInfo zombieInfo = zombie.Get<CZombieInfo>();
                if (zombieInfo.AreaRectangle.Intersects(cameraArea))
                {
                    float scale = zombieInfo.Size / BaseTextureSize;
                    graphicsContext.SpriteBatch.DrawCentered(texture, zombie.Transform.Position, ZombieRenderer.GetColor(zombieInfo), zombie.Transform.Rotation, scale);
                }
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
