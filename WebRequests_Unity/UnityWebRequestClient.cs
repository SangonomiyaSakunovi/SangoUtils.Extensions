using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace SangoUtils.WebRequests_Unity
{
    public class UnityWebRequestClient
    {
        private int _webReqTimeout = 15;
        private int _webReqTryCount = 3;
        private string _serviceHost = "localhost";

        private string _requestHeader = "application/json";

        private List<UnityWebRequestPack> _sendHttpPacks = new List<UnityWebRequestPack>(10);
        private List<UnityWebRequestPack> _receivedHttpPacks = new List<UnityWebRequestPack>(10);

        private List<UnityWebResourceBasePack> _sendHttpResourcePacks = new List<UnityWebResourceBasePack>(10);
        private List<UnityWebResourceBasePack> _receivedHttpResourcePacks = new List<UnityWebResourceBasePack>(10);

        public void Init(string serverHost, string requestHeader = "application/json", int webReqTimeout = 15, int webReqTryCount = 3)
        {
            _serviceHost = serverHost;
            _requestHeader = requestHeader;
            _webReqTimeout = webReqTimeout;
            _webReqTryCount = webReqTryCount;
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
            AddHttpRequest<T>(httpId, httpType, parame);
        }

        public void SendHttpResourceRequest(UnityWebResourceBasePack httpBaseResourcePack)
        {
            AddHttpResourceRequest(httpBaseResourcePack);
        }

        private void AddHttpRequest<T>(int httpId, UnityWebRequestType httpType, string parame) where T : class
        {
            UnityWebRequestPack pack = new UnityWebRequestPack<T>
            {
                HttpID = httpId
            };
            string api = UnityWebRequestService.Instance.GetHttpApi(httpId);

            pack.HttpAPI = _serviceHost + api;
            pack.HttpRequestType = httpType;

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
                switch (pack.HttpRequestType)
                {
                    case UnityWebRequestType.Get:
                        {
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                pack.HttpAPI = string.Format("{0}?{1}", pack.HttpAPI, pack.Parame);
                            }
                            pack.UnityWebRequest = new UnityWebRequest(pack.HttpAPI, UnityWebRequest.kHttpVerbGET);
                            pack.UnityWebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.UnityWebRequest.timeout = _webReqTimeout;
                            pack.UnityWebRequest.SetRequestHeader("Content-Type", _requestHeader);
                            pack.UnityWebRequest.SetRequestHeader("token", "");
                            pack.UnityWebRequest.SendWebRequest();
                        }
                        break;
                    case UnityWebRequestType.Post:
                        {
                            pack.UnityWebRequest = new UnityWebRequest(pack.HttpAPI, UnityWebRequest.kHttpVerbPOST);
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                byte[] databyte = Encoding.UTF8.GetBytes(pack.Parame);
                                pack.UnityWebRequest.uploadHandler = new UploadHandlerRaw(databyte);
                            }
                            pack.UnityWebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.UnityWebRequest.timeout = _webReqTimeout;
                            pack.UnityWebRequest.SetRequestHeader("Content-Type", _requestHeader);
                            pack.UnityWebRequest.SetRequestHeader("token", "");
                            pack.UnityWebRequest.SendWebRequest();
                        }
                        break;
                    case UnityWebRequestType.Put:
                        {
                            pack.UnityWebRequest = new UnityWebRequest(pack.HttpAPI, UnityWebRequest.kHttpVerbPUT);
                            if (!string.IsNullOrEmpty(pack.Parame))
                            {
                                byte[] databyte = Encoding.UTF8.GetBytes(pack.Parame);
                                pack.UnityWebRequest.uploadHandler = new UploadHandlerRaw(databyte);
                            }
                            pack.UnityWebRequest.downloadHandler = new DownloadHandlerBuffer();
                            pack.UnityWebRequest.timeout = _webReqTimeout;
                            pack.UnityWebRequest.SetRequestHeader("Content-Type", _requestHeader);
                            pack.UnityWebRequest.SetRequestHeader("token", "");
                            pack.UnityWebRequest.SendWebRequest();
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
                if (pack.UnityWebRequest == null)
                {
                    _receivedHttpPacks.Remove(pack);
                    continue;
                }

                if (pack.UnityWebRequest.isDone)
                {
                    int responseCode = (int)pack.UnityWebRequest.responseCode;
                    string responseJson = pack.UnityWebRequest.downloadHandler.text;
                    if (responseCode != 200 && --pack.TryCount > 0)
                    {
                        Debug.Log("Try reconnect Id: [" + pack.HttpID + " ], try times: " + pack.TryCount);

                        _sendHttpPacks.Add(pack);
                        continue;
                    }
                    pack.OnDataReceived(responseJson, responseCode, pack.HttpID);
                }
                else if (pack.UnityWebRequest.result == UnityWebRequest.Result.ProtocolError || pack.UnityWebRequest.result == UnityWebRequest.Result.ConnectionError)
                {

                }
                else
                {
                    continue;
                }
                pack.UnityWebRequest.Abort();
                pack.UnityWebRequest.Dispose();
                pack.UnityWebRequest = null;
                _receivedHttpPacks.Remove(pack);
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
