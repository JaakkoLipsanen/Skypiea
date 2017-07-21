using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.Graphics;
using Flai.Misc;
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
            this.DrawThumbStick(graphicsContext, entity.Get<CVirtualThumbstick>());
        }

        private void DrawThumbStick(GraphicsContext graphicsContext, CVirtualThumbstick virtualThumbstickComponent)
        {
            VirtualThumbstick thumbstick = virtualThumbstickComponent.Thumbstick;

            // if the thumbstick style is relative and the user isn't pressing down, the CenterPosition doesn't have a value.
            // don't draw the thumbstick in that case
            if (thumbstick.CenterPosition.HasValue)
            {
                TextureDefinition thumbstickTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "ThumbstickBase");
                float alpha = thumbstick.IsPressed ? 0.5f : 1f;

                // base
                graphicsContext.SpriteBatch.DrawCentered(thumbstickTexture, thumbstick.CenterPosition.Value, Color.Gray * 0.75f * alpha);
                if (thumbstick.Direction.HasValue)
                {
                    // draw the "position"/direction texture only if the thumbstick is actually pressed
                    if (thumbstick.IsPressed)
                    {
                        const float MaxDistance = 60f;
                        graphicsContext.SpriteBatch.DrawCentered(thumbstickTexture, thumbstick.CenterPosition.Value + thumbstick.Direction.Value * MaxDistance, Color.LightGray * 0.5f * alpha, 0, 0.5f);
                    }
                    // otherwise draw the text on the thumbstick
                    else
                    {
                        string name = (virtualThumbstickComponent.Entity.Name == EntityNames.MovementThumbStick) ? "MOVEMENT" : "SHOOTING";
                        graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.24"], name, thumbstick.CenterPosition.Value, Color.White * 0.5f * alpha);
                    }
                }
            }
        }
    }
}
