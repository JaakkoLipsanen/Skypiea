using System;
using Flai;
using Flai.CBES;
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
