#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace SangoUtils.Editors
{
    public class EditorHttpRequestEventArgs
    {
        public EditorHttpRequestEventArgs() { }

        public EditorHttpRequestEventArgs(string requestUrl)
        {
            RequestUrl = requestUrl;
        }

        public EditorHttpRequestEventArgs(string requestUrl, Action<string> onMessaged)
        {
            RequestUrl = requestUrl;
            OnMessaged = onMessaged;
        }

        public EditorHttpRequestEventArgs(string requestUrl, Dictionary<string, string> messageParamDict, Action<string>? onMessaged)
        {
            RequestUrl = requestUrl;
            MessageParamDict = messageParamDict;
            OnMessaged = onMessaged;
        }

        public string RequestUrl { get; set; } = "";
        public Dictionary<string, string> MessageParamDict { get; set; } = new Dictionary<string, string>();
        public Action<string>? OnMessaged { get; set; } = null;
    }
}
#endif
