using System;
using System.Collections.Generic;

namespace SangoUtils_Event
{
    public class EventListenerHandler
    {
        private static readonly string _lock = "_eventMessageLock";
        private readonly Queue<IEventMessageBase> _eventMessageQueue = new Queue<IEventMessageBase>();
        private readonly EventMessageMap _eventMessageMap = new EventMessageMap();

        public void Init()
        {
            _eventMessageQueue.Clear();
        }
        public void UpdateEventListenerQueue()
        {
            lock (_lock)
            {
                while (_eventMessageQueue.Count > 0)
                {
                    IEventMessageBase eventMessage = _eventMessageQueue.Dequeue();
                    Type eventType = eventMessage.GetType();
                    int eventId = eventType.GetHashCode();
                    InvokeEventMessageListener(eventId, eventMessage);
                }
            }
        }
        public void Clear() { }

        public void AddEventMessageListener(int eventId, Action<IEventMessageBase> eventMessage)
        {
            lock (_lock)
            {
                _eventMessageMap.AddEventMessageListener(eventId, eventMessage);
            }
        }
        public void RemoveEventMessageListenerByEventId(int eventId)
        {
            lock (_lock)
            {
                _eventMessageMap.RemoveEventMessageListener(eventId);
            }
        }
        public void RemoveEventMessageListenerByTarget(object target)
        {
            lock (_lock)
            {
                _eventMessageMap.RemoveTargetListener(target);
            }
        }

        public void SendEventMessageAsync(IEventMessageBase eventMessage)
        {
            lock (_lock)
            {
                _eventMessageQueue.Enqueue(eventMessage);
            }
        }

        public void SendEventMessage(IEventMessageBase eventMessage)
        {
            Type eventType = eventMessage.GetType();
            int eventId = eventType.GetHashCode();
            InvokeEventMessageListener(eventId, eventMessage);
        }

        private void InvokeEventMessageListener(int eventId, IEventMessageBase eventMessage)
        {
            List<Action<IEventMessageBase>> eventMessageList = _eventMessageMap.GetAllEventMessageHandler(eventId);
            if (eventMessageList != null)
            {
                for (int i = 0; i < eventMessageList.Count; i++)
                {
                    eventMessageList[i].Invoke(eventMessage);
                }
            }
        }
    }
}