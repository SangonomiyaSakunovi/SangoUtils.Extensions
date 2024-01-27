using System;
using System.Collections.Generic;
using System.Reflection;

namespace SangoUtils_UDP
{
    public abstract class UdpEventBaseListenPortID
    {
        [UdpEventPortApiKey("ExamplePort")]
        public const int ExamplePort = 52516;

        private static readonly Dictionary<int, string> _idDict = new Dictionary<int, string>();

        public static void AddUdpEventApi<T>() where T : UdpEventBaseListenPortID
        {
            FieldInfo[] fields = typeof(T).GetFields();
            Type attributeType = typeof(UdpEventPortApiKey);
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].IsDefined(attributeType, false))
                {
                    int id = (int)fields[i].GetValue(null);
                    object attribute = fields[i].GetCustomAttributes(attributeType, false)[0];
                    UdpEventPortApiKey? udpEventPortApiKey = attribute as UdpEventPortApiKey;
                    if (udpEventPortApiKey != null)
                    {
                        string api = udpEventPortApiKey.udpEventApi;
                        _idDict[id] = api;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(udpEventPortApiKey));
                    }
                }
            }
        }

        public static string GetUdpEventApi(int udpEventId)
        {
            _idDict.TryGetValue(udpEventId, out var api);
            return api;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class UdpEventPortApiKey : Attribute
    {
        public string udpEventApi;

        public UdpEventPortApiKey(string udpEventApi)
        {
            this.udpEventApi = udpEventApi;
        }
    }
}
