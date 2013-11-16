using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;

namespace Zombie.View
{
    public class BulletRenderer : EntityProcessingRenderer
    {
        public BulletRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<BulletComponent>())
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity)
        {
            BulletComponent bullet = entity.Get<BulletComponent>();
            TransformComponent transform = entity.Get<TransformComponent>();

            graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, transform.Position, Color.Yellow * 0.5f, transform.Rotation, new Vector2(16, 3));
        }
    }
}
