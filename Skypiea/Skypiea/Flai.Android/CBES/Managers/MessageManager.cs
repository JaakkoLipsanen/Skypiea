using System;
using System.Collections.Generic;
using Flai.CBES.Pools;
using Flai.DataStructures;

namespace Flai.CBES.Managers
{
    // hmm.. not 100% what "in T" (contravariance) means for this.. i guess it's okay..
    public delegate void MessageAction<T>(T message) where T : Message;

    // ... MessageLockQueue generates garbage since it creates new delegates... blahh..
    internal class MessageManager
    {
        private readonly Bag<MessageCollection> _messageCollections = new Bag<MessageCollection>();
        private readonly Queue<MessageBroadcastAction> _messageLockQueue = new Queue<MessageBroadcastAction>();
        private readonly MessagePool _messagePool = new MessagePool();
        private bool _isLocked = false;

        public void BeginLock()
        {
            Ensure.False(_isLocked, "Messages are already blocked!");
            _isLocked = true;
        }

        public void EndLock()
        {
            Ensure.True(_isLocked, "Messages are not blocked!");

            _isLocked = false;
            while (_messageLockQueue.Count != 0)
            {
                MessageBroadcastAction action = _messageLockQueue.Dequeue();
                action.Collection.BroadcastMessage(action.Message, _messagePool);
            }
        }

        // todo: possible some kind of "UnsubscribeToken"? though is that really needed, should be useful only for lambdas since normal funcs
        // can be unsubscribed easily.. maybe its a bit pointless..
        public void Subscribe<T>(MessageAction<T> action)
            where T : Message
        {
            this.GetMessageCollectionFor<T>().AddAction(action);
        }

        // not sure if working
        public bool Unsubscribe<T>(MessageAction<T> action)
            where T : Message
        {
            // should this throw an exception if the action is not found?
            return this.GetMessageCollectionFor<T>().RemoveAction(action);
        }

        // it's now possible to send Message T, then while locked have some system unsubscribe from it and then when the message is sent, the system doesn't get it. but I think it's ok
        public void Broadcast<T>(T message)
            where T : Message
        {
            MessageCollection<T> messageCollection = this.GetMessageCollectionFor<T>();
            if (_isLocked)
            {
                // is this safe? I mean having the messageCollection be in the function and be used in lambda? i guess it should be always..
                _messageLockQueue.Enqueue(new MessageBroadcastAction(messageCollection, message));
            }
            else
            {
                messageCollection.BroadcastMessage(message);
                if (Message<T>.IsPoolable)
                {
                    _messagePool.Store<T>(message as PoolableMessage);
                }
            }
        }

        public void BroadcastFromPool<T>(Action<T> initializeAction)
           where T : PoolableMessage, new()
        {
            T message = this.FetchPoolable<T>();
            if (initializeAction != null)
            {
                initializeAction(message);
            }

            this.Broadcast(message);
        }

        public T FetchPoolable<T>()
            where T : PoolableMessage, new()
        {
            return _messagePool.Fetch<T>();
        }

        public bool HasListeners<T>()
            where T : Message
        {
            MessageCollection<T> messageCollection = (MessageCollection<T>)_messageCollections[Message<T>.ID];
            return messageCollection != null && messageCollection.HasListeners;
        }

        private MessageCollection<T> GetMessageCollectionFor<T>()
            where T : Message
        {
            MessageCollection<T> messageCollection = (MessageCollection<T>)_messageCollections[Message<T>.ID];
            if (messageCollection == null)
            {
                messageCollection = new MessageCollection<T>();
                _messageCollections[Message<T>.ID] = messageCollection;
            }

            return messageCollection;
        }

        #region MessageCollection

        private abstract class MessageCollection { internal abstract void BroadcastMessage(Message message, MessagePool messagePool); }
        private class MessageCollection<T> : MessageCollection
            where T : Message
        {
            private readonly List<MessageAction<T>> _messageCallbacks = new List<MessageAction<T>>();
            private readonly Bag<MessageAction<T>> _broadcastBag = new Bag<MessageAction<T>>();

            public bool HasListeners
            {
                get { return _messageCallbacks.Count > 0; }
            }

            public void AddAction(MessageAction<T> action)
            {
                _messageCallbacks.Add(action);
            }

            public bool RemoveAction(MessageAction<T> action)
            {
                return _messageCallbacks.Remove(action);
            }

            public void BroadcastMessage(T message)
            {
                // use this "broadcast bag" thingy so that if the callback unsubscribes from the event, the for loop still works properly
                _broadcastBag.AddAll(_messageCallbacks);
                for (int i = 0; i < _broadcastBag.Count; i++)
                {
                    _broadcastBag[i](message);
                }

                _broadcastBag.Clear();
            }

            internal override void BroadcastMessage(Message message, MessagePool messagePool)
            {
                this.BroadcastMessage((T)message);
                if (Message<T>.IsPoolable)
                {
                    messagePool.Store<T>((PoolableMessage)message);
                }
            }
        }

        #endregion

        #region MessageBroadcastAction

        private struct MessageBroadcastAction
        {
            public readonly Message Message;
            public readonly MessageCollection Collection;

            public MessageBroadcastAction(MessageCollection collection, Message message)
            {
                this.Collection = collection;
                this.Message = message;
            }
        }

        #endregion

    }
}
