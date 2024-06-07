using System.Text.Json;
using UnityEngine.Networking;

namespace SangoUtils.WebRequests_Unity
{
    public enum UnityWebRequestType
    {
        Get,
        Post,
        Put
    }

    public abstract class UnityWebRequestPack
    {
        public int HttpID { get; set; }
        public string HttpAPI { get; set; }
        public UnityWebRequestType HttpRequestType { get; set; }
        public string Parame { get; set; }
        public int TryCount { get; set; }
        public UnityWebRequest UnityWebRequest { get; set; }

        public abstract void OnDataReceived(string dataStr, int code, int messageId);
    }

    public class UnityWebRequestPack<T> : UnityWebRequestPack where T : class
    {
        public override void OnDataReceived(string dataStr, int code, int messageId)
        {
            T? dataInfo = JsonSerializer.Deserialize<T>(dataStr);
            if (dataInfo != null)
            {
                UnityWebRequestService.Instance?.SendHttpBroadcast<T>(messageId, dataInfo);
            }
        }
    }
}
