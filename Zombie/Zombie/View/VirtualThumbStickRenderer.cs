using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;

namespace Zombie.View
{
    public class VirtualThumbStickRenderer : EntityProcessingRenderer
    {
        public VirtualThumbStickRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.WithTag(EntityTags.VirtualThumbStick))
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity)
        {
            VirtualThumbStickComponent virtualThumbStick = entity.Get<VirtualThumbStickComponent>();
            this.DrawThumbStick(graphicsContext, virtualThumbStick);
        }

        private void DrawThumbStick(GraphicsContext graphicsContext, VirtualThumbStickComponent virtualThumbStick)
        {
            const float MaxDistance = 60f;
            graphicsContext.PrimitiveRenderer.DrawRectangle(graphicsContext, virtualThumbStick.ThumbStick.CenterPosition, 12f, Color.Blue);
            graphicsContext.PrimitiveRenderer.DrawRectangle(graphicsContext, virtualThumbStick.ThumbStick.CenterPosition + virtualThumbStick.ThumbStick.Direction * MaxDistance, 32f, Color.Blue);
        }
    }
}
