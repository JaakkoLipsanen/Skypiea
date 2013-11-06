using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Prefabs;

namespace Zombie.View
{
     public class ZombieRenderer : EntityProcessingRenderer
    {
        public ZombieRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.FromPrefab<ZombiePrefab>())
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity)
        {
            TransformComponent transform = entity.Get<TransformComponent>();
            AreaComponent area = entity.Get<AreaComponent>();

            if (CameraComponent.Active.GetArea(graphicsContext.ScreenSize).Intersects(area.Area))
            {
                graphicsContext.SpriteBatch.DrawCentered(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Zombie"), transform.Position, Color.DimGray, transform.Rotation, 1.5f);
            }
        }
    }
}
