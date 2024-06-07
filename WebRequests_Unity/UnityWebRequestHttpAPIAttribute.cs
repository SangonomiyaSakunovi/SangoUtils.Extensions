using System;

namespace SangoUtils.WebRequests_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class UnityWebRequestHttpAPIAttribute : Attribute
    {
        public string HttpAPI;

        public UnityWebRequestHttpAPIAttribute(string httpAPI)
        {
            HttpAPI = httpAPI;
        }
    }
}
