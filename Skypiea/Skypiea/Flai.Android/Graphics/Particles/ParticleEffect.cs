using Flai.Graphics.Particles.Controllers;
using Flai.Graphics.Particles.TriggerHandlers;
using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles
{
    public class ParticleEffect
    {
        private ParticleControllerCollection _controllers;
        private ParticleEmitterCollection _emitters;
        private ParticleTriggerHandlerCollection _triggerHandlers;

        public ParticleControllerCollection Controllers
        {
            get { return _controllers ?? (this.Controllers = new ParticleControllerCollection()); }
            set
            {
                Ensure.NotNull(value);
                if (_controllers != value)
                {
                    _controllers = value;
                    _controllers.SetOwner(this);
                }
            }
        }

        public ParticleEmitterCollection Emitters
        {
            get { return _emitters ?? (this.Emitters = new ParticleEmitterCollection()); }
            set
            {
                Ensure.NotNull(value);
                if (_emitters != value)
                {
                    _emitters = value;
                    _emitters.SetOwner(this);
                }
            }
        }

        public ParticleTriggerHandlerCollection TriggerHandlers
        {
            get { return _triggerHandlers ?? (this.TriggerHandlers = new ParticleTriggerHandlerCollection()); }
            set
            {
                Ensure.NotNull(value);
                _triggerHandlers = value;
            }
        }

        internal void Update(UpdateContext updateContext)
        {
            this.Controllers.Update(updateContext);
            this.Emitters.Update(updateContext);
        }

        public void Trigger(ITransform2D transform)
        {
            this.Trigger(transform.Position, transform.Rotation);
        }

        public void Trigger(Vector2 position)
        {
            this.Trigger(position, 0);
        }

        public void Trigger(Vector2 position, float rotation)
        {
            this.Emitters.Trigger(position, rotation);
        }

        public void Terminate()
        {
            this.Emitters.Terminate();
        }
    }
}
