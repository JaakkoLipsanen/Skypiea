using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Messages;
using Zombie.Misc;

namespace Zombie.Systems
{
    public class ZombieExplosionManagerSystem : EntitySystem
    {
        protected override void Initialize()
        {
            this.EntityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            // todo: particles!! this is very unefficient!! single particle shouldn't be a entity, rather make something like ParticleSystem, IParticleSystem/IParticleSubmitter and then something something
            const int ParticleCount = 6;

            Vector2 position = message.Zombie.Get<TransformComponent>().Position;
            for (int i = 0; i < ParticleCount; i++)
            {
                float direction = i/(float) ParticleCount*FlaiMath.TwoPi; // + Global.Random.NextFloat(-FlaiMath.TwoPi / ParticleCount / 2, FlaiMath.TwoPi / ParticleCount / 2);
                this.EntityWorld.AddEntity(Prefab.CreateInstance<ZombieExplosionPrefab>(position, direction));
            }
        }
    }

    public class ZombieExplosionPrefab : Prefab
    {
        protected override Entity BuildEntity(Entity entity, params object[] parameters)
        {
            Vector2 position = (Vector2)parameters[0];
            float direction = (float)parameters[1];
            float speed = Global.Random.NextFloat(48, 96);
            float size = Global.Random.NextFloat(32, 72);

            entity.Add(new TransformComponent(position));
            entity.Add(new ZombieExplosionParticleComponent(direction, speed, size));
            entity.Tag = EntityTags.ZombieExplosionParticle;

            return entity;
        }
    }

    public class ZombieExplosionParticleComponent : Component
    {
        private const float ParticleLifeTime = 1f;
        private readonly Vector2 _direction;
        private readonly float _speed;
        private readonly float _size;

        private TransformComponent _transform;
        private float _timeAlive = 0f;

        public float Size
        {
            get { return FlaiMath.Lerp(_size, 0, 1 - _timeAlive / ParticleLifeTime); }
        }

        public float Alpha
        {
            get { return (1 - _timeAlive / ParticleLifeTime) * (1 - _timeAlive / ParticleLifeTime); }
        }

        public ZombieExplosionParticleComponent(float direction, float speed, float size)
        {
            _direction = FlaiMath.GetAngleVector(direction);
            _speed = speed;
            _size = size;
        }

        protected override void OnAttachedToParent()
        {
            _transform = this.Parent.Get<TransformComponent>();
        }

        protected override void PreUpdate(UpdateContext updateContext)
        {
            _timeAlive += updateContext.DeltaSeconds;
            _transform.Position += _direction * _speed * this.Alpha * updateContext.DeltaSeconds;
            if (_timeAlive > ZombieExplosionParticleComponent.ParticleLifeTime)
            {
                this.Parent.Delete();
            }
        }
    }
}
