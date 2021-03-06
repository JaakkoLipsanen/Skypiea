﻿using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.DataStructures;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;
using System;

namespace Skypiea.View
{
    public class BulletRenderer : EntityRenderer
    {
        public BulletRenderer(EntityWorld entityWorld)
            : base(entityWorld, Aspect.All<CBullet>())
        {
        }

        protected override void Draw(GraphicsContext graphicsContext, ReadOnlyBag<Entity> entities)
        {
            RectangleF cameraArea = SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active);
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                if (!cameraArea.Contains(entity.Transform.Position))
                {
                    continue;
                }

                this.DrawBullet(graphicsContext, entity, entity.Get<CBullet>());
            }
        }

        private void DrawBullet(GraphicsContext graphicsContext, Entity entity, CBullet bullet)
        {     
            TextureDefinition texture = this.GetTexture(bullet.Weapon.Type);
            if (bullet.Weapon.Type == WeaponType.RocketLauncher)
            {
                graphicsContext.SpriteBatch.DrawCentered(texture, entity.Transform.Position, Color.White, entity.Transform.Rotation, 2);
            }
            else if (bullet.Weapon.Type == WeaponType.Bouncer)
            {
                graphicsContext.SpriteBatch.DrawCentered(texture, entity.Transform.Position, new Color(160, 160, 160), entity.Transform.Rotation, 1);
            }
            else if (bullet.Weapon.Type == WeaponType.Flamethrower)
            {
                CLifeTime lifeTime = entity.Get<CLifeTime>();
                Color color = Color.Lerp(Color.LightGoldenrodYellow, Color.OrangeRed, 1 - lifeTime.NormalizedTimeRemaining) * 0.75f;
                graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, entity.Transform.Position, color * lifeTime.NormalizedTimeRemaining, 0, FlaiMath.Scale(lifeTime.NormalizedTimeRemaining, 1, 0, 8, 32));
            }
            else if (bullet.Weapon.Type == WeaponType.Waterblaster)
            {
                CLifeTime lifeTime = entity.Get<CLifeTime>();
                Color color = Color.Lerp(Color.AliceBlue, new Color(0, 69, 255), 1 - lifeTime.NormalizedTimeRemaining) * 0.75f;
                graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, entity.Transform.Position, color * lifeTime.NormalizedTimeRemaining, 0, FlaiMath.Scale(lifeTime.NormalizedTimeRemaining, 1, 0, 8, 32));
            }
            else
            {
                graphicsContext.SpriteBatch.DrawCentered(texture, entity.Transform.Position, new Color(255, 255, 128) * 0.625f, entity.Transform.Rotation, 4);
            }
        }

        private TextureDefinition GetTexture(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.RocketLauncher:
                    return SkypieaViewConstants.LoadTexture(_contentProvider, "RocketBullet");

                case WeaponType.Bouncer:
                    return new TextureDefinition(_contentProvider.DefaultManager.LoadTexture("Weapons/RicochetBullet")); // SkypieaViewConstants.LoadTexture(_contentProvider, "RicochetBullet");

                case WeaponType.Flamethrower:
                case WeaponType.Waterblaster:
                    return new TextureDefinition(); // "null"

                case WeaponType.AssaultRifle:
                case WeaponType.Shotgun:
                case WeaponType.Minigun:
                    return SkypieaViewConstants.LoadTexture(_contentProvider, "Bullet");

                default:
                    throw new ArgumentException("weaponType");
            }
        }
    }
}
