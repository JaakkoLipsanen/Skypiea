using Flai;
using Flai.CBES;
using Flai.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Weapons;

namespace Skypiea.View
{
    public class WeaponRenderer : FlaiRenderer
    {
        private readonly CWeapon _playerWeapon;
        public WeaponRenderer(EntityWorld entityWorld)
        {
            _playerWeapon = entityWorld.FindEntityByName(EntityNames.Player).Get<CWeapon>();
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            if (_playerWeapon.Weapon.Type == WeaponType.Laser)
            {
                this.DrawLaser(graphicsContext, _playerWeapon.Weapon as Laser);
            }
        }

        private void DrawLaser(GraphicsContext graphicsContext, Laser laser)
        {
            if (laser.IsShooting)
            {
                const float Scale = 3;
                Texture2D laserTexture = _contentProvider.DefaultManager.LoadTexture("Laser");

                float firstPartLength = laserTexture.Height - (graphicsContext.TotalSeconds * 300f % laserTexture.Height);
                Rectangle firstPartSource = new RectangleF(0, firstPartLength, laserTexture.Width, laserTexture.Height - firstPartLength).ToRectangle();
                graphicsContext.SpriteBatch.Draw(laserTexture, laser.LaserSegment.Start, firstPartSource, Color.White, FlaiMath.GetAngle(laser.LaserSegment.Direction) - FlaiMath.PiOver2, new Vector2(laserTexture.Width / 2f, 0), Scale);

                float secondPartStartDistance = laserTexture.Height - firstPartLength;
                graphicsContext.SpriteBatch.Draw(laserTexture, laser.LaserSegment.Start + laser.LaserSegment.Direction * secondPartStartDistance * Scale, Color.White, FlaiMath.GetAngle(laser.LaserSegment.Direction) - FlaiMath.PiOver2, new Vector2(laserTexture.Width / 2f, 0), Scale);
            //    graphicsContext.PrimitiveRenderer.DrawLine(laser.LaserSegment, Color.Blue * 0.75f, 8f);
                //graphicsContext.PrimitiveRenderer.DrawLine(laser.LaserSegment, Color.Lerp(Color.Yellow, Color.Black, 0.5f), Laser.Size);
                //graphicsContext.PrimitiveRenderer.DrawLine(laser.LaserSegment, Color.Lerp(Color.Yellow, Color.Black, 0.25f), Laser.Size * 0.75f);
                //graphicsContext.PrimitiveRenderer.DrawLine(laser.LaserSegment, Color.Lerp(Color.Yellow, Color.Red, 0.75f), Laser.Size * 0.25f);
            }
        }
    }
}
