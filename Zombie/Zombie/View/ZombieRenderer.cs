using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Prefabs;

namespace Zombie.View
{
    public class ZombieRenderer : EntityProcessingRenderer
    {
        public ZombieRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.WithTag(EntityTags.Zombie))
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity)
        {
            TransformComponent transform = entity.Get<TransformComponent>();
            AreaComponent area = entity.Get<AreaComponent>();

            if (CameraComponent.Active.GetArea(graphicsContext.ScreenSize).Intersects(area.Area))
            {
                graphicsContext.SpriteBatch.DrawCentered(graphicsContext.ContentProvider.DefaultManager.LoadTexture("Zombie"), transform.Position, ZombieRenderer.GetColor(entity), transform.Rotation, 1.5f);
            }
        }

        private static readonly Aspect BasicZombieAspect = Aspect.FromPrefab<BasicZombiePrefab>();
        private static readonly Aspect RusherZombieAspect = Aspect.FromPrefab<RusherZombiePrefab>();
        private static Color GetColor(Entity entity)
        {
            if (ZombieRenderer.BasicZombieAspect.Matches(entity))
            {
                return Color.DimGray;
            }
            else if (ZombieRenderer.RusherZombieAspect.Matches(entity))
            {
                return new Color(72, 72, 255);
            }

            return Color.HotPink;
        }
    }
}
