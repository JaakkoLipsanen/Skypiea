
using Microsoft.Xna.Framework;

namespace Flai
{
    public abstract class FlaiGameComponent : FlaiService, IGameComponent
    {
        protected bool _enabled = true;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if(_enabled != value)
                {
                    _enabled = value;
                    this.OnEnabledChanged();
                }
            }
        }

        protected FlaiGameComponent(FlaiServiceContainer services)
            : base(services)
        {
        }

        public virtual void Initialize() { }

        public virtual void Update(UpdateContext updateContext) { }
        protected virtual void OnEnabledChanged() { }
    }
}
