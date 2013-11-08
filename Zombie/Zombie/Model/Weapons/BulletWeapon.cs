using Flai;
using Flai.CBES;
using Flai.Misc;
using Zombie.Components;

namespace Zombie.Model.Weapons
{
    public abstract class BulletWeapon : Weapon
    {
        private readonly Timer _bulletTimer;
        private int? _bullets;

        public override bool CanShoot
        {
            get { return _bulletTimer.HasFinished; }
        }

        public override int? BulletsRemaining
        {
            get { return _bullets; }
        }

        protected BulletWeapon(float timeBetweenShots, int? bullets)
        {
            _bulletTimer = new Timer(timeBetweenShots);
            _bullets = bullets;
        }

        public sealed override void Update(UpdateContext updateContext)
        {
            _bulletTimer.Update(updateContext);
            this.UpdateInner(updateContext);
        }

        public sealed override void Shoot(EntityWorld entityWorld, TransformComponent parentTransform)
        {
            if (this.CanShoot && !this.IsFinished)
            {
                this.ShootInner(entityWorld, parentTransform);
                _bulletTimer.Restart();
            }
        }

        protected virtual void UpdateInner(UpdateContext updateContext) { }
        protected abstract void ShootInner(EntityWorld entityWorld, TransformComponent parentTransform);

        // todo: it'd be a lot better if I had centralized "ShootBullet"/"AddBulletToWorld" method that should always be called. since then I could also make the "OnBulletHit" a virtual method here
        protected void DecreaseBulletCount()
        {
            if (_bullets.HasValue)
            {
                _bullets--;
            }
        }
    }

}
