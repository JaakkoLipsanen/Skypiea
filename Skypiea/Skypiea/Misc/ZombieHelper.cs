using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.Graphics.Particles;
using Flai.Graphics.Particles.EmitterStyles;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Model.Boosters;
using Skypiea.View;

namespace Skypiea.Misc
{
    public static class ZombieHelper
    {
        public static bool TakeDamage(Entity zombie, float damage)
        {
            return ZombieHelper.TakeDamage(zombie, damage, null);
        }

        public static bool TakeDamage(Entity zombie, float damage, Vector2? explosionVelocity)
        {
            CHealth health = zombie.Get<CHealth>();
            if (!health.IsAlive)
            {
                return true;
            }

            IBoosterState boosterState = zombie.EntityWorld.Services.Get<IBoosterState>();
            bool killed = health.TakeDamage(damage * BoosterHelper.GetZombieDamageRedcutionMultiplier(boosterState));
            if (explosionVelocity.HasValue)
            {
                if (killed)
                {
                    ZombieHelper.TriggerBloodExplosion(zombie.Transform, explosionVelocity.Value);
                }
                else
                {
                    ZombieHelper.TriggerBloodSplatter(zombie.Transform, Vector2.Zero);
                }
            }

            if (killed)
            {
                zombie.Get<CZombieInfo>().SetKillReason(KillReason.Normal);
            }

            return killed;
        }

        public static bool Kill(Entity zombie, Vector2? explosionVelocity)
        {
            const float LotsOfDamage = 1000 * 1000;
            zombie.Get<CHealth>().TakeDamage(LotsOfDamage);
            if (explosionVelocity.HasValue)
            {
                ZombieHelper.TriggerBloodExplosion(zombie.Transform, explosionVelocity.Value);
            }

            zombie.Get<CZombieInfo>().SetKillReason(KillReason.Instakill);
            return true;
        }

        public static void TriggerBloodExplosion(CTransform2D transform, Vector2 velocity)
        {
            IParticleEngine particleEngine = transform.Entity.EntityWorld.Services.Get<IParticleEngine>();
            ParticleEffect effect = particleEngine[ParticleEffectID.ZombieExplosion];
            RotationalPointEmitter emitter = (RotationalPointEmitter)effect.Emitters[0].EmitterStyle;
            if (velocity == Vector2.Zero)
            {
                emitter.RotationalRange = Range.FullRotation;
                effect.Emitters[0].ReleaseParameters.Speed = ParticleEffectHelper.ZombieExplosionDefaultSpeed;
            }
            else
            {
                const float ExplosionRotationalBias = 1.15f;
                emitter.RotationalRange = Range.CreateCentered(FlaiMath.GetAngle(velocity), ExplosionRotationalBias);
                effect.Emitters[0].ReleaseParameters.Speed = new Range(0, velocity.Length());
            }

            effect.Trigger(transform.Position);
        }

        public static void TriggerBloodSplatter(CTransform2D transform)
        {
            ZombieHelper.TriggerBloodSplatter(transform, Vector2.Zero);
        }

        public static void TriggerBloodSplatter(CTransform2D transform, Vector2 direction)
        {
            IParticleEngine particleEngine = transform.Entity.EntityWorld.Services.Get<IParticleEngine>();
            particleEngine[ParticleEffectID.ZombieBloodSplatter].Trigger(transform.Position);
        }
    }
}
