using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
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
           // graphicsContext.PrimitiveRenderer.DrawRectangle(virtualThumbstick.ThumbStick.CenterPosition, 12f, Color.Blue);
            graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("ThumbstickBase"), virtualThumbstick.ThumbStick.CenterPosition, Color.Gray * 0.75f);

            if (virtualThumbstick.ThumbStick.Direction != Vector2.Zero)
            {
                graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("ThumbstickBase"), virtualThumbstick.ThumbStick.CenterPosition + virtualThumbstick.ThumbStick.Direction*MaxDistance, Color.LightGray*0.5f, 0, 0.5f);
            }
            else
            {
                string name = virtualThumbstick.Entity.Name == EntityNames.MovementThumbStick ? "MOVEMENT" : "ROTATION";
                graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.24"], name, virtualThumbstick.ThumbStick.CenterPosition, Color.White * 0.5f);
            }            
        }
    }
}
