
using Flai.General;

namespace Flai.CBES
{
    // maybe better name? "Message" is pretty damn generic name and it's pretty easy to get name-collisions.
    public abstract class Message
    {
        public object Tag { get; set; }
        // Mask<Message> TypeBit; ? man im getting addicted to these masks :P
    }

    public abstract class PoolableMessage : Message
    {
        protected internal virtual void Cleanup() { }
    }

    internal static class Message<T>
        where T : Message
    {
        public static readonly int ID = (int)TypeID<Message>.GetID<T>();
        public static bool IsPoolable = typeof(PoolableMessage).IsAssignableFrom(typeof(T));
    }

    public static class PoolableMessage<T>
        where T : PoolableMessage
    {
        public static readonly int ID = (int)TypeID<PoolableMessage>.GetID<T>();
    }
}
