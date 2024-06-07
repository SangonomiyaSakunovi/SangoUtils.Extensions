using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using UnityEngine;

namespace SangoUtils.WebRequests_Unity
{
    public class UnityWebRequestService : MonoBehaviour
    {
#pragma warning disable CS8625
        private UnityWebRequestClient _httpClient = null;
#pragma warning restore CS8625
        private readonly Dictionary<int, string> _httpIDDict = new Dictionary<int, string>();

        private Dictionary<int, BaseUnityWebRequest_Http> _requestDict = new Dictionary<int, BaseUnityWebRequest_Http>();

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private static UnityWebRequestService? _instance;

        public static UnityWebRequestService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(UnityWebRequestService)) as UnityWebRequestService;
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject("[" + typeof(UnityWebRequestService).FullName + "]");
                        _instance = gameObject.AddComponent<UnityWebRequestService>();
                        gameObject.hideFlags = HideFlags.HideInHierarchy;
                        DontDestroyOnLoad(gameObject);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// You need call this API before using any other APIs.
        /// </summary>
        /// <typeparam name="T">The UnityWebRequestHttpAPIData, which inherited BaseUnityWebRequestHttpAPIData</typeparam>
        /// <param name="serverHost">The host url</param>
        /// <param name="requestHeader">The req header</param>
        /// <param name="webReqTimeout">The req time out</param>
        /// <param name="webReqTryCount">The req try count</param>
        public void InitService<T>(string serverHost, string requestHeader = "application/json", int webReqTimeout = 15, int webReqTryCount = 3) where T : BaseUnityWebRequestHttpAPIData
        {
            _httpClient = new UnityWebRequestClient();
            _httpClient.Init(serverHost, requestHeader, webReqTimeout, webReqTryCount);
            _stringBuilder.Clear();
            AddHttpApi<T>();
        }

        private void Awake()
        {
            if (null != _instance && _instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (_httpClient != null)
            {
                _httpClient.UpdateHttpRequest();
            }
        }

        public void OnDispose()
        {
            _httpClient.Clear();
        }

        public void SendHttpGet<T>(int httpId, Dictionary<string, string> getParameDic) where T : class
        {
            string getParameStr;
            if (getParameDic == null || getParameDic.Count == 0)
                getParameStr = string.Empty;
            else
            {
                bool isFirst = true;
                foreach (var item in getParameDic)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        _stringBuilder.Append('&');
                    }
                    _stringBuilder.Append(Uri.EscapeDataString(item.Key));
                    _stringBuilder.Append('=');
                    _stringBuilder.Append(Uri.EscapeDataString(item.Value));
                }
                getParameStr = _stringBuilder.ToString();
                _stringBuilder.Clear();
            }
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Get, getParameStr);
        }

        public void SendHttpGet<T>(int httpId, string getParameStr) where T : class
        {
            //Protocol: getParameStr = url?parame1Name=parame1Value&parame2Name=parame2Value
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Get, getParameStr);
        }

        public void SendHttpGet<T>(int httpId) where T : class
        {
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Get, string.Empty);
        }

        public void SendHttpPost<T>(int httpId, Dictionary<string, string> postParameDic) where T : class
        {
            string postParameStr;
            if (postParameDic == null || postParameDic.Count == 0)
                postParameStr = string.Empty;
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool isFirst = true;
                foreach (var item in postParameDic)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        stringBuilder.Append('&');
                    }
                    stringBuilder.Append(Uri.EscapeDataString(item.Key));
                    stringBuilder.Append('=');
                    stringBuilder.Append(Uri.EscapeDataString(item.Value));
                }
                postParameStr = stringBuilder.ToString();
            }
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Post, postParameStr);
        }

        public void SendHttpPost<T>(int httpId, string postParameStr) where T : class
        {
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Post, postParameStr);
        }

        public void SendHttpPost<T>(int httpId, object postParame) where T : class
        {
            string postParameStr = JsonSerializer.Serialize(postParame);
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Post, postParameStr);
        }

        public void SendHttpPost<T>(int httpId) where T : class
        {
            _httpClient.SendHttpRequest<T>(httpId, UnityWebRequestType.Post, string.Empty);
        }

        public void SendHttpPut(int httpId, string putParameStr)
        {
            _httpClient.SendHttpRequest<string>(httpId, UnityWebRequestType.Put, putParameStr);
        }

        public void SendHttpPut(int httpId, object putParame)
        {
            string putParameStr = JsonSerializer.Serialize(putParame);
            _httpClient.SendHttpRequest<string>(httpId, UnityWebRequestType.Put, putParameStr);
        }

        public void SendHttpBroadcast<T>(int requestId, T data) where T : class
        {
            if (_requestDict.TryGetValue(requestId, out BaseUnityWebRequest_Http request))
            {
                request.OnOperationResponsed<T>(data);
            }
        }

        public void AddRequest(BaseUnityWebRequest_Http req)
        {
            if (!_requestDict.ContainsKey(req.HttpID))
            {
                _requestDict.Add(req.HttpID, req);
            }
            else
            {
                Debug.LogError("Already has this request.");
            }
        }

        public void RemoveRequest(BaseUnityWebRequest_Http req)
        {
            if (_requestDict.ContainsKey(req.HttpID))
            {
                _requestDict.Remove(req.HttpID);
            }
            else
            {
                Debug.LogError("Already remove this request.");
            }

        }

        public void HttpResource(UnityWebResourceBasePack httpBaseResourcePack)
        {
            _httpClient.SendHttpResourceRequest(httpBaseResourcePack);
        }

        private void AddHttpApi<T>() where T : BaseUnityWebRequestHttpAPIData
        {
            FieldInfo[] fields = typeof(T).GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (Attribute.IsDefined(fields[i], typeof(UnityWebRequestHttpAPIAttribute)))
                {
                    int id = (int)fields[i].GetValue(null);
                    UnityWebRequestHttpAPIAttribute? attribute = (UnityWebRequestHttpAPIAttribute)fields[i].GetCustomAttribute(typeof(UnityWebRequestHttpAPIAttribute));
                    if (attribute != null)
                    {
                        string api = attribute.HttpAPI;
                        if (!_httpIDDict.ContainsKey(id))
                        {
                            _httpIDDict.Add(id, api);
                        }
                        else
                        {
                            throw new Exception("The same id has used in unityWebRequest API.");
                        }
                    }
                }
            }
        }

        public string GetHttpApi(int httpID)
        {
            if (_httpIDDict.TryGetValue(httpID, out var api))
            {
                return api;
            }
            return string.Empty;
        }
    }
}
