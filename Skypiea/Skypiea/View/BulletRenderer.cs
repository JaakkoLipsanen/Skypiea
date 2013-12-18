using System;
using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Graphics;
using Flai.DataStructures;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

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

                CBullet bullet = entity.Get<CBullet>(); // not used :P       
                Texture2D texture = this.GetTexture(bullet.Weapon.Type);
                if (bullet.Weapon.Type == WeaponType.RocketLauncher)
                {
                    graphicsContext.SpriteBatch.DrawCentered(texture, entity.Transform.Position, Color.White, entity.Transform.Rotation, 2);
                }
                else if (bullet.Weapon.Type == WeaponType.Ricochet)
                {
                    graphicsContext.SpriteBatch.DrawCentered(texture, entity.Transform.Position, Color.LightGray, entity.Transform.Rotation, 1);
                }
                else if (bullet.Weapon.Type == WeaponType.Flamethrower)
                {
                    CLifeTime lifeTime = entity.Get<CLifeTime>();
                    Color color = Color.Lerp(Color.LightGoldenrodYellow, Color.OrangeRed, 1 - lifeTime.NormalizedTimeRemaining) * 0.75f;
                    graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, entity.Transform.Position, color * lifeTime.NormalizedTimeRemaining, 0, FlaiMath.Scale(lifeTime.NormalizedTimeRemaining, 1, 0, 8, 32));
                }
                else
                {
                    // graphicsContext.SpriteBatch.DrawCentered(graphicsContext.BlankTexture, entity.Transform.Position, Color.Yellow * 0.5f, entity.Transform.Rotation, new Vector2(16, 3));
                    graphicsContext.SpriteBatch.DrawCentered(texture, entity.Transform.Position, new Color(255, 255, 128) * 0.625f, entity.Transform.Rotation, 4);
                }
            }
        }

        private Texture2D GetTexture(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.RocketLauncher:
                    return _contentProvider.DefaultManager.LoadTexture("Rocket");

                case WeaponType.Ricochet:
                    return _contentProvider.DefaultManager.LoadTexture("RicochetBullet");

                case WeaponType.Flamethrower:
                    return null;

                case WeaponType.AssaultRifle:
                case WeaponType.Shotgun:
                case WeaponType.Minigun:
                    return _contentProvider.DefaultManager.LoadTexture("Bullet");

                default:
                    throw new ArgumentException("weaponType");
            }
        }
    }
}
