using SangoNetProtol;

namespace SangoUtils.NetOperation
{
    public abstract class BaseNetController : BaseNetOperation
    {
        protected abstract void DefaultOperationEvent(BaseNetClientPeer peer);

        public virtual void OnInit(NetOperationCode netOperationCode, NetServerOperationHandler handler)
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
