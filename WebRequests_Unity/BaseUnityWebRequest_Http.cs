using System.Collections.Generic;

namespace SangoUtils.WebRequests_Unity
{
    public abstract class BaseUnityWebRequest_Http
    {
        public int HttpID { get; private set; }

        public BaseUnityWebRequest_Http(int httpID)
        {
            HttpID = httpID;
            UnityWebRequestService.Instance.AddRequest(this);
        }

        protected void SendRequest<T>() where T : class
        {
            UnityWebRequestService.Instance?.SendHttpPost<T>(HttpID);
        }

        protected void SendRequest<T>(string getParamStr) where T : class
        {
            UnityWebRequestService.Instance?.SendHttpPost<T>(HttpID, getParamStr);
        }

        protected void SendRequest<T>(Dictionary<string, string> getParameDic) where T : class
        {
            UnityWebRequestService.Instance?.SendHttpPost<T>(HttpID, getParameDic);
        }

        public abstract void OnOperationResponsed<T>(T data) where T : class;        

        public void OnDispose()
        {
            UnityWebRequestService.Instance.RemoveRequest(this);
        }
    }
}
