using SangoNetProtol;

namespace SangoUtils.NetOperation
{
    public abstract class BaseNetClientBroadcast : BaseNetOperation
    {
        protected abstract void DefaultOperationClientBroadcast(BaseNetClientPeer peer);

        public abstract void OnOperationClientBroadcast(string message, BaseNetClientPeer peer);

        public virtual void OnInit(NetOperationCode netOperationCode, NetServerOperationHandler handler)
        {
            NetOperationCode = netOperationCode;
            handler.AddNetClientBroadcast(this);
        }

        public virtual void OnDispose(NetServerOperationHandler handler)
        {
            handler.RemoveNetClientBroadcast(this);
        }
    }
}
