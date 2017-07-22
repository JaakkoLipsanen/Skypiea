using Flai.Graphics.Particles.EmitterStyles;
using Flai.Graphics.Particles.Modifiers;
using Flai.Graphics.Particles.TriggerHandlers;
using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles
{
    // sealed?
    public sealed class ParticleEmitter
    {
        private readonly ParticleBuffer _particleBuffer;
        private readonly EmitterStyle _emitterStyle;
        private readonly float _particleLifeTime;
        private ParticleEffect _owner;
        private float _totalSeconds;

        public bool Enabled { get; set; }
        public ReleaseParameters ReleaseParameters { get; set; }
        public ParticleBlendMode BlendMode { get; set; }
        public TextureDefinition Texture { get; set; }
        public ParticleModifierCollection Modifiers { get; set; }
        public ModifierExecutionStrategy ExecutionStrategy { get; set; }

        public EmitterStyle EmitterStyle
        {
            get { return _emitterStyle; }
        }

        public float ParticleLifeTime
        {
            get { return _particleLifeTime; }
        }

        public int Budget
        {
            get { return _particleBuffer.Capacity; }
        }

        internal ParticleBuffer Buffer
        {
            get { return _particleBuffer; }
        }

        public ParticleEmitter(int budget, float particleLifeTime, EmitterStyle emitterStyle)
        {
            Ensure.True(budget > 0);
            Ensure.IsValid(particleLifeTime);
            Ensure.True(particleLifeTime >= 0);
            Ensure.NotNull(emitterStyle);

            _particleBuffer = new ParticleBuffer(budget);
            _particleLifeTime = particleLifeTime;
            _emitterStyle = emitterStyle;

            this.Modifiers = new ParticleModifierCollection();
            this.ReleaseParameters = new ReleaseParameters();
            this.ExecutionStrategy = ModifierExecutionStrategy.Serial;
            this.Enabled = true;
        }

        internal void Initialize(ParticleEffect owner)
        {
            _owner = owner;
        }

        public void Terminate()
        {
            _particleBuffer.Clear();
        }

        #region Update

        public void Update(UpdateContext updateContext)
        {
            _totalSeconds += updateContext.DeltaSeconds;
            if (_particleBuffer.IsEmpty)
            {
                _totalSeconds = 0f; // if particle buffer is empty, then reset the _totalSeconds to keep the floating point precision good
                return;
            }

            this.UpdateParticles(updateContext);
            if (!_particleBuffer.IsEmpty)
            {
                ParticleBuffer.Iterator iterator = _particleBuffer.CreateIterator(ParticleBuffer.IteratorMode.Normal);
                this.ExecutionStrategy.ExecuteModifiers(updateContext, this.Modifiers, ref iterator);
            }
        }

        private void UpdateParticles(UpdateContext updateContext)
        {
            int currentIndex = _particleBuffer.TailIndex;
            int count = _particleBuffer.Count;
            float inverseParticleLifeTime = 1f / _particleLifeTime;

            for (int i = 0; i < count; i++)
            {
                Particle particle;
                _particleBuffer.Get(currentIndex, out particle);

                float age = _totalSeconds - particle.Inception;
                if (age > _particleLifeTime)
                {
                    _particleBuffer.RemoveFirst();
                }
                else
                {
                    particle.Age = age * inverseParticleLifeTime;
                    particle.Position += particle.Velocity * updateContext.DeltaSeconds;
                    _particleBuffer.Set(currentIndex, ref particle);
                }

                if (++currentIndex == _particleBuffer.Capacity)
                {
                    currentIndex = 0;
                }
            }
        }

        #endregion

        #region Trigger

        public void Trigger(Vector2 position)
        {
            this.Trigger(position, 0, Global.Random.Next(this.ReleaseParameters.Quantity));
        }

        public void Trigger(Vector2 position, float rotation)
        {
            this.Trigger(position, rotation, Global.Random.Next(this.ReleaseParameters.Quantity));
        }

        public void Trigger(ITransform2D transform)
        {
            this.Trigger(transform.Position, transform.Rotation, Global.Random.Next(this.ReleaseParameters.Quantity));
        }

        public void Trigger(Vector2 position, float rotation, int releaseAmount)
        {
            if (!this.Enabled)
            {
                return;
            }

            if (_owner.TriggerHandlers.Count > 0)
            {
                TriggerContext triggerContext = new TriggerContext
                {
                    Position = position,
                    Rotation = rotation,
                    ReleaseAmount = releaseAmount,
                    Canceled = false,
                };

                _owner.TriggerHandlers.Process(ref triggerContext);
                if (triggerContext.Canceled)
                {
                    return;
                }

                position = triggerContext.Position;
                rotation = triggerContext.Rotation;
                releaseAmount = triggerContext.ReleaseAmount;
            }

            Matrix rotationMatrix = default(Matrix);
            bool isRotated = FlaiMath.RealModulus(rotation, FlaiMath.TwoPi) != 0;
            if (isRotated)
            {
                Matrix.CreateRotationZ(rotation, out rotationMatrix);
            }

            Particle particle = default(Particle);
            for (int i = 0; i < releaseAmount; i++)
            {
                _emitterStyle.GenerateOffsetAndForce(out particle.Position, out particle.Velocity);
                if (isRotated)
                {
                    Vector2.Transform(ref particle.Position, ref rotationMatrix, out particle.Position);
                    Vector2.Transform(ref particle.Velocity, ref rotationMatrix, out particle.Velocity);
                }

                particle.Inception = _totalSeconds;
                particle.Position += position;
                particle.Velocity *= Global.Random.NextFloat(this.ReleaseParameters.Speed);
                particle.Rotation = Global.Random.NextFloat(this.ReleaseParameters.Rotation);
                particle.Scale = Global.Random.NextFloat(this.ReleaseParameters.Scale);
                particle.Color = this.ReleaseParameters.Color.NextVector3(Global.Random);
                particle.Opacity = Global.Random.NextFloat(this.ReleaseParameters.Opacity);
                particle.Noise = Global.Random.NextFloat(0, 1);

                _particleBuffer.AddLast(ref particle);
            }
        }

        #endregion
    }
}
