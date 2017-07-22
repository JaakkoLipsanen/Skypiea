using Flai.DataStructures;

namespace Flai.Graphics.Particles.Controllers
{
    public class ParticleControllerCollection : ExtendableList<ParticleEffectController>
    {
        #region Owner stuff

        private ParticleEffect _owner;
        internal void SetOwner(ParticleEffect particleEffect)
        {
            _owner = particleEffect;
            foreach (ParticleEffectController controller in this)
            {
                controller.Initialize(_owner);
            }
        }

        public override void Add(ParticleEffectController item)
        {
            base.Add(item);
            if (_owner != null)
            {
                item.Initialize(_owner);
            }
        }

        public override void Insert(int index, ParticleEffectController item)
        {
            base.Insert(index, item);
            if (_owner != null)
            {
                item.Initialize(_owner);
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
    }
}
