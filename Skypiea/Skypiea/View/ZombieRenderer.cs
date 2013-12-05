using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs;

namespace Skypiea.View
{
    public class ZombieRenderer : EntityProcessingRenderer
    {
        public ZombieRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.WithTag(EntityTags.Zombie))
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity)
        {
            CArea area = entity.Get<CArea>();
            if (CCamera2D.Active.GetArea(graphicsContext.ScreenSize).Intersects(area.Area))
            {
                graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("ZombieShadow"), entity.Transform.Position, Color.White * 0.5f, 0, 1.4f * 0.75f);
                graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("Zombie"), entity.Transform.Position, ZombieRenderer.GetColor(entity), entity.Transform.Rotation, 1 * 0.75f);
            }
        }

        private static readonly Aspect BasicZombieAspect = Aspect.FromPrefab<BasicZombiePrefab>();
        private static readonly Aspect RusherZombieAspect = Aspect.FromPrefab<RusherZombiePrefab>();
        private static Color GetColor(Entity entity)
        {
            if (ZombieRenderer.BasicZombieAspect.Matches(entity))
            {
                return new Color(0, 255, 255);
            }
            else if (ZombieRenderer.RusherZombieAspect.Matches(entity))
            {
                return new Color(72, 72, 255);
            }

            return Color.HotPink;
        }
    }
}
