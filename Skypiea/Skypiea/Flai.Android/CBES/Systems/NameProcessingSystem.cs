

namespace Flai.CBES.Systems
{
    public abstract class NameProcessingSystem : EntitySystem
    {
        private readonly string _name;
        private Entity _entity;

        public Entity Entity
        {
            get { return _entity; }
        }

        protected NameProcessingSystem(string name)
        {
            _name = name.NullIfEmpty();
            Ensure.NotNull(_name);
        }

        internal override void InternalPreInitialize()
        {
            _entity = this.EntityWorld.FindEntityByName(_name);
            Ensure.NotNull(_entity);
        }

        protected sealed override void Update(UpdateContext updateContext)
        {
            this.Process(updateContext, this.Entity);
        }

        protected abstract void Process(UpdateContext updateContext, Entity entity);
    }
}
