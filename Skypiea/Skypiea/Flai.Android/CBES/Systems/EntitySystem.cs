
using Flai.General;

namespace Flai.CBES.Systems
{
    public abstract class EntitySystem
    {
        private bool _enabled = true;

        protected internal EntityWorld EntityWorld { get; internal set; }
        protected internal virtual int ProcessOrder { get { return 0; } }

        internal TypeMask<EntitySystem> SystemBit { get; set; }
        public bool IsEnabled
        {
            get { return _enabled; }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    if (_enabled)
                    {
                        this.OnEnabled();
                    }
                    else
                    {
                        this.OnDisabled();
                    }
                }
            }
        }

        // do entity system specific stuff here, hide details from user.
        // should be always called by OnInitialize
        internal virtual void OnPreInitialize()
        {
            this.InternalPreInitialize();
            this.PreInitialize();
        }

        internal void OnInitialize()
        {
            this.Initialize();
        }

        internal void OnShutdown()
        {
            this.Shutdown();
        }

        internal void OnUpdate(UpdateContext updateContext)
        {
            if (this.IsEnabled)
            {
                this.BeginUpdate(updateContext);
                this.Update(updateContext);
                this.EndUpdate(updateContext);
            }
        }

        internal virtual void InternalPreInitialize() { }
        protected virtual void PreInitialize() { }
        protected virtual void Initialize() { }
        protected virtual void Shutdown() { }

        protected virtual void Update(UpdateContext updateContext) { }

        internal virtual void OnEntityAddedToWorld(Entity entity) { }
        internal virtual void OnEntityRemovedFromWorld(Entity entity) { }
        internal virtual void OnEntityChangedInWorld(Entity entity) { }

        protected virtual void OnEnabled() { }
        protected virtual void OnDisabled() { }

        protected virtual void BeginUpdate(UpdateContext updateContext) { }
        protected virtual void EndUpdate(UpdateContext updateContext) { }

        public void ToggleEnabled()
        {
            this.IsEnabled = !this.IsEnabled;
        }
    }
}
