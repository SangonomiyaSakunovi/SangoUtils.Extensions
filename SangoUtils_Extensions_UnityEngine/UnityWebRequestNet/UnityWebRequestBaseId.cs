using System;
using System.Collections.Generic;
using System.Reflection;

namespace SangoUtils.Extensions_Unity.UnityWebRequestNet
{
    public abstract class UnityWebRequestBaseId
    {
        [UnityWebRequestApiKey("Register")]
        public const int registerId = 10001;
        [UnityWebRequestApiKey("Login")]
        public const int loginId = 10002;
        [UnityWebRequestApiKey("testPath/path1/path2")]
        public const int testId = 10003;

        private static readonly Dictionary<int, string> _idDict = new Dictionary<int, string>();

        public static void AddHttpApi<T>() where T : UnityWebRequestBaseId
        {
            FieldInfo[] fields = typeof(T).GetFields();
            Type attributeType = typeof(UnityWebRequestApiKey);
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].IsDefined(attributeType, false))
                {
                    int id = (int)fields[i].GetValue(null);
                    object attribute = fields[i].GetCustomAttributes(attributeType, false)[0];
                    UnityWebRequestApiKey? unityWebRequestApiKey = attribute as UnityWebRequestApiKey;
                    if (unityWebRequestApiKey != null)
                    {
                        string api = unityWebRequestApiKey.httpApi;
                        _idDict[id] = api;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(unityWebRequestApiKey));
                    }
                }
            }
        }

        public static string GetHttpApi(int httpId)
        {
            _idDict.TryGetValue(httpId, out var api);
            return api;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class UnityWebRequestApiKey : Attribute
    {
        public string httpApi;

        public UnityWebRequestApiKey(string httpApi)
        {
            this.httpApi = httpApi;
        }
    }
}
