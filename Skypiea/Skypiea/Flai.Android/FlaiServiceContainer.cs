
using System;
using Microsoft.Xna.Framework;

namespace Flai
{
    // Jesus christ, Micrsoft.Xna.Framework.Game's Services property is readonly.. 
    // Thus, I can't make this class inherit from GameServiceContainer, so I have to wrap it. However, I can still make it implement IServiceProvider
    public class FlaiServiceContainer : IServiceProvider
    {
        private readonly GameServiceContainer _container;
        public FlaiServiceContainer()
            : this(new GameServiceContainer())
        {
        }

        public FlaiServiceContainer(GameServiceContainer container)
        {
            _container = container;
        }


        /// <summary>
        /// Adds service to the container
        /// </summary>
        public void Add<T>(T provider)
        {
            Ensure.NotNull(provider);
            _container.AddService(typeof(T), provider);
        }

        /// <summary>
        /// Gets service from the container
        /// </summary>
        public T Get<T>()
        {
            object service = _container.GetService(typeof(T));
            Ensure.NotNull(service, "Could not find service of type!");
       
            return (T)service;
        }

        /// <summary>
        /// Removes service from the container
        /// </summary>
        public void Remove<T>()
        {
            _container.RemoveService(typeof(T));
        }

        /// <summary>
        /// Returns whether the container contains a service of type T
        /// </summary>
        public bool Contains<T>()
        {
            return _container.GetService(typeof(T)) != null;
        }

        public T TryGet<T>()
        {
            return this.GetService<T>();
        }

        /// <summary>
        /// Tries to get service from the container, and if succeeds, returns true
        /// </summary>
        public bool TryGet<T>(out T service)
        {
            object temp = _container.GetService(typeof(T));
            if (temp == null)
            {
                service = default(T);
                return false;
            }

            service = (T)temp;
            return true;
        }

        #region IServiceProvider Members

        object IServiceProvider.GetService(Type serviceType)
        {
            return _container.GetService(serviceType);
        }

        #endregion   
    }
}
