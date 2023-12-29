using SangoNetProtol;
using SangoScripts_Server.Net;
using SangoScripts_Server.Utils;

namespace SangoScripts_Server
{
    public abstract class BaseNetHandler
    {
        public NetOperationCode NetOperationCode { get; private set; }

        public abstract void OnOperationRequest(string message, ClientPeer peer);

        public virtual void OnInit(NetOperationCode netOperationCode)
        {
            NetOperationCode = netOperationCode;
            NetService.Instance.AddNetHandler(this);
        }

        public virtual void OnDispose()
        {
            NetService.Instance.RemoveNetHandler(this);
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