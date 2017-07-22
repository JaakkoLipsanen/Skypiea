
namespace Flai.Graphics.Particles.Controllers
{
    public interface ITransformSettable
    {
        void SetTransform(ITransform2D transform);
    }

    public abstract class ParticleEffectController
    {
        protected ParticleEffect ParticleEffect { get; private set; }
        internal void Initialize(ParticleEffect particleEffect)
        {
            Ensure.Null(this.ParticleEffect);
            this.ParticleEffect = particleEffect;
            this.InitializeInner();
        }
        protected virtual void InitializeInner() { }

        protected internal abstract void Update(UpdateContext updateContext);
    }
}
