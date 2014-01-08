using Flai;
using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.View
{
    public class WeaponRenderer : FlaiRenderer
    {
        private readonly EntityWorld _entityWorld;
        private readonly CWeapon _playerWeapon;

        public WeaponRenderer(EntityWorld entityWorld)
        {
            _entityWorld = entityWorld;
            _playerWeapon = entityWorld.FindEntityByName(EntityNames.Player).Get<CWeapon>();
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            if (_playerWeapon.Weapon.Type == WeaponType.Laser)
            {
                this.DrawLaser(graphicsContext, (Laser)_playerWeapon.Weapon);
            }
        }

        private void DrawLaser(GraphicsContext graphicsContext, Laser laser)
        {
            if (laser.IsShooting)
            {
                const float Scale = 3;
                const float Speed = 200;

                TextureDefinition laserTexture = SkypieaViewConstants.LoadTexture(_contentProvider, "Laser");

                // can't use texture wrapping so create the same effect by drawing the laser twice
                float firstPartLength = laserTexture.Height - (_entityWorld.TotalUpdateTime * Speed % laserTexture.Height);
                Rectangle firstPartSource = new RectangleF(0, firstPartLength, laserTexture.Width, laserTexture.Height - firstPartLength).ToRectangle();
                graphicsContext.SpriteBatch.Draw(laserTexture, laser.LaserSegment.Start, firstPartSource, Color.White, FlaiMath.GetAngle(laser.LaserSegment.Direction) - FlaiMath.PiOver2, new Vector2(laserTexture.Width / 2f, 0), Scale);

                // there is probably some over-draw here in the second part, but.. don't care :P
                float secondPartStartDistance = laserTexture.Height - firstPartLength;
                graphicsContext.SpriteBatch.Draw(laserTexture, laser.LaserSegment.Start + laser.LaserSegment.Direction * secondPartStartDistance * Scale, Color.White, FlaiMath.GetAngle(laser.LaserSegment.Direction) - FlaiMath.PiOver2, new Vector2(laserTexture.Width / 2f, 0), Scale);
            }
        }
    }
}
