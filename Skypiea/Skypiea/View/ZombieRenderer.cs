using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.DataStructures;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
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
            foreach (Entity zombie in entities)
            {
                CZombieInfo zombieInfo = zombie.Get<CZombieInfo>();
                if (zombieInfo.AreaRectangle.Intersects(cameraArea))
                {
                    float scale = zombieInfo.Size / BaseTextureSize;
                    graphicsContext.SpriteBatch.DrawCentered(this.GetTexture(zombieInfo.Type), zombie.Transform.Position, ZombieRenderer.GetColor(zombieInfo), zombie.Transform.Rotation, scale);
                }

                // draw golden goblin glow
                if (zombieInfo.Type == ZombieType.GoldenGoblin)
                {
                    graphicsContext.SpriteBatch.DrawCentered(graphicsContext.ContentProvider.DefaultManager.LoadTexture("GoldenGoblinGlow"), zombie.Transform.Position, Color.Gold * 0.5f, 0, 3f + FlaiMath.Sin(graphicsContext.TotalSeconds * 2));

                    CGoldenGoblinAI goldenGoblinAI = zombie.Get<CGoldenGoblinAI>();
                    for (int i = 0; i < goldenGoblinAI.Waypoints.Length - 1; i++)
                    {
                        graphicsContext.PrimitiveRenderer.DrawLine(goldenGoblinAI.Waypoints[i], goldenGoblinAI.Waypoints[i + 1], Color.Red, 4f);
                    }
                }
            }
        }

        private TextureDefinition GetTexture(ZombieType zombieType)
        {
            switch (zombieType)
            {
                case ZombieType.GoldenGoblin:
                    return _contentProvider.DefaultManager.LoadTexture("ZombieWhite");

                default:
                    return SkypieaViewConstants.LoadTexture(_contentProvider, "Zombie");
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

                case ZombieType.GoldenGoblin:
                    return Color.Gold;

                default:
                    return Color.HotPink;
            }
        }
    }
}
