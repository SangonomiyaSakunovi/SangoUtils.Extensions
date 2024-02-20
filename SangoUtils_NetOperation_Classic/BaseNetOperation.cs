using SangoNetProtocol_Classic;
using System;
using System.Text.Json;

namespace SangoUtils_NetOperation_Classic
{
    public abstract class BaseNetOperation
    {
        public NetOperationCode NetOperationCode { get; protected set; } = NetOperationCode.Default;

        protected static string ToJson(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        protected static T? FromJson<T>(string str) where T : class
        {
            T? t;
            try
            {
                t = JsonSerializer.Deserialize<T>(str);
            }
            catch (Exception)
            {
                throw;
            }
            return t;
        }
    }
}
