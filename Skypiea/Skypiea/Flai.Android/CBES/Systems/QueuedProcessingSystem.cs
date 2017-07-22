using System;
using Flai.DataStructures;

namespace Flai.CBES.Systems
{
    // dunno if even works
    public abstract class QueuedProcessingSystem : EntitySystem
    {
        private readonly CircularQueue<Entity> _entities = new CircularQueue<Entity>();
        protected readonly Aspect _aspect;

        private int _maxEntitiesToProcess = 50;
        protected int MaxEntitiesToProcessPerFrame
        {
            get { return _maxEntitiesToProcess; }
            set
            {
                Ensure.True(value >= 0);
                _maxEntitiesToProcess = value;
            }
        }

        protected QueuedProcessingSystem(Aspect aspect)
            : this(aspect, 50)
        {
        }

        protected QueuedProcessingSystem(Aspect aspect, int maxEntitiesToProcess)
        {
            _aspect = aspect;
            this.MaxEntitiesToProcessPerFrame = maxEntitiesToProcess;
        }

        internal override void InternalPreInitialize()
        {
            foreach (Entity entity in base.EntityWorld.FindEntitiesByAspect(_aspect))
            {
                _entities.Enqueue(entity);
            }
        }

        protected override void Update(UpdateContext updateContext)
        {
            int toProcess = FlaiMath.Min(_maxEntitiesToProcess, _entities.Count);
            for (int i = 0; i < toProcess; i++)
            {
                this.Process(updateContext, _entities.Dequeue());
            }
        }

        internal override void OnEntityAddedToWorld(Entity entity)
        {
            if (_aspect.Matches(entity))
            {
                _entities.Enqueue(entity);
            }
        }

        internal override void OnEntityRemovedFromWorld(Entity entity)
        {
            if (_aspect.Matches(entity))
            {
                if (!_entities.Remove(entity))
                {
                    throw new ArgumentException("Not in sync");
                }
            }
        }

        protected abstract void Process(UpdateContext updateContext, Entity entity);
    }
}