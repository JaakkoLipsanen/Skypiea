using System;
using System.Text;
using Flai.General;

namespace Flai.CBES
{
    // struct..?
    // only problem is that the Tag and the predicate result can change and there is no way to know if that changes
    // tag = done. it can change only once (lets assume before it is added to entity)
    // will not do predicate, at least before I need it

    // considerations... Maybe there could be 3 different types of Aspects, base-Aspect, All/One/Exclude -Aspect, Tag-Aspect and Prefab-Aspect? sounds good? yay? is there 
    // really any point in combining those three?
    public abstract class Aspect
    {
        #region Empty Aspect

        private class EmptyAspect : Aspect
        {
            public override bool IsEmpty
            {
                get { return true; }
            }

            public override bool Matches(Entity entity)
            {
                return false;
            }
        }

        #endregion

        #region All Aspect

        private class AllAspect : Aspect
        {
            public override bool IsEmpty
            {
                get { return false; }
            }

            public override bool Matches(Entity entity)
            {
                return true;
            }
        }

        #endregion

        #region PrefabAspect

        private class PrefabAspect<T> : Aspect
            where T : Prefab
        {
            public override bool IsEmpty
            {
                get { return false; }
            }

            public override bool Matches(Entity entity)
            {
                return entity.PrefabID == Prefab<T>.ID;
            }
        }

        #endregion

        #region TagAspect

        internal class TagAspect : Aspect
        {
            private readonly uint _tag;
            public override bool IsEmpty
            {
                // Can't be empty, tag is not allowed to be empty
                get { return false; }
            }

            internal TagAspect(uint tag)
            {
                _tag = tag;
            }

            public override bool Matches(Entity entity)
            {
                return entity.Tag == _tag;
            }
        }

        #endregion

        #region Combine Aspect

        // Okay this assumes that it's "AND" always and not "OR".. but its okay..
        private class CombineAspect : Aspect
        {
            private readonly Aspect _aspect1;
            private readonly Aspect _aspect2;

            public override bool IsEmpty
            {
                get { return _aspect1.IsEmpty && _aspect2.IsEmpty; }
            }

            internal CombineAspect(Aspect aspect1, Aspect aspect2)
            {
                if (aspect1 == null || aspect2 == null)
                {
                    throw new ArgumentException("Aspect can't be null!");
                }

                _aspect1 = aspect1;
                _aspect2 = aspect2;
            }

            public override bool Matches(Entity entity)
            {
                return _aspect1.Matches(entity) && _aspect2.Matches(entity);
            }
        }

        #endregion

        public abstract bool IsEmpty { get; }
        public static readonly Aspect Empty = new EmptyAspect();
        public static readonly Aspect AcceptAll = new AllAspect(); // meh name

        public abstract bool Matches(Entity entity);

        #region Static All

        public static ComponentAspect All<T1>()
            where T1 : Component
        {
            return new ComponentAspect().WithAll<T1>();
        }

        public static ComponentAspect All<T1, T2>()
            where T1 : Component
            where T2 : Component
        {
            return new ComponentAspect().WithAll<T1, T2>();
        }

        public static ComponentAspect All<T1, T2, T3>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            return new ComponentAspect().WithAll<T1, T2, T3>();
        }

        public static ComponentAspect All<T1, T2, T3, T4>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
        {
            return new ComponentAspect().WithAll<T1, T2, T3, T4>();
        }

        public static ComponentAspect All<T1, T2, T3, T4, T5>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
        {
            return new ComponentAspect().WithAll<T1, T2, T3, T4, T5>();
        }

        public static ComponentAspect All<T1, T2, T3, T4, T5, T6>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
            where T6 : Component
        {
            return new ComponentAspect().WithAll<T1, T2, T3, T4, T5, T6>();
        }

        #endregion

        #region Static Any

        public static ComponentAspect Any<T1>()
            where T1 : Component
        {
            return new ComponentAspect().WithAny<T1>();
        }

        public static ComponentAspect Any<T1, T2>()
            where T1 : Component
            where T2 : Component
        {
            return new ComponentAspect().WithAny<T1, T2>();
        }

        public static ComponentAspect Any<T1, T2, T3>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            return new ComponentAspect().WithAny<T1, T2, T3>();
        }

        public static ComponentAspect Any<T1, T2, T3, T4>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
        {
            return new ComponentAspect().WithAny<T1, T2, T3, T4>();
        }

        public static ComponentAspect Any<T1, T2, T3, T4, T5>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
        {
            return new ComponentAspect().WithAny<T1, T2, T3, T4, T5>();
        }

        public static ComponentAspect Any<T1, T2, T3, T4, T5, T6>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
            where T6 : Component
        {
            return new ComponentAspect().WithAny<T1, T2, T3, T4, T5, T6>();
        }

        #endregion

        #region Static Exclude

        public static ComponentAspect Exclude<T1>()
            where T1 : Component
        {
            return new ComponentAspect().ExcludeWith<T1>();
        }

        public static ComponentAspect Exclude<T1, T2>()
            where T1 : Component
            where T2 : Component
        {
            return new ComponentAspect().ExcludeWith<T1, T2>();
        }

        public static ComponentAspect Exclude<T1, T2, T3>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            return new ComponentAspect().ExcludeWith<T1, T2, T3>();
        }

        public static ComponentAspect Exclude<T1, T2, T3, T4>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
        {
            return new ComponentAspect().ExcludeWith<T1, T2, T3, T4>();
        }

        public static ComponentAspect Exclude<T1, T2, T3, T4, T5>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
        {
            return new ComponentAspect().ExcludeWith<T1, T2, T3, T4, T5>();
        }

        public static ComponentAspect Exclude<T1, T2, T3, T4, T5, T6>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
            where T6 : Component
        {
            return new ComponentAspect().ExcludeWith<T1, T2, T3, T4, T5, T6>();
        }

        #endregion

        #region Static WithTag

        public static Aspect WithTag(uint tag)
        {
            return new TagAspect(tag);
        }

        #endregion

        public static Aspect Combine(Aspect aspect1, Aspect aspect2)
        {
            return new CombineAspect(aspect1, aspect2);
        }

        public static Aspect FromPrefab<T>()
            where T : Prefab
        {
            return new PrefabAspect<T>();
        }
    }

    public sealed class ComponentAspect : Aspect
    {
        private TypeMask<Component> _allMask = TypeMask<Component>.Empty;
        private TypeMask<Component> _excludeMask = TypeMask<Component>.Empty;
        private TypeMask<Component> _anyMask = TypeMask<Component>.Empty;

        public override bool IsEmpty
        {
            get { return _allMask.IsEmpty && _excludeMask.IsEmpty && _anyMask.IsEmpty; }
        }

        internal ComponentAspect()
        {
        }

        public override bool Matches(Entity entity)
        {
            // if fully empty, then no entity matches
            if (this.IsEmpty)
            {
                return false;
            }

            return
                (_allMask.IsEmpty || (_allMask & entity.ComponentMask) == _allMask) &&
                (_anyMask.IsEmpty || (_anyMask & entity.ComponentMask) != TypeMask<Component>.Empty) &&
                (_excludeMask.IsEmpty || (_excludeMask & entity.ComponentMask) == TypeMask<Component>.Empty); // ???? in artemis this is != _excludeMask but that doesn't make sense
        }

        #region WithAll

        public ComponentAspect WithAll<T1>()
            where T1 : Component
        {
            _allMask |= Component<T1>.Bit;
            return this;
        }

        public ComponentAspect WithAll<T1, T2>()
            where T1 : Component
            where T2 : Component
        {
            _allMask |= Component<T1>.Bit;
            _allMask |= Component<T2>.Bit;
            return this;
        }

        public ComponentAspect WithAll<T1, T2, T3>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            _allMask |= Component<T1>.Bit;
            _allMask |= Component<T2>.Bit;
            _allMask |= Component<T3>.Bit;

            return this;
        }

        public ComponentAspect WithAll<T1, T2, T3, T4>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
        {
            _allMask |= Component<T1>.Bit;
            _allMask |= Component<T2>.Bit;
            _allMask |= Component<T3>.Bit;
            _allMask |= Component<T4>.Bit;

            return this;
        }

        public ComponentAspect WithAll<T1, T2, T3, T4, T5>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
        {
            _allMask |= Component<T1>.Bit;
            _allMask |= Component<T2>.Bit;
            _allMask |= Component<T3>.Bit;
            _allMask |= Component<T4>.Bit;
            _allMask |= Component<T5>.Bit;

            return this;
        }

        public ComponentAspect WithAll<T1, T2, T3, T4, T5, T6>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
            where T6 : Component
        {
            _allMask |= Component<T1>.Bit;
            _allMask |= Component<T2>.Bit;
            _allMask |= Component<T3>.Bit;
            _allMask |= Component<T4>.Bit;
            _allMask |= Component<T5>.Bit;
            _allMask |= Component<T6>.Bit;

            return this;
        }

        #endregion

        #region WithAny

        public ComponentAspect WithAny<T1>()
            where T1 : Component
        {
            _anyMask |= Component<T1>.Bit;
            return this;
        }

        public ComponentAspect WithAny<T1, T2>()
            where T1 : Component
            where T2 : Component
        {
            _anyMask |= Component<T1>.Bit;
            _anyMask |= Component<T2>.Bit;
            return this;
        }

        public ComponentAspect WithAny<T1, T2, T3>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            _anyMask |= Component<T1>.Bit;
            _anyMask |= Component<T2>.Bit;
            _anyMask |= Component<T3>.Bit;

            return this;
        }

        public ComponentAspect WithAny<T1, T2, T3, T4>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
        {
            _anyMask |= Component<T1>.Bit;
            _anyMask |= Component<T2>.Bit;
            _anyMask |= Component<T3>.Bit;
            _anyMask |= Component<T4>.Bit;

            return this;
        }

        public ComponentAspect WithAny<T1, T2, T3, T4, T5>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
        {
            _anyMask |= Component<T1>.Bit;
            _anyMask |= Component<T2>.Bit;
            _anyMask |= Component<T3>.Bit;
            _anyMask |= Component<T4>.Bit;
            _anyMask |= Component<T5>.Bit;

            return this;
        }

        public ComponentAspect WithAny<T1, T2, T3, T4, T5, T6>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
            where T6 : Component
        {
            _anyMask |= Component<T1>.Bit;
            _anyMask |= Component<T2>.Bit;
            _anyMask |= Component<T3>.Bit;
            _anyMask |= Component<T4>.Bit;
            _anyMask |= Component<T5>.Bit;
            _anyMask |= Component<T6>.Bit;

            return this;
        }

        #endregion

        // name ??
        #region ExcludeWith

        public ComponentAspect ExcludeWith<T1>()
            where T1 : Component
        {
            _excludeMask |= Component<T1>.Bit;
            return this;
        }

        public ComponentAspect ExcludeWith<T1, T2>()
            where T1 : Component
            where T2 : Component
        {
            _excludeMask |= Component<T1>.Bit;
            _excludeMask |= Component<T2>.Bit;
            return this;
        }

        public ComponentAspect ExcludeWith<T1, T2, T3>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            _excludeMask |= Component<T1>.Bit;
            _excludeMask |= Component<T2>.Bit;
            _excludeMask |= Component<T3>.Bit;

            return this;
        }

        public ComponentAspect ExcludeWith<T1, T2, T3, T4>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
        {
            _excludeMask |= Component<T1>.Bit;
            _excludeMask |= Component<T2>.Bit;
            _excludeMask |= Component<T3>.Bit;
            _excludeMask |= Component<T4>.Bit;

            return this;
        }

        public ComponentAspect ExcludeWith<T1, T2, T3, T4, T5>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
        {
            _excludeMask |= Component<T1>.Bit;
            _excludeMask |= Component<T2>.Bit;
            _excludeMask |= Component<T3>.Bit;
            _excludeMask |= Component<T4>.Bit;
            _excludeMask |= Component<T5>.Bit;

            return this;
        }

        public ComponentAspect ExcludeWith<T1, T2, T3, T4, T5, T6>()
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
            where T6 : Component
        {
            _excludeMask |= Component<T1>.Bit;
            _excludeMask |= Component<T2>.Bit;
            _excludeMask |= Component<T3>.Bit;
            _excludeMask |= Component<T4>.Bit;
            _excludeMask |= Component<T5>.Bit;
            _excludeMask |= Component<T6>.Bit;

            return this;
        }

        #endregion

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("All: ").AppendLine(_allMask.ToString());
            builder.Append("Any: ").AppendLine(_anyMask.ToString());
            builder.Append("Exclude: ").AppendLine(_excludeMask.ToString());

            return builder.ToString();
        }
    }
}