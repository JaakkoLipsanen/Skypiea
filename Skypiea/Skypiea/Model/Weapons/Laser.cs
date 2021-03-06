using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Boosters;
using Skypiea.Systems.Zombie;

namespace Skypiea.Model.Weapons
{
    public class Laser : Weapon
    {
        private const float InitialAmmo = 40;
        private const float AmmoUsedPerSecond = 3;
        private const float DamagePerSecond = 60;
        public const float MaxHitDistance = 4;
        public const float Size = 16f;

        private float _ammoRemaining = Laser.InitialAmmo;
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

        public Laser(float ammoMultiplier)
        {
            _ammoRemaining *= ammoMultiplier;
        }

        public override void Update(UpdateContext updateContext, EntityWorld entityWorld)
        {
            // todo: there is some time between Update (Component.PreUpdate) and Shoot call.. so the "IsShooting" will be false there,
            // >> even if the laser is actually shooting... pfff...
            this.IsShooting = false;
        }

        public override void Shoot(UpdateContext updateContext, EntityWorld entityWorld, Entity playerEntity)
        {
            float boosterAttackSpeedMultiplier = BoosterHelper.GetPlayerAttackSpeedMultiplier(entityWorld.Services.Get<IBoosterState>());
            float passiveAttackSpeedMultiplier = entityWorld.Services.Get<IPlayerPassiveStats>().FireRateMultiplier;
            float attackSpeedMultiplier = boosterAttackSpeedMultiplier * passiveAttackSpeedMultiplier;

            if (this.CanShoot)
            {
                _ammoRemaining -= updateContext.DeltaSeconds * attackSpeedMultiplier * Laser.AmmoUsedPerSecond;
                if (_ammoRemaining < 0)
                {
                    _ammoRemaining = 0;
                }

                this.LaserSegment = this.GetLaserSegment(playerEntity.Transform, entityWorld);
                IZombieSpatialMap zombieSpatialMap = entityWorld.Services.Get<IZombieSpatialMap>();
                foreach (Entity zombie in zombieSpatialMap.GetAllIntersecting(this.LaserSegment, Laser.MaxHitDistance))
                {
                    if (zombie.Get<CHealth>().IsAlive && ZombieHelper.TakeDamage(zombie, Laser.DamagePerSecond * attackSpeedMultiplier * updateContext.DeltaSeconds, null))
                    {
                        ZombieHelper.TriggerBloodExplosion(zombie.Transform, this.LaserSegment.Direction * SkypieaConstants.PixelsPerMeter * 10f);
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
            RectangleF cameraArea = SkypieaConstants.GetAdjustedCameraArea(CCamera2D.Active);
            Vector2 rayEndPoint;
            if (!ray.Intersects(cameraArea, out rayEndPoint))
            {
                return new Segment2D(-Vector2.One * 1000, -Vector2.One * 1000);
            }

            return new Segment2D(ray.Position, rayEndPoint);
        }

        public override void OnNewInstancePickedUp()
        {
            _ammoRemaining += Laser.InitialAmmo;
        }
    }
}
