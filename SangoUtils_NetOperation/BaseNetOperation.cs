using System;
using System.Text.Json;

namespace SangoUtils_NetOperation
{
    public abstract class BaseNetOperation
    {
        protected static string SetJsonString(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        protected static T DeJsonString<T>(string str)
        {
            T t;
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
