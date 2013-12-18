using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.DataStructures;
using Flai.Diagnostics;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
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
            CDrop drop = entity.Get<CDrop>();
            Vector2 screenPosition = this.GetArrowScreenBorderPosition(graphicsContext, entity.Transform.Position);

            float distanceFromBorder = CCamera2D.Active.GetArea().MinDistance(drop.Transform.Position);
            float scale = FlaiMath.Scale(distanceFromBorder, 0, 1536, 1, 0.25f);

            if (drop.DropType == DropType.Life)
            {
                this.DrawLifeDropArrow(graphicsContext, drop, screenPosition, scale);
            }
            else
            {
                this.DrawWeaponDropArrow(graphicsContext, drop, screenPosition, scale);
            }
        }

        private void DrawLifeDropArrow(GraphicsContext graphicsContext, CDrop drop, Vector2 screenPosition, float scale)
        {
            graphicsContext.SpriteBatch.DrawCentered(_contentProvider.DefaultManager.LoadTexture("Drops/Life"), screenPosition, Color.White * SkypieaViewConstants.DropArrowAlpha, 0, 6 * scale);
        }

        private void DrawWeaponDropArrow(GraphicsContext graphicsContext, CDrop drop, Vector2 screenPosition, float scale)
        {
            const float Size = 27;

            CWeaponDrop weaponDrop = drop.Entity.Get<CWeaponDrop>();
            graphicsContext.PrimitiveRenderer.DrawRectangle(screenPosition, Size * scale, new Color(72, 72, 228) * SkypieaViewConstants.DropArrowAlpha);
            graphicsContext.SpriteBatch.DrawStringFadedCentered(graphicsContext.FontContainer["Minecraftia.16"], weaponDrop.Type.ToChar(), screenPosition, Color.Black * SkypieaViewConstants.DropArrowAlpha, Color.White * SkypieaViewConstants.DropArrowAlpha, 0, 0.85f * scale);
            graphicsContext.PrimitiveRenderer.DrawRectangleOutlines(RectangleF.CreateCentered(screenPosition, Size * scale), Color.White * SkypieaViewConstants.DropArrowAlpha, 2 * scale);
        }

        #region Get Arrow Screen Border Position

        private Vector2 GetArrowScreenBorderPosition(GraphicsContext graphicsContext, Vector2 entityPosition)
        {
            Vector2 screenCenter = graphicsContext.ScreenSize / 2f;

            Vector2 direction = entityPosition - CCamera2D.Active.Position;// CCamera2D.Active.WorldToScreen(graphicsContext.ScreenSize, entityPosition);
            Ray2D ray = new Ray2D(screenCenter, Vector2.Normalize(direction));
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
