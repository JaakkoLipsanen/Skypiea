using Flai;
using Flai.CBES;
using Flai.General;
using Skypiea.Components;
using Skypiea.Model.Boosters;

namespace Skypiea.Model.Weapons
{
    public abstract class BulletWeapon : Weapon
    {
        private readonly Timer _bulletTimer;
        private readonly int? _initialBulletCount;
        private int? _bulletsRemaining;

        public override int? AmmoRemaining
        {
            get { return _bulletsRemaining; }
        }

        public override bool CanShoot
        {
            get { return _bulletTimer.HasFinished; }
        }

        protected BulletWeapon(int? bullets, float timeBetweenShots)
        {
            Ensure.True(timeBetweenShots > 0);
            Ensure.IsValid(timeBetweenShots);

            _initialBulletCount = bullets;
            _bulletsRemaining = bullets;
            _bulletTimer = new Timer(timeBetweenShots);
        }

        public sealed override void Update(UpdateContext updateContext, EntityWorld entityWorld)
        {
            float boosterAttackSpeedMultiplier = BoosterHelper.GetPlayerAttackSpeedMultiplier(entityWorld.Services.Get<IBoosterState>());
            float passiveAttackSpeedMultiplier = entityWorld.Services.Get<IPlayerPassiveStats>().FireRateMultiplier;
            _bulletTimer.Update(updateContext.DeltaSeconds * boosterAttackSpeedMultiplier * passiveAttackSpeedMultiplier);

            this.UpdateInner(updateContext);
        }

        public sealed override void Shoot(UpdateContext updateContext, EntityWorld entityWorld, Entity playerEntity)
        {
            if (this.CanShoot && !this.IsFinished)
            {
                this.ShootInner(updateContext, entityWorld, playerEntity);
                _bulletTimer.Restart();
            }
        }

        protected virtual void UpdateInner(UpdateContext updateContext) { }
        protected abstract void ShootInner(UpdateContext updateContext, EntityWorld entityWorld, Entity playerEntity);

        public virtual bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            return false;
        }

        public override void OnNewInstancePickedUp()
        {
            if (_initialBulletCount.HasValue && _bulletsRemaining.HasValue)
            {
                _bulletsRemaining += _initialBulletCount.Value;
            }
        }

        protected void DecreaseBulletCount()
        {
            if (_bulletsRemaining.HasValue)
            {
                _bulletsRemaining--;
            }
        }
    }
}
