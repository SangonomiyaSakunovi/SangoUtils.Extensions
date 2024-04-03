using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace SangoUtils.Extensions_Unity.UnityWebRequestNet
{
    public class UnityWebRequestClient
    {
        private const int _webReqTimeout = 15;
        private const int _webReqTryCount = 3;
        private const string _serviceHost = "localhost";

        private List<UnityWebRequestPack> _sendHttpPacks = new List<UnityWebRequestPack>(10);
        private List<UnityWebRequestPack> _receivedHttpPacks = new List<UnityWebRequestPack>(10);

        private List<UnityWebResourceBasePack> _sendHttpResourcePacks = new List<UnityWebResourceBasePack>(10);
        private List<UnityWebResourceBasePack> _receivedHttpResourcePacks = new List<UnityWebResourceBasePack>(10);

        public void Init()
        {
        }
        public void Clear()
        {
        }

        public void UpdateHttpRequest()
        {
            HandleRequestSend();
            HandleRequestResponsed();
            HandleResourceRequestSend();
            HandleResourceRequestResponsed();
        }

        public void SendHttpRequest<T>(int httpId, UnityWebRequestType httpType, string parame) where T : class
        {
            AddHttpRequest<T>(httpId, httpType, parame, typeof(T));
        }

        public void SendHttpResourceRequest(UnityWebResourceBasePack httpBaseResourcePack)
        {
            AddHttpResourceRequest(httpBaseResourcePack);
        }

        private void AddHttpRequest<T>(int httpId, UnityWebRequestType httpType, string parame, Type dataType) where T : class
        {
            UnityWebRequestPack<T> pack = new UnityWebRequestPack<T>();
            pack.Id = httpId;
            string apiKey = UnityWebRequestBaseId.GetHttpApi(httpId); 
            string serviceHost = _serviceHost;

            pack.Url = serviceHost + apiKey;
            pack.HttpType = httpType;
            pack.DataType = dataType;
            pack.Parame = parame;
            pack.TryCount = _webReqTryCount;

            _sendHttpPacks.Add(pack);
        }

        private void HandleRequestSend()
        {
            if (_sendHttpPacks.Count == 0) return;

            for (int i = 0; i < _sendHttpPacks.Count; i++)
            {
                UnityWebRequestPack pack = _sendHttpPacks[i];
                switch (pack.HttpType)
                {
                    case UnityWebRequestType.Get:
                        {
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                pack.Url = string.Format("{0}?{1}", pack.Url, pack.Parame);
                            }
                            pack.WebRequest = new UnityWebRequest(pack.Url, UnityWebRequest.kHttpVerbGET);
                            pack.WebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.WebRequest.timeout = _webReqTimeout;
                            pack.WebRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                            pack.WebRequest.SetRequestHeader("token", "");
                            pack.WebRequest.SendWebRequest();
                        }
                        break;
                    case UnityWebRequestType.Post:
                        {
                            pack.WebRequest = new UnityWebRequest(pack.Url, UnityWebRequest.kHttpVerbPOST);
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                byte[] databyte = Encoding.UTF8.GetBytes(pack.Parame);
                                pack.WebRequest.uploadHandler = new UploadHandlerRaw(databyte);
                            }
                            pack.WebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.WebRequest.timeout = _webReqTimeout;
                            pack.WebRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                            pack.WebRequest.SetRequestHeader("token", "");
                            pack.WebRequest.SendWebRequest();
                        }
                        break;
                    case UnityWebRequestType.Put:
                        {
                            pack.WebRequest = new UnityWebRequest(pack.Url, UnityWebRequest.kHttpVerbPUT);
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                byte[] databyte = Encoding.UTF8.GetBytes(pack.Parame);
                                pack.WebRequest.uploadHandler = new UploadHandlerRaw(databyte);
                            }
                            pack.WebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.WebRequest.timeout = _webReqTimeout;
                            pack.WebRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                            pack.WebRequest.SetRequestHeader("token", "");
                            pack.WebRequest.SendWebRequest();
                        }
                        break;
                }
                _receivedHttpPacks.Add(pack);
            }
            _sendHttpPacks.Clear();
        }

        private void HandleRequestResponsed()
        {
            if (_receivedHttpPacks.Count == 0)
                return;

            for (int i = _receivedHttpPacks.Count - 1; i >= 0; i--)
            {
                UnityWebRequestPack pack = _receivedHttpPacks[i];
                if (pack.WebRequest == null)
                {
                    _receivedHttpPacks.Remove(pack);
                    continue;
                }

                if (pack.WebRequest.isDone)
                {
                    int responseCode = (int)pack.WebRequest.responseCode;
                    string responseJson = pack.WebRequest.downloadHandler.text;
                    if (responseCode != 200 && --pack.TryCount > 0)
                    {
                        Debug.Log("Try reconnect Id: [" + pack.Id + " ], try times: " + pack.TryCount);

                        _sendHttpPacks.Add(pack);
                        continue;
                    }
                    CheckResponseCode(pack.Id, responseCode, responseJson);
                    pack.OnDataReceived(responseJson, responseCode, pack.Id);
                }
                else if (pack.WebRequest.result == UnityWebRequest.Result.ProtocolError || pack.WebRequest.result == UnityWebRequest.Result.ConnectionError)
                {

                }
                else
                {
                    continue;
                }
                pack.WebRequest.Abort();
                pack.WebRequest.Dispose();
                pack.WebRequest = null;
                _receivedHttpPacks.Remove(pack);
            }
        }

        private void CheckResponseCode(int httpId, int responseCode, string responseJson)
        {
            if (responseCode == 200)
                return;

            int codeType = responseCode / 100;
            string apiKey = UnityWebRequestBaseId.GetHttpApi(httpId);

            switch (codeType)
            {
                case 4:
                    Debug.LogWarning(string.Format("{0} : {1} : ClientError", responseCode, apiKey));
                    break;
                case 5:
                case 6:
                    Debug.LogWarning(string.Format("{0} : {1} : ServerError", responseCode, apiKey));
                    break;
                default:
                    Debug.LogWarning(string.Format("{0} : {1} : UnknownError", responseCode, apiKey));
                    break;
            }
        }

        private void AddHttpResourceRequest(UnityWebResourceBasePack httpBaseResourcePack)
        {
            _sendHttpResourcePacks.Add(httpBaseResourcePack);
        }

        private void HandleResourceRequestSend()
        {
            if (_sendHttpResourcePacks.Count == 0) return;
            for (int i = 0; i < _sendHttpResourcePacks.Count; i++)
            {
                UnityWebResourceBasePack pack = _sendHttpResourcePacks[i];
                pack.TryCount = _webReqTryCount;
                pack.OnRequest();
                _receivedHttpResourcePacks.Add(pack);
            }
            _sendHttpResourcePacks.Clear();
        }

        private void HandleResourceRequestResponsed()
        {
            if (_receivedHttpResourcePacks.Count == 0) return;

            for (int i = _receivedHttpResourcePacks.Count - 1; i >= 0; i--)
            {
                UnityWebResourceBasePack pack = _receivedHttpResourcePacks[i];
                if (pack.WebRequest == null)
                {
                    _receivedHttpResourcePacks.Remove(pack);
                    continue;
                }

                if (pack.WebRequest.isDone)
                {
                    int responseCode = (int)pack.WebRequest.responseCode;
                    if (responseCode != 200 && --pack.TryCount > 0)
                    {
                        _sendHttpResourcePacks.Add(pack);
                        continue;
                    }
                    if (pack.WebRequest.downloadHandler.isDone)
                    {
                        pack.OnResponsed();
                    }
                    else
                    {
                        pack.OnErrored();
                    }
                }

                else if (pack.WebRequest.result == UnityWebRequest.Result.ProtocolError || pack.WebRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    pack.OnErrored();
                }
                else
                {
                    continue;
                }
                _receivedHttpResourcePacks.Remove(pack);
            }
        }

    }
}
