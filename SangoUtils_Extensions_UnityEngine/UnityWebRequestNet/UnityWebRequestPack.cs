using SangoUtils_Extensions_UnityEngine.Service;
using System;
using System.Text.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace SangoUtils_Extensions_UnityEngine.UnityWebRequestNet
{
    public enum UnityWebRequestType
    {
        Get,
        Post,
        Put
    }

    public abstract class UnityWebRequestPack
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public UnityWebRequestType HttpType { get; set; }
        public Type DataType { get; set; }
        public string Parame { get; set; }
        public int TryCount { get; set; }
        public UnityWebRequest WebRequest { get; set; }

        public abstract void OnDataReceived(string dataStr, int code, int messageId);
    }

    public class UnityWebRequestPack<T> : UnityWebRequestPack where T : class
    {
        public override void OnDataReceived(string dataStr, int code, int messageId)
        {
            Debug.Log("HttpMessageId:[" + messageId + "], ReceivedStr: " + dataStr);
            UnityWebRequestResponseData dataInfo = JsonSerializer.Deserialize<UnityWebRequestResponseData>(dataStr);
            if (dataInfo != null)
            {
                if (dataInfo.res == 0)
                {
                    if (!string.IsNullOrEmpty(dataInfo.data))
                    {
                        T data;
                        if (typeof(T).Name == "String")
                        {
                            data = dataInfo.data as T;
                        }
                        else
                        {
                            data = JsonSerializer.Deserialize<T>(dataInfo.data);
                        }
                        if (data != null)
                        {
                            UnityWebRequestService.Instance?.HttpBroadcast<T>(data, messageId, dataInfo.res);
                        }
                    }
                    else
                    {
                        UnityWebRequestService.Instance?.HttpBroadcast<T>(null, messageId, dataInfo.res);
                    }
                }
                else
                {
                    UnityWebRequestService.Instance?.HttpBroadcast<T>(null, messageId, dataInfo.res);
                }
            }

        }
    }

    public class UnityWebRequestResponseData
    {
        public int res { get; set; } = 0;
        public string data { get; set; } = "";
    }
}
