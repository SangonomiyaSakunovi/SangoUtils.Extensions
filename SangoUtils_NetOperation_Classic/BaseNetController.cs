using SangoNetProtocol_Classic;

namespace SangoUtils_NetOperation_Classic
{
    public abstract class BaseNetController : BaseNetOperation
    {
        protected abstract void DefaultOperationEvent(BaseNetClientPeer peer);

        public virtual void OnInit(int netOperationCode, NetServerOperationHandler handler)
        {
            NetOperationCode = netOperationCode;
            handler.AddNetController(this);
        }

        public virtual void OnDispose(NetServerOperationHandler handler)
        {
            handler.RemoveNetController(this);
        }
    }
}
