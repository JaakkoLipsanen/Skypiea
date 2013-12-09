using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Services;
using System;

namespace Skypiea.Model.Weapons
{
    public class Laser : Weapon
    {
        private const float AmmoCount = 40;
        private const float AmmoUsedPerSecond = 3;
        private const float DamagePerSecond = 60;
        private const float MaxHitDistance = 4;
        public const float Size = 16f;

        private float _ammoRemaining = Laser.AmmoCount;
        public override WeaponType Type
        {
            get { return WeaponType.Laser; }
        }

        public override int? AmmoRemaining
        {
            get { return (int)FlaiMath.Ceiling(_ammoRemaining); }
        }

        public override bool CanShoot
        {
            get { return !this.IsFinished; }
        }

        public bool IsShooting { get; private set; }
        public Segment2D LaserSegment { get; private set; }

        public override void Update(UpdateContext updateContext, EntityWorld entityWorld)
        {
            // todo: there is some time between Update (Component.PreUpdate) and Shoot call.. so the "IsShooting" will be false there,
            // >> even if the laser is actually shooting... pfff...
            this.IsShooting = false;
        }

        public override void Shoot(UpdateContext updateContext, Entity playerEntity)
        {
            if (this.CanShoot)
            {
                _ammoRemaining -= updateContext.DeltaSeconds * Laser.AmmoUsedPerSecond;
                if (_ammoRemaining < 0)
                {
                    _ammoRemaining = 0;
                }

                this.LaserSegment = this.GetLaserSegment(playerEntity.Transform, playerEntity.EntityWorld);
                IZombieSpatialMap zombieSpatialMap = playerEntity.EntityWorld.Services.Get<IZombieSpatialMap>();
                foreach (Entity zombie in zombieSpatialMap.GetZombiesIntersecting(this.LaserSegment, Laser.MaxHitDistance))
                {
                    ZombieHelper.TakeDamageOrDelete(zombie, Laser.DamagePerSecond * updateContext.DeltaSeconds);
                }

                this.IsShooting = true;
            }
            else
            {
                this.IsShooting = false;
            }
        }

        private Segment2D GetLaserSegment(CTransform2D rayStartTransform, EntityWorld entityWorld)
        {
            Ray2D ray = new Ray2D(rayStartTransform);
            RectangleF cameraArea = SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active);
            Vector2 rayEndPoint;
            if (!ray.Intersects(cameraArea, out rayEndPoint))
            {
                throw new InvalidOperationException("camera is fucked up? or player isn't in the camera area");
            }

            return new Segment2D(ray.Position, rayEndPoint);
        }
    }
}
