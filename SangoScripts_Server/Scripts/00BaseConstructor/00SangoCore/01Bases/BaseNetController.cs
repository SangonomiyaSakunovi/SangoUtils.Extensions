using SangoNetProtol;
using SangoScripts_Server.Utils;

namespace SangoScripts_Server
{
    public abstract class BaseNetController
    {
        public NetOperationCode NetOperationCode { get; private set; }

        public abstract void DefaultOperationEvent();

        public virtual void OnInit<T>(NetOperationCode netOperationCode, BaseNetService<T> instance) where T : class, new()
        {
            NetOperationCode = netOperationCode;
            instance.AddNetController(this);
        }

        public virtual void OnDispose<T>(BaseNetService<T> instance) where T : class, new()
        {
            instance.RemoveNetController(this);
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
