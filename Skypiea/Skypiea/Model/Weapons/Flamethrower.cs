using Flai;
using Flai.CBES;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class Flamethrower : BulletWeapon
    {
        private const int InitialAmmo = 300;
        private const float TimeBetweenShots = 0.8f;
        private float _bulletCounter = 0f;

        public Flamethrower(float ammoMultiplier)
            : base((int)(Flamethrower.InitialAmmo * ammoMultiplier), Flamethrower.TimeBetweenShots)
        {
        }

        public override bool CanShoot
        {
            get { return !this.IsFinished; }
        }

        public override WeaponType Type
        {
            get { return WeaponType.Flamethrower; }
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<FlamethrowerBulletPrefab>(playerEntity.Transform, this);
            playerEntity.EntityWorld.CreateEntityFromPrefab<FlamethrowerBulletPrefab>(playerEntity.Transform, this);

            _bulletCounter += 0.33333f;
            if (_bulletCounter > 1)
            {
                _bulletCounter -= 1;
                this.DecreaseBulletCount();
            }
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            if (entityHit.Get<CHealth>().IsAlive && ZombieHelper.TakeDamage(entityHit, updateContext.DeltaSeconds * 10f, null))
            {
                ZombieHelper.TriggerBloodExplosion(entityHit.Transform, Vector2.Zero);
            }

            return false;
        }
    }

    public class Waterthrower : Flamethrower
    {
        public override WeaponType Type
        {
            get { return WeaponType.Waterblaster; }
        }

        public Waterthrower(float ammoMultiplier) 
            : base(ammoMultiplier)
        {
        }
    }
}
