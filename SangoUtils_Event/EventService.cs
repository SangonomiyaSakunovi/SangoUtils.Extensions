using System;
using System.Text;

namespace SangoUtils_Event
{
    public class EventService
    {
        private EventListenerHandler _listenerHandler;
        private EventCallBackHandler _callBackHandler;

        private static EventService? _instance;

        private StringBuilder _sb;

        public Action<string>? LogErrorFunc { get; set; }

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
            _listenerHandler = new EventListenerHandler();
            _listenerHandler.Init();
            _callBackHandler = new EventCallBackHandler();
            _callBackHandler.Init();

            _sb = new StringBuilder();
        }

        public void OnUpdate()
        {
            _listenerHandler.UpdateEventListenerQueue();
        }

        public void OnDispose()
        {
            _listenerHandler.Clear();
        }

        #region EventListener
        public void AddEventListener<T>(Action<IEventMessageBase> eventMessage) where T : IEventMessageBase
        {
            Type eventType = typeof(T);
            int eventId = eventType.GetHashCode();
            AddEventListener(eventId, eventMessage);
        }

        public void AddEventListener(int eventId, Action<IEventMessageBase> eventMessage)
        {
            _listenerHandler.AddEventMessageListener(eventId, eventMessage);
        }

        public void RemoveEventListener<T>() where T : IEventMessageBase
        {
            Type eventType = typeof(T);
            int eventId = eventType.GetHashCode();
            RemoveEventListener(eventId);
        }

        public void RemoveEventListener(int eventId)
        {
            _listenerHandler.RemoveEventMessageListenerByEventId(eventId);
        }

        public void RemoveTargetListener(object target)
        {
            _listenerHandler.RemoveEventMessageListenerByTarget(target);
        }

        public void SendEventMessage(IEventMessageBase eventMessage)
        {
            _listenerHandler.SendEventMessage(eventMessage);
        }

        public void PostEventMessage(IEventMessageBase eventMessage)
        {
            _listenerHandler.SendEventMessageAsync(eventMessage);
        }
        #endregion

        #region EventCallBack
        public void AddEventCallBack<T>(Action<string> cb, out string eventId) where T : class
        {
            Type eventType = typeof(T);
            string className = eventType.Name;
            string cbName = cb.Method.Name;
            _sb.Append(className);
            _sb.Append("_");
            _sb.Append(cbName);
            eventId = _sb.ToString();
            _sb.Clear();
            AddEventCallBack(eventId, cb);
        }

        public void AddEventCallBack(string eventId, Action<string> cb)
        {
            _callBackHandler.AddEventCallBack(eventId, cb);
        }

        public void RemoveEventCallBack<T>(Action<string> cb) where T : class
        {
            Type eventType = typeof(T);
            string className = eventType.Name;
            string cbName = cb.Method.Name;
            _sb.Append(className);
            _sb.Append("_");
            _sb.Append(cbName);
            string eventId = _sb.ToString();
            _sb.Clear();
            RemoveEventCallBack(eventId);
        }

        public void RemoveEventCallBack(string eventId)
        {
            _callBackHandler.RemoveEventCallBack(eventId);
        }

        public void InvokeEventCallBack(string eventId, string param)
        {
            _callBackHandler.InvokeEventCallBack(eventId, param);
        }
        #endregion
    }
}
