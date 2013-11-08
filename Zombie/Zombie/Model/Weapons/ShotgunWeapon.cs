using Flai;
using Flai.CBES;
using Zombie.Components;

namespace Zombie.Model.Weapons
{
    public class ShotgunWeapon : BulletWeapon
    {
        private const float MinimumTimeBetweenShots = 0.4f;

        public override WeaponType Type
        {
            get { return WeaponType.Shotgun; }
        }

        public ShotgunWeapon()
            : base(ShotgunWeapon.MinimumTimeBetweenShots, 15)
        {
        }

        protected override void ShootInner(EntityWorld entityWorld, TransformComponent parentTransform)
        {
            const int Bullets = 6;
            const float SpreadAngle = 0.3f;
            for (int i = 0; i < Bullets; i++)
            {
                entityWorld.AddEntity(Prefab.CreateInstance<ShotgunBulletPrefab>(parentTransform, -SpreadAngle / 2f + i / (float)Bullets * SpreadAngle, (BulletHitCallback)this.OnBulletHit));               
            }

            this.DecreaseBulletCount();
        }

        private bool OnBulletHit(BulletComponent bullet, Entity entityHit)
        {
            if (entityHit != null)
            {
                HealthComponent health = entityHit.TryGet<HealthComponent>();
                if (health)
                {
                    health.TakeDamage(20);
                }
                else
                {
                    entityHit.Delete();
                }
            }

            bullet.Parent.Delete();
            return false; // okay this shouldn't return always false. but rather like "allow 2 first hits to go through, and the stop"
        }
    }

    // todo: should Weapon bullets be at Prefab/ folder or in the same class as the weapon?
    // BulletPrefab..?
    public class ShotgunBulletPrefab : Prefab
    {
        // should bullets even have size? they could be just points... or maybe it's wise for them to have size..
        private static readonly SizeF BulletAreaSize = new SizeF(6, 6);

        // todo: instead of doing something like this, how about make a callback "OnCollided(Entity entity (or Vector2 position tms))" and then the weapon itself (whos callback it will be calling) decides the damage etc. this.. could be A LOT better!
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            const float Speed = Tile.Size * 25f;
            TransformComponent transform = (TransformComponent)parameters[0];
            float angleOffset = (float) parameters[1];
            BulletHitCallback callback = (BulletHitCallback)parameters[2];
            float rotation = transform.Rotation + angleOffset;

            entity.Add(new BulletComponent(ShotgunBulletPrefab.BulletAreaSize, callback));
            entity.Add(new TransformComponent(transform.Position) { Rotation = rotation });
            entity.Add(new VelocityComponent(FlaiMath.GetAngleVector(rotation), Speed));

            return entity;
        }
    }
}
