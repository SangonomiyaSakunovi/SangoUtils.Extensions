using System;
using System.Collections.Generic;

namespace SangoUtils.Event
{
    internal class EventCallBackHandler
    {
        private readonly Dictionary<string, Action<string>> _eventCallBack_string_string = new Dictionary<string, Action<string>>();

        public void Init()
        {
            _eventCallBack_string_string.Clear();
        }

        public void AddEventCallBack(string eventId, Action<string> eventCallBack)
        {
            if (!_eventCallBack_string_string.ContainsKey(eventId))
            {
                _eventCallBack_string_string.Add(eventId, eventCallBack);
            }
            else
            {
                EventService.Instance.LogErrorFunc?.Invoke($"Event callback is already existed : {eventId}");
            }
        }

        public void RemoveEventCallBack(string eventId)
        {
            if (_eventCallBack_string_string.ContainsKey(eventId))
            {
                _eventCallBack_string_string.Remove(eventId);
            }
        }

        public void InvokeEventCallBack(string eventId, string param)
        {
            if (_eventCallBack_string_string.ContainsKey(eventId))
            {
                _eventCallBack_string_string[eventId]?.Invoke(param);
            }
        }
    }
}
