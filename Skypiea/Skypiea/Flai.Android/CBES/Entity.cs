using System.Collections.Generic;
#if CBES_3D
using DefaultTransformComponent = Flai.CBES.Components.CTransform3D;
#else
using DefaultTransformComponent = Flai.CBES.Components.CTransform2D;
#endif

using Flai.CBES.Systems;
using Flai.DataStructures;
using Flai.General;
using System;

namespace Flai.CBES
{
    // todo: scene nodes
    public sealed class Entity
    {
        #region Fields and Properties

        private readonly Bag<Entity> _children = new Bag<Entity>(0);
        private readonly ReadOnlyBag<Entity> _readOnlyChildren;

        private readonly EntityWorld _entityWorld;
        private readonly int _entityID; // entityID == index in the ComponentManager. stays always same, even if pooled. 

        private bool _isDeleting = false;

        public DefaultTransformComponent Transform { get; internal set; } // please don't be null, please don't be null.
        public uint UniqueID { get; internal set; } // uniqueID == unique ID that changes even if pooled. always unique
        public string Name { get; internal set; }
        // public bool IsEnabled { get; set; } // implement this

        public Entity Parent { get; private set; }
        public ReadOnlyBag<Entity> Children
        {
            get { return _readOnlyChildren; }
        }

        public EntityWorld EntityWorld
        {
            get { return _entityWorld; }
        }

        public IEnumerable<Component> AllComponents
        {
            get { return _entityWorld.ComponentManager.GetAllComponents(this); }
        }

        // really bad name :(
        public bool IsDeleting
        {
            get { return _isDeleting; }
        }

        /// <summary>
        /// Returns whether the entity is active or if the entity is deleted from the world
        /// </summary>
        public bool IsActive // IsValid?
        {
            get { return _entityWorld.EntityManager.IsActive(_entityID); }
        }

        internal TypeMask<EntitySystem> SystemMask { get; private set; } // ?? should be ProcessingSystem instead of EntitySystem (since EntitySystem doesn't "register" the entities, but EntitySystem already has SystemBit property so it's easier to just use it
        internal TypeMask<Component> ComponentMask { get; private set; }
        internal uint? PrefabID { get; set; }

        internal int EntityID
        {
            get { return _entityID; }
        }

        // todo: add "Group" string property? for example "BulletExplosion" and "BombExplosion" could be a part of "Effects" or "Explosions" group?
        // >> i could see use for this
        // tag can only be set once
        public uint? Tag
        {
            get { return _entityWorld.TagManager.GetTagOfEntity(this); }
            set
            {
                if (value.HasValue)
                {
                    _entityWorld.TagManager.Register(this, value.Value);
                }
                else
                {
                    _entityWorld.TagManager.Unregister(this);
                }

                _entityWorld.EntityRefreshManager.Refresh(this);
            }
        }

        #endregion

        internal Entity(EntityWorld entityWorld, int entityID)
        {
            _entityWorld = entityWorld;

            _readOnlyChildren = new ReadOnlyBag<Entity>(_children);
            _entityID = entityID;
            // this.IsEnabled = true;
        }

        #region Add/Get/Has

        public T AddFromPool<T>()
            where T : PoolableComponent, new()
        {
            return _entityWorld.ComponentManager.AddComponentFromPool<T>(this);
        }

        public T Add<T>()
            where T : Component, new()
        {
            return this.Add(new T());
        }

        // !! "T" has to be the correct type of the component, not a parent-type !!
        public T Add<T>(T component) // todo: does this work as a collection initializer?
            where T : Component
        {
            _entityWorld.ComponentManager.AddComponent(this, component);
            return component;
        }

        public bool Remove<T>()
            where T : Component
        {
            Ensure.True(Component<T>.CanBeRemoved, "Cannot remove a component of type " + typeof(T).Name);
            return _entityWorld.ComponentManager.RemoveComponent<T>(this);
        }

        public bool TryGet<T>(out T value)
            where T : Component
        {
            value = this.TryGet<T>();
            return value != null;
        }

        public T TryGet<T>()
            where T : Component
        {
            return _entityWorld.ComponentManager.TryGetComponent<T>(this);
        }

        public T Get<T>()
            where T : Component
        {
            return _entityWorld.ComponentManager.GetComponent<T>(this);
        }

        // TODO: Get<T> which allows any component that can be casted to T?

        public bool Has<T>()
            where T : Component
        {
            return this.ComponentMask.HasBits(Component<T>.Bit); // assume that this is up-to-date all the time
        }

        #endregion

        #region Delete

        public void Delete()
        {
            if (!_isDeleting)
            {
                _isDeleting = true;
                this.EntityWorld.DeleteEntity(this);

                // blaah!!! should these happen straight away? DeleteEntity might not (usually doesnt) delete straight away because of locks!!
                if (this.Parent != null)
                {
                    this.Parent.DetachChild(this);
                    this.Parent = null;
                }

                // ehh? should this happen? or just detach children and set their parents to null? dunno
                for (int i = 0; i < _children.Count; i++)
                {
                    // should I set child.Parent to null before doing this? since the parent (this) is already deleted
                    _children[i].Delete(); // this can cause a pretty massive reaction
                }

                _children.Clear();
            }
        }

        #endregion

        #region Parent/Child stuff

        public void AttachChild(Entity child)
        {
            if (child.Parent != null)
            {
                // throw an exception..?
                child.Parent.DetachChild(child);
            }

            child.Parent = this;
            _children.Add(child);
        }

        public void AttachParent(Entity parent)
        {
            if (parent == this.Parent)
            {
                return;
            }

            if (this.Parent != null)
            {
                this.Parent.DetachChild(this);
            }

            if (parent != null)
            {
                parent.AttachChild(this);
            }
            else
            {
                this.Parent = null;
            }
        }

        public void DetachAllChildren()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                // ? also change childs transform to keep it in same place? probably yeah..?
                _children[i].Parent = null;
            }

            _children.Clear();
        }

        private void DetachChild(Entity child)
        {
            // well this is not too performant.. I guess I could make binary-search somehow with UniqueID's and such but since
            // I don't expect that many entities will have a lot of children, that's not necessary. and it'd be probably slower for small amount of childs
            if (!_children.Remove(child))
            {
                throw new ArgumentException("child");
            }
        }

        #endregion

        #region Add/Remove Component/System Bit

        internal void AddComponentBit<T>()
          where T : Component
        {
            this.ComponentMask |= Component<T>.Bit;
        }

        public void ResetComponentMask()
        {
            this.ComponentMask = TypeMask<Component>.Empty;
        }

        internal void RemoveComponentBit<T>()
            where T : Component
        {
            this.ComponentMask &= ~Component<T>.Bit;
        }

        internal void AddSystemBit(TypeMask<EntitySystem> bit) // meh, can't use generics here since EntitySystem itself can't know its type
        {
            this.SystemMask |= bit;
        }

        internal void RemoveSystemBit(TypeMask<EntitySystem> bit) // meh, can't use generics here since EntitySystem itself can't know its type
        {
            this.SystemMask &= ~bit;
        }

        #endregion

        public static implicit operator bool(Entity entity)
        {
            return entity != null;
        }

        public override int GetHashCode()
        {
            return this.UniqueID.GetHashCode(); // hmm.. should this return UniqueID or EntityID? not sure.. i guess uniqueID?
        }

        // could be used for custom aspects
        public void Refresh()
        {
            _entityWorld.EntityRefreshManager.Refresh(this);
        }

        internal void Reset()
        {
            this.ComponentMask = TypeMask<Component>.Empty;
            this.SystemMask = TypeMask<EntitySystem>.Empty;
            this.PrefabID = null;
            this.Transform = null; // the Transform is already removed (EntityManager.RemoveInner calls ComponentManager.RemoveAllComponents), but this still has a reference
            // this.IsEnabled = true;

            _isDeleting = false;
            this.Parent = null;
            _children.Clear();
        }

        public override string ToString()
        {
            if (this.Name != null)
            {
                return this.Name;
            }

            return string.Format("EntityID: {0}, UniqueID = {1}", this.EntityID, this.UniqueID);
        }
    }
}