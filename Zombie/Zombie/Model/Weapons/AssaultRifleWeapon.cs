using Flai;
using Flai.CBES;
using Microsoft.Xna.Framework;
using Zombie.Components;

namespace Zombie.Model.Weapons
{
    public class AssaultRifleWeapon : Weapon
    {
        private const float MinimumTimeBetweenShots = 0.1f;
        private float _timeSinceLastShot = 0f;

        public override bool CanShoot
        {
            get { return _timeSinceLastShot >= AssaultRifleWeapon.MinimumTimeBetweenShots; }
        }

        public override bool IsFinished
        {
            get { return true; } // assault rifle doesn't have bullets
        }

        public override void Update(UpdateContext updateContext)
        {
            _timeSinceLastShot += updateContext.DeltaSeconds;
        }

        public override void Shoot(EntityWorld entityWorld, TransformComponent parentTransform)
        {
            if (this.CanShoot)
            {
                entityWorld.AddEntity(Prefab.CreateInstance<AssaultRifleBulletPrefab>(parentTransform));
                _timeSinceLastShot = 0f;
            }
        }
    }

    // BulletPrefab..?
    public class AssaultRifleBulletPrefab : Prefab
    {
        // todo: instead of doing something like this, how about make a callback "OnCollided(Entity entity (or Vector2 position tms))" and then the weapon itself (whos callback it will be calling) decides the damage etc. this.. could be A LOT better!
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            const float Speed = Tile.Size * 25f;
            TransformComponent transform = (TransformComponent)parameters[0];
            entity.Add(new BulletComponent(10));
            entity.Add(new TransformComponent(transform));

            Vector2 direction = FlaiMath.GetAngleVector(transform.Rotation + Global.Random.NextFloat() * 0.05F); // small offset in directino
            entity.Add(new VelocityComponent(direction, Speed));

            return entity;
        }
    }
}
