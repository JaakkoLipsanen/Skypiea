using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Flai.General;
using Flai.IO;

namespace Flai.CBES
{
    // todo: i could be a super uber hacker and use reflection to make Prefab to have generic parameters so that the entity templates themselves
    // >> wouldn't have to have so awful code.. or actually that doesn't even require reflection, if all prefabs inherited from
    // >> Prefab<T1>, Prefab<T1, T2>, Prefab<T1, T2, T3> etc. that would still require some plumbing though
    public abstract class Prefab
    {
        protected abstract void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters);

        #region Static

        private static readonly Dictionary<Type, Prefab> Prefabs = new Dictionary<Type, Prefab>();

        // should it be possible to give parameters to prefab in the constructor..? or create/add them by user?
        // not sure if this is a good idea
        static Prefab()
        {
            foreach (Type type in TypeHelper.FindTypesInheritingFrom<Prefab>(AssemblyHelper.GetAllNonFrameworkAssemblies(), false))
            {
                Prefab.Prefabs.Add(type, (Prefab)Activator.CreateInstance(type));
            }
        }

        internal static Entity BuildEntity<T>(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
            where T : Prefab, new()
        {
            entity.PrefabID = Prefab<T>.ID;
            Prefab.Prefabs[typeof(T)].BuildEntity(entityWorld, entity, parameters);

            return entity;
        }

        internal static Entity LoadEntityFromFile<T>(EntityWorld entityWorld, Entity entity, BinaryReader reader)
            where T : LoadablePrefab, new()
        {
            return LoadablePrefab.LoadEntityFromFile<T>(entityWorld, entity, reader);
        }

        internal static Entity BuildEntity(string prefabName, EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            var kvp = Prefab.Prefabs.First(x => x.Key.Name == prefabName);
            entity.PrefabID = PrefabHelper.GetPrefabTypeID(kvp.Key);
            kvp.Value.BuildEntity(entityWorld, entity, parameters);

            return entity;
        }

        internal static Entity LoadEntityFromFile(string prefabName, EntityWorld entityWorld, Entity entity, BinaryReader reader)
        {
            return LoadablePrefab.LoadEntityFromFile(prefabName, entityWorld, entity, reader);
        }

        #endregion
    }

    // Prefab that is possible to load from file (BinaryReader)
    public abstract class LoadablePrefab : Prefab
    {
        // by default loadable prefabs just throw an exception
        protected override void BuildEntity(EntityWorld entityWorld, Entity entity, ParameterCollection parameters)
        {
            throw new NotSupportedException("");
        }

        protected abstract void LoadEntityFromFile(EntityWorld entityWorld, Entity entity, BinaryReader reader);

        #region Static

        private static readonly Dictionary<Type, LoadablePrefab> LoadablePrefabs = new Dictionary<Type, LoadablePrefab>();

        // not sure if this is a good idea
        static LoadablePrefab()
        {
            foreach (Type type in TypeHelper.FindTypesInheritingFrom<LoadablePrefab>(AssemblyHelper.GetAllNonFrameworkAssemblies(), false))
            {
                LoadablePrefab.LoadablePrefabs.Add(type, (LoadablePrefab)Activator.CreateInstance(type));
            }
        }

        new internal static Entity LoadEntityFromFile<T>(EntityWorld entityWorld, Entity entity, BinaryReader reader)
            where T : LoadablePrefab, new()
        {
            entity.PrefabID = Prefab<T>.ID;

            LoadablePrefab prefab = LoadablePrefab.LoadablePrefabs[typeof(T)];
            prefab.LoadEntityFromFile(entityWorld, entity, reader);
            return entity;
        }

        new internal static Entity LoadEntityFromFile(string prefabName, EntityWorld entityWorld, Entity entity, BinaryReader reader)
        {
            var kvp = LoadablePrefab.LoadablePrefabs.First(x => x.Key.Name == prefabName);
            LoadablePrefab prefab = LoadablePrefab.LoadablePrefabs[kvp.Key];
            entity.PrefabID = PrefabHelper.GetPrefabTypeID(kvp.Key);
            prefab.LoadEntityFromFile(entityWorld, entity, reader);

            return entity;
        }

        #endregion
    }

    // public?
    internal static class Prefab<T>
        where T : Prefab
    {
        public static readonly uint ID = TypeID<Prefab>.GetID<T>();
    }

    internal static class PrefabHelper
    {
        private static readonly Dictionary<Type, uint> PrefabTypeBits = new Dictionary<Type, uint>();

        // use reflection to get reference to generic Prefab<T> and get the "SystemBit" value from it
        internal static uint GetPrefabTypeID(Type type)
        {
            uint id;
            if (!PrefabHelper.PrefabTypeBits.TryGetValue(type, out id))
            {
                // semi slow but done only once per type shouldn't matter
                Type genericType = typeof(Prefab<>).MakeGenericType(type);
                FieldInfo fieldInfo = genericType.GetField("SystemBit", BindingFlags.Public | BindingFlags.Static);
                id = (uint)fieldInfo.GetValue(null);

                PrefabHelper.PrefabTypeBits[type] = id;
            }

            return id;
        }
    }
}