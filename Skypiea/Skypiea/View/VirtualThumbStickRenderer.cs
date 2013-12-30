﻿using Flai.CBES;
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
            CVirtualThumbstick virtualThumbstick = entity.Get<CVirtualThumbstick>();
            this.DrawThumbStick(graphicsContext, virtualThumbstick);
        }

        private void DrawThumbStick(GraphicsContext graphicsContext, CVirtualThumbstick virtualThumbstickComponent)
        {
            const float MaxDistance = 60f;

            VirtualThumbstick thumbstick = virtualThumbstickComponent.Thumbstick;
            if (thumbstick.CenterPosition.HasValue)
            {
                // base
                graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("ThumbstickBase"), thumbstick.CenterPosition.Value, Color.Gray * 0.75f);

                if (thumbstick.Direction.HasValue)
                {
                    // if the thumbstick style is relative, then never draw the "name of the thumbstick", if the style is fixed then draw the name only if the direction IS zero
                    if (thumbstick.Style == ThumbstickStyle.Relative || thumbstick.Direction != Vector2.Zero)
                    {
                        graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("ThumbstickBase"), thumbstick.CenterPosition.Value + thumbstick.Direction.Value * MaxDistance, Color.LightGray * 0.5f, 0, 0.5f);
                    }
                    else
                    {
                        string name = virtualThumbstickComponent.Entity.Name == EntityNames.MovementThumbStick ? "MOVEMENT" : "SHOOTING";
                        graphicsContext.SpriteBatch.DrawStringCentered(graphicsContext.FontContainer["Minecraftia.24"], name, thumbstick.CenterPosition.Value, Color.White * 0.5f);
                    }
                }
            }
        }
    }
}
