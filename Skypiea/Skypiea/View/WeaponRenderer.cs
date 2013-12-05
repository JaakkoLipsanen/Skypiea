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
                graphicsContext.PrimitiveRenderer.DrawLine(laser.LaserSegment, Color.Lerp(Color.Yellow, Color.Black, 0.5f), Laser.Size);
                graphicsContext.PrimitiveRenderer.DrawLine(laser.LaserSegment, Color.Lerp(Color.Yellow, Color.Black, 0.25f), Laser.Size * 0.75f);
                graphicsContext.PrimitiveRenderer.DrawLine(laser.LaserSegment, Color.Lerp(Color.Yellow, Color.Red, 0.75f), Laser.Size * 0.25f);
            }
        }
    }
}
