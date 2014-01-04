using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.DataStructures;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Model;
using Skypiea.Systems.Zombie;

namespace Skypiea.View
{
    public class ZombieRenderer : EntityRenderer
    {
        private readonly IZombieSpatialMap _spatialMap;
        public ZombieRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<CZombieInfo>())
        {
            _spatialMap = entityWorld.Services.Get<IZombieSpatialMap>();
        }

        // using _spatialMap.GetAllIntersecting(CCamera2D.Active.GetArea(graphicsContext.ScreenSize))) causes all kinds of problems (render order changes when zombies move from cell and pretty sure other things too)
        protected override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            const float RealTextureSize = 48;
            RectangleF cameraArea = CCamera2D.Active.GetArea(graphicsContext.ScreenSize);
            TextureDefinition texture = SkypieaViewConstants.LoadTexture(_contentProvider, "Zombie");

            foreach (Entity zombie in entities)
            {
                CZombieInfo zombieInfo = zombie.Get<CZombieInfo>();
                if (zombieInfo.AreaRectangle.Intersects(cameraArea))
                {
                    float scale = zombieInfo.Size / RealTextureSize;
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
