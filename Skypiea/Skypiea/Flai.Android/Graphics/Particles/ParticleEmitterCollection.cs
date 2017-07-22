using Flai.DataStructures;
using Flai.Graphics.Particles.Controllers;
using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles
{
    public class ParticleEmitterCollection : ExtendableList<ParticleEmitter>
    {
        #region Owner stuff

        private ParticleEffect _owner;
        internal void SetOwner(ParticleEffect particleEffect)
        {
            _owner = particleEffect;
            foreach (ParticleEmitter controller in this)
            {
                controller.Initialize(_owner);
            }
        }

        public override void Add(ParticleEmitter item)
        {
            base.Add(item);
            if (_owner != null)
            {
                item.Initialize(_owner);
            }
        }

        public override void Insert(int index, ParticleEmitter item)
        {
            base.Insert(index, item);
            if (_owner != null)
            {
                item.Initialize(_owner);
            }
        }

        public override ParticleEmitter this[int index]
        {
            get { return base[index]; }
            set
            {
                base[index] = value;
                if (_owner != null && value != null)
                {
                    value.Initialize(_owner);
                }
            }
        }

        #endregion

        internal void Update(UpdateContext updateContext)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Update(updateContext);
            }
        }

        internal void Terminate()
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Terminate();
            }
        }

        internal void Trigger(Vector2 position, float rotation)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Trigger(position, rotation);
            }
        }
    }
}
