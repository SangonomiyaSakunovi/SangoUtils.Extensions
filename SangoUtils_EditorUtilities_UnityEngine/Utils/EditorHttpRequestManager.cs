#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace SangoUtils.Editors
{
    public class EditorHttpRequestManager
    {
        private StringBuilder _stringBuilder = new StringBuilder();

        public void SendHttpRequest(EditorHttpRequestEventArgs eventArgs)
        {
            SendHttpRequest(eventArgs.RequestUrl, eventArgs.MessageParamDict, eventArgs.OnMessaged);
        }

        public void SendHttpRequest(string url, Dictionary<string, string> keyValuePairs, Action<string>? onMessaged)
        {
            string message = string.Empty;
            if (keyValuePairs == null || keyValuePairs.Count == 0)
                message = string.Empty;
            else
            {
                bool isFirst = true;
                foreach (var item in keyValuePairs)
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
                    _stringBuilder.Append("=");
                    _stringBuilder.Append(Uri.EscapeDataString(item.Value));
                }
                message = _stringBuilder.ToString();
                _stringBuilder.Clear();
            }
            SendHttpRequest(url, message, onMessaged);
        }

        public void SendHttpRequest(string url, string message, Action<string>? onMessaged)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("Request url is empty.");
                return;
            }
            EditorCoroutineRunner.StartEditorCoroutine(SendRequestCoroutine(url, message, onMessaged));
        }

        private IEnumerator SendRequestCoroutine(string url, string message, Action<string>? onMessaged)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            if (!string.IsNullOrEmpty(message))
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(message);
                request.uploadHandler = new UploadHandlerRaw(bytes);
            }
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            yield return request.SendWebRequest();

            if (request.responseCode != 200)
            {
                Debug.LogError("Request failed with response code: " + request.responseCode);
            }
            else if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string response = request.downloadHandler.text;
                onMessaged?.Invoke(response);
            }
            request.Abort();
            request.Dispose();
        }
    }
}
#endif