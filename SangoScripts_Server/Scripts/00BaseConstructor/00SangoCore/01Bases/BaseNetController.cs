using SangoNetProtol;
using SangoScripts_Server.Net;
using SangoScripts_Server.Utils;

namespace SangoScripts_Server
{
    public abstract class BaseNetController
    {
        public NetOperationCode NetOperationCode { get; private set; }

        public abstract void DefaultOperationEvent();

        public virtual void OnInit(NetOperationCode netOperationCode)
        {
            NetOperationCode = netOperationCode;
            NetService.Instance.AddNetController(this);
        }

        public virtual void OnDispose()
        {
            NetService.Instance.RemoveNetController(this);
        }

        protected static string SetJsonString(object ob)
        {
            return JsonUtils.SetJsonString(ob);
        }

        public static T? DeJsonString<T>(string str)
        {
            T? t;
            try
            {
                t = JsonUtils.DeJsonString<T>(str);
            }
            catch (Exception)
            {
                throw;
            }
            return t;
        }
    }
}
