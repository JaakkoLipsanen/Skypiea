using Flai;
using Flai.CBES;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Prefabs.Bullets;

namespace Skypiea.Model.Weapons
{
    public class Minigun : BulletWeapon
    {
        public override WeaponType Type
        {
            get { return WeaponType.Minigun; }
        }

        public Minigun() 
            : base(140, 0.1f)
        {
        }

        protected override void ShootInner(UpdateContext updateContext, Entity playerEntity)
        {
            playerEntity.EntityWorld.CreateEntityFromPrefab<NormalBulletPrefab>(playerEntity.Transform, this, 0f, Tile.Size * 40f);
            this.DecreaseBulletCount();
        }

        public override bool OnBulletHitCallback(UpdateContext updateContext, CBullet bullet, Entity entityHit)
        {
            if (entityHit != null)
            {
                if (!ZombieHelper.TakeDamageOrDelete(entityHit, 10))
                {
                    ZombieHelper.TriggerBloodSplatter(bullet.Entity.Transform);
                }
            }

            bullet.Entity.Delete();
            return true;
        }
    }
}
