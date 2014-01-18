using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.DataStructures;
using Flai.Diagnostics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;
using System;

namespace Skypiea.View
{
    public class DropArrowRenderer : EntityRenderer
    {
        public DropArrowRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<CDrop>())
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            RectangleF cameraArea = CCamera2D.Active.GetArea().Inflate(-16);
            foreach (Entity entity in entities)
            {
                if (!cameraArea.Contains(entity.Transform.Position))
                {
                    this.DrawArrow(graphicsContext, entity);
                }
            }
        }

        private void DrawArrow(GraphicsContext graphicsContext, Entity entity)
        {
            if (DropHelper.IsBlinking(entity.Get<CLifeTime>()))
            {
                return;
            }

            CDrop drop = entity.Get<CDrop>();
            Vector2 screenPosition = this.GetArrowScreenBorderPosition(graphicsContext, entity.Transform.Position);
            RectangleF cameraArea = CCamera2D.Active.GetArea();

            float distanceFromBorder = cameraArea.MinDistance(drop.Transform.Position);
            float scale = FlaiMath.Scale(distanceFromBorder, 0, 1536, 1, 0.25f);

            if (drop.DropType == DropType.Life)
            {
                this.DrawLifeDropArrow(graphicsContext, drop, screenPosition, scale);
            }
            else if(drop.DropType == DropType.Weapon)
            {
                this.DrawWeaponDropArrow(graphicsContext, drop, screenPosition, scale);
            }
        }

        private void DrawLifeDropArrow(GraphicsContext graphicsContext, CDrop drop, Vector2 screenPosition, float scale)
        {
            const float BaseScale = 6;
            graphicsContext.SpriteBatch.DrawCentered(SkypieaViewConstants.LoadTexture(_contentProvider, "Life"), screenPosition, Color.White * SkypieaViewConstants.DropArrowAlpha, 0, BaseScale * scale);
        }

        private void DrawWeaponDropArrow(GraphicsContext graphicsContext, CDrop drop, Vector2 screenPosition, float scale)
        {
            const float Size = 30;
            const string FontName = "Minecraftia.16";
            CWeaponDrop weaponDrop = drop.Entity.Get<CWeaponDrop>();

            graphicsContext.PrimitiveRenderer.DrawRectangle(screenPosition, Size * scale, Color.White * SkypieaViewConstants.DropArrowAlpha);
            graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(RectangleF.CreateCentered(screenPosition, Size * scale), Color.Black * SkypieaViewConstants.DropArrowAlpha, 4 * scale);
            graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer[FontName], weaponDrop.Type.ToChar(), screenPosition, Color.White * SkypieaViewConstants.DropArrowAlpha, Color.Black * SkypieaViewConstants.DropArrowAlpha, 0, scale);
        }

        #region Get Arrow Screen Border Position

        private Vector2 GetArrowScreenBorderPosition(GraphicsContext graphicsContext, Vector2 entityPosition)
        {
            Vector2 direction = entityPosition - CCamera2D.Active.Position;
            Ray2D ray = new Ray2D(graphicsContext.ScreenSize / 2f, Vector2.Normalize(direction)); // ray from screen center to the direction of the drop
            if (!Check.IsValid(ray))
            {
                return -Vector2.One * 10000; // invalid number
            }

            RectangleF screenArea = new RectangleF(graphicsContext.ScreenArea).Inflate(-16);
            Vector2 hitPosition;
            if (ray.Intersects(screenArea.GetSideSegment(Direction2D.Left), out hitPosition) || ray.Intersects(screenArea.GetSideSegment(Direction2D.Right), out hitPosition) ||
                ray.Intersects(screenArea.GetSideSegment(Direction2D.Up), out hitPosition) || ray.Intersects(screenArea.GetSideSegment(Direction2D.Down), out hitPosition))
            {
                return hitPosition;
            }

            throw new InvalidOperationException("");
        }

        #endregion
    }
}
