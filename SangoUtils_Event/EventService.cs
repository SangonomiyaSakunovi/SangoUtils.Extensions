using System;

namespace SangoUtils_Event
{
    public class EventService
    {
        private EventListenerHandler _eventHandler;

        private static EventService? _instance;

        public Action<string>? LogErrorFunc;

        public static EventService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventService();
                }
                return _instance;
            }
        }

        public void OnInit()
        {
            _eventHandler = new EventListenerHandler();
            _eventHandler.Init();
        }

        public void OnUpdate()
        {
            _eventHandler.UpdateEventListenerQueue();
        }

        public void OnDispose()
        {
            _eventHandler.Clear();
        }

        public void AddEventListener<T>(Action<IEventMessageBase> eventMessage) where T : IEventMessageBase
        {
            Type eventType = typeof(T);
            int eventId = eventType.GetHashCode();
            AddEventListener(eventId, eventMessage);
        }

        public void AddEventListener(int eventId, Action<IEventMessageBase> eventMessage)
        {
            _eventHandler.AddEventMessageListener(eventId, eventMessage);
        }

        public void RemoveEventListener<T>() where T : IEventMessageBase
        {
            Type eventType = typeof(T);
            int eventId = eventType.GetHashCode();
            RemoveEventListener(eventId);
        }

        public void RemoveEventListener(int eventId)
        {
            _eventHandler.RemoveEventMessageListenerByEventId(eventId);
        }

        public void RemoveTargetListener(object target)
        {
            _eventHandler.RemoveEventMessageListenerByTarget(target);
        }

        public void SendEventMessage(IEventMessageBase eventMessage)
        {
            _eventHandler.SendEventMessage(eventMessage);
        }

        public void PostEventMessage(IEventMessageBase eventMessage)
        {
            _eventHandler.SendEventMessageAsync(eventMessage);
        }
    }
}
