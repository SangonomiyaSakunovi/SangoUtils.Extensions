using SangoUtils.Extensions_Unity.Service;
using System.Collections.Generic;

namespace SangoUtils.Extensions_Unity.UnityWebRequestNet
{
    public abstract class BaseUnityWebRequestRequest
    {
        public int HttpId { get; private set; }
        protected Dictionary<string, string> _contentDict;

        public virtual void DefaultRequest<T>() where T : class
        {

        }

        protected virtual void SendRequest<T>() where T : class
        {
            UnityWebRequestService.Instance?.HttpPost<T>(HttpId);
        }

        protected virtual void SendRequest<T>(string getParamStr) where T : class
        {
            UnityWebRequestService.Instance?.HttpPost<T>(HttpId, getParamStr);
        }

        protected virtual void SendRequest<T>(Dictionary<string, string> getParameDic) where T : class
        {
            UnityWebRequestService.Instance?.HttpPost<T>(HttpId, getParameDic);
        }

        public virtual void OnOperationResponsed<T>(T data, int resCode) where T : class
        {

        }

        public virtual void OnInit(int httpId)
        {
            HttpId = httpId;
            _contentDict = new Dictionary<string, string>();
            UnityWebRequestService.Instance.AddRequest(this);
        }

        public virtual void OnDispose()
        {
            UnityWebRequestService.Instance.RemoveRequest(this);
        }
    }
}
