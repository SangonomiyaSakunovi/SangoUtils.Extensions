using SangoNetProtol;
using System;
using System.Text.Json;

namespace SangoUtils_NetOperation
{
    public abstract class BaseNetOperation
    {
        public NetOperationCode NetOperationCode { get; protected set; } = NetOperationCode.Default;

        protected static string SetJsonString(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        protected static T? DeJsonString<T>(string str) where T : class
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
