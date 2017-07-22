using Flai.DataStructures;

namespace Flai.CBES.Pools
{
    // meh.. using Message<T> atm...
    internal class MessagePool
    {
        private readonly Bag<ObjectPool<PoolableMessage>> _messagePools = new Bag<ObjectPool<PoolableMessage>>();

        public T Fetch<T>()
            where T : PoolableMessage, new()
        {
            return (T)this.GetPool<T>().Fetch();
        }

        public void Store<T>(PoolableMessage message)
            where T : Message
        {
            message.Cleanup();

            // assume that _messagePools[Message<T>.ID] is *NOT* null, since Fetch<T> is called...
            // otherwise this'd need some reflection hacks that ComponentPool has..
            _messagePools[Message<T>.ID].Store(message);
        }

        public void Store<T>(T message)
            where T : PoolableMessage, new()
        {
            message.Cleanup();
            this.GetPool<T>().Store(message);
        }

        private ObjectPool<PoolableMessage> GetPool<T>()
            where T : PoolableMessage, new()
        {
            return _messagePools[Message<T>.ID] ?? (_messagePools[Message<T>.ID] = new ObjectPool<PoolableMessage>(() => new T()));
        }
    }
}
