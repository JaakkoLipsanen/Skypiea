using System;
using System.Linq;
using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Model.Weapons
{
    public class Laser : Weapon
    {
        private const float AmmoCount = 30f;
        private const float AmmoUsedPerSecond = 2f;
        private const float DamagePerSecond = 10;
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

        public override void Update(UpdateContext updateContext)
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
                foreach (Entity zombie in playerEntity.EntityWorld.FindEntitiesWithTag(EntityTags.Zombie).Where(entity => Segment2D.MinimumDistance(this.LaserSegment, entity.Get<CArea>().Area) < Laser.MaxHitDistance))
                {
                    CHealth health = zombie.TryGet<CHealth>();
                    if (health)
                    {
                        health.TakeDamage(Laser.DamagePerSecond * updateContext.DeltaSeconds);
                    }
                    else
                    {
                        zombie.Delete();
                    }
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
            Vector2? zombieHitPoint = this.GetFirstZombieHit(ray, entityWorld);
            if (zombieHitPoint.HasValue)
            {
                return new Segment2D(ray.Position, zombieHitPoint.Value);
            }

            // if not zombie hit is found, then set endpoint to where the ray hits the camera boundaries (not really camera boundaries so that it doesn't look to player like the laser hits it, but close outside it)
            RectangleF cameraArea = SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active);
            Vector2 rayEndPoint;
            if (!ray.Intersects(cameraArea, out rayEndPoint))
            {
                throw new InvalidOperationException("camera is fucked up?");
            }

            return new Segment2D(ray.Position, rayEndPoint);
        }

        private Vector2? GetFirstZombieHit(Ray2D laserRay, EntityWorld entityWorld)
        {
            RectangleF cameraArea = SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active);

            float closestDistance = float.MaxValue;
            Vector2 closestPoint = default(Vector2);
            foreach (CArea zombieArea in entityWorld.FindEntitiesWithTag(EntityTags.Zombie).Where(zombie => cameraArea.Contains(zombie.Transform.Position)).Select(zombie => zombie.Get<CArea>()))
            {
                Vector2 intersectionPoint;
                if (laserRay.Intersects(zombieArea.Area, out intersectionPoint))
                {
                    float distance = Vector2.Distance(laserRay.Position, intersectionPoint);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPoint = intersectionPoint;
                    }
                }
            }

            if (closestDistance != float.MaxValue)
            {
                return closestPoint;
            }

            return null;
        }
    }
}
