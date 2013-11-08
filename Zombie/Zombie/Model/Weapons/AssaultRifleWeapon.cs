using Flai;
using Flai.CBES;
using Zombie.Components;

namespace Zombie.Model.Weapons
{
    public class AssaultRifleWeapon : BulletWeapon
    {
        private const float MinimumTimeBetweenShots = 0.15f;
        public override WeaponType Type
        {
            get { return WeaponType.AssaultRifle; }
        }

        public AssaultRifleWeapon()
            : base(AssaultRifleWeapon.MinimumTimeBetweenShots, null)
        {
        }

        protected override void ShootInner(EntityWorld entityWorld, TransformComponent parentTransform)
        {
            entityWorld.AddEntity(Prefab.CreateInstance<AssaultRifleBulletPrefab>(parentTransform, (BulletHitCallback)this.OnBulletHit));
            this.DecreaseBulletCount();
        }

        private bool OnBulletHit(BulletComponent bullet, Entity entityHit)
        {
            if (entityHit != null)
            {
                HealthComponent health = entityHit.TryGet<HealthComponent>();
                if (health)
                {
                    health.TakeDamage(10);
                }
                else
                {
                    entityHit.Delete();
                }
            }

            bullet.Parent.Delete();
            return true;
        }
    }

    // todo: should Weapon bullets be at Prefab/ folder or in the same class as the weapon?
    // BulletPrefab..?
    public class AssaultRifleBulletPrefab : Prefab
    {
        // should bullets even have size? they could be just points... or maybe it's wise for them to have size..
        private static readonly SizeF BulletAreaSize = new SizeF(6, 6);

        // todo: instead of doing something like this, how about make a callback "OnCollided(Entity entity (or Vector2 position tms))" and then the weapon itself (whos callback it will be calling) decides the damage etc. this.. could be A LOT better!
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            const float Speed = Tile.Size * 25f;
            TransformComponent transform = (TransformComponent)parameters[0];
            BulletHitCallback callback = (BulletHitCallback) parameters[1];

            entity.Add(new BulletComponent(AssaultRifleBulletPrefab.BulletAreaSize, callback));
            entity.Add(new TransformComponent(transform));
            entity.Add(new VelocityComponent(FlaiMath.GetAngleVector(transform.Rotation), Speed));

            return entity;
        }
    }
}
