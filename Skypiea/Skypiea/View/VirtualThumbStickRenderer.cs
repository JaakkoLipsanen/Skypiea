using Flai.CBES;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.View
{
    public class VirtualThumbStickRenderer : EntityProcessingRenderer
    {
        public VirtualThumbStickRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<CVirtualThumbstick>())
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, Entity entity)
        {
            CVirtualThumbstick virtualThumbstick = entity.Get<CVirtualThumbstick>();
            this.DrawThumbStick(graphicsContext, virtualThumbstick);
        }

        private void DrawThumbStick(GraphicsContext graphicsContext, CVirtualThumbstick virtualThumbstick)
        {
            const float MaxDistance = 60f;
            graphicsContext.PrimitiveRenderer.DrawRectangle(virtualThumbstick.ThumbStick.CenterPosition, 12f, Color.Blue);
            graphicsContext.PrimitiveRenderer.DrawRectangle(virtualThumbstick.ThumbStick.CenterPosition + virtualThumbstick.ThumbStick.Direction * MaxDistance, 32f, Color.Blue);
        }
    }
}
